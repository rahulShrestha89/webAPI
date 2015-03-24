using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Security.Cryptography;
using GameManager.DAL;
using System.Web.Helpers;
using GameManager.Helpers;
using GameManager.Models;

namespace GameManager.Controllers
{
    public class ApiKeyController : ApiController
    {
        private GameManagerContext db = new GameManagerContext();

        public HttpResponseMessage GetApikey(String Email, String password)
        {
            
            if (Email == null || password == null)
            {
                var response = Request.CreateResponse(HttpStatusCode.BadRequest);
                return response;

            }
            else
            {
                bool check = new AccountController().registeredUser(Email, password);
                if (check == true)
                {
                    var ApiKey = db.Users.FirstOrDefault(u => u.Email == Email).ApiKey;
                    User newobj = db.Users.FirstOrDefault(u => u.Email == Email);

                    if(ApiKey == null){
                        var newapikey = "";
                        newapikey = CustomHelpers.GetApiKey();
                        newobj.ApiKey = newapikey;
                        db.SaveChanges();
                    }
                    
                    var response = Request.CreateResponse(HttpStatusCode.OK, ApiKey);
                    return response;
                }
                else
                {
                    var responses = Request.CreateResponse(HttpStatusCode.Forbidden);
                    return responses;
                }
            }
        }
    }
}


