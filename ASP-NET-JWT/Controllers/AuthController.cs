using ASP_NET_JWT.Models;
using ASP_NET_JWT.Settings;
using ASP_NET_JWT.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ASP_NET_JWT.Controllers
{
    public class AuthController : ApiController
    {
        //[HttpGet]
        //public IHttpActionResult test()
        //{
        //    return Ok();
        //}

        [HttpPost]
        public HttpResponseMessage Login([FromBody]User user)
        {
            var resultMsgDic = new Dictionary<string, string>();
            try
            {
                User queriedUser = new UserRepository().GetUser(user.userName);
                bool credentials = false;

                if (queriedUser == null)
                {
                    resultMsgDic.Add(DefineManager.MSG_DIC_RESULT_KEY, DefineManager.MSG_NOT_FOUND_USER);
                    return Request.CreateResponse(
                        HttpStatusCode.NotFound,
                        resultMsgDic
                        );
                }

                credentials = queriedUser.password.Equals(user.password);

                switch (credentials)
                {
                    case false:
                        resultMsgDic.Add(DefineManager.MSG_DIC_RESULT_KEY, DefineManager.MSG_WRONG_USER_LOGIN_INFO);
                        return Request.CreateResponse(
                            HttpStatusCode.Forbidden,
                            resultMsgDic
                            );
                    case true:
                        resultMsgDic.Add(DefineManager.MSG_DIC_RESULT_KEY, TokenManager.GenerateToken(queriedUser.userName));
                        return Request.CreateResponse(
                            HttpStatusCode.OK,
                            resultMsgDic
                            );
                }
            }
            catch(Exception err)
            {
                resultMsgDic.Add(DefineManager.MSG_DIC_RESULT_KEY, "Error: " + err);
            }
            return Request.CreateResponse(
                HttpStatusCode.InternalServerError,
                resultMsgDic
                );
        }
    }
}
