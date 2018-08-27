using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP_NET_JWT.Settings
{
    public class DefineManager
    {
        public static readonly int
            EXPIRE_TOKEN_TIME_MIN = 10;

        public static readonly string
            MSG_NOT_FOUND_USER = "The user was not found.",
            MSG_WRONG_USER_LOGIN_INFO = "The username/password combination was wrong.",
            MSG_DIC_RESULT_KEY = "message";
    }
}