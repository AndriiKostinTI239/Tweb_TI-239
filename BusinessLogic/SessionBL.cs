using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Core;
using labTW.BusinessLogic.Interfaces;
using labTW.Domain.Entities.Users; // Правильный namespace для ISession

namespace labTW.BusinessLogic
{
    public class SessionBL : UserApi, ISession
    {
        public User GetCurrentUser()
        {
            throw new NotImplementedException();
        }

        public bool IsUserLoggedIn()
        {
            throw new NotImplementedException();
        }

        public UserLoginResp UserLogin(string username, string password)
        {
            throw new NotImplementedException();
        }

        public void UserLogout()
        {
            throw new NotImplementedException();
        }
    }
}
