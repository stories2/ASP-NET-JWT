using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP_NET_JWT.Models
{
    public class UserRepository
    {
        public List<User> simpleUserList;
        public UserRepository()
        {
            simpleUserList = new List<User>();
            simpleUserList.Add(new User() { userName = "Anduin Lothar", password = "wow_alliance" });
            simpleUserList.Add(new User() { userName = "Sylvanas Windrunner", password = "wow_horde" });
        }

        public User GetUser(string userName)
        {
            try
            {
                return simpleUserList.First(user => user.userName.Equals(userName));
            }
            catch(Exception error)
            {
                return null;
            }
        }
    }
}