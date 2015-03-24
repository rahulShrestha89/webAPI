
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GameManager.DAL;
using System.Web.Helpers;
using GameManager.Models;

namespace GameManager.Controllers
{
    public class AccountController : ApiController
    {
        private GameManagerContext db = new GameManagerContext();

        public bool registeredUser(String Email, string password)
        {
            bool isRegistered = false;

            User user = new User();
            user = db.Users.FirstOrDefault(u => u.Email == Email);
            if (user != null)
            {
                if (Crypto.VerifyHashedPassword(user.Password, password))
                {
                    isRegistered = true;
                }
            }
            return isRegistered;
        }

        public bool validApiLogin(string Email, string apikey)
        {
            bool isValid = false;

            User user = new User();
            user = db.Users.FirstOrDefault(u => u.ApiKey == apikey);

            if (user != null)
            {
                isValid = true;
            }

            return isValid;
        }

        public string getRole(string Email, string apikey)
        {
            string role = "";
            int roleid;
            bool validApiLogin = this.validApiLogin(Email, apikey);

            if (validApiLogin)
            {
                User user = new User();
                user = db.Users.FirstOrDefault(u => u.Email == Email && u.ApiKey == apikey);

                if (user != null)
                {
                    roleid = (int)user.Role;
                    Roles e = (Roles)roleid;
                    role = e.ToString();
                }
            }
            return role;
        }
    }
}