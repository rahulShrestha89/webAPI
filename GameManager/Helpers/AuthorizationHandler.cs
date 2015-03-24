using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GameManager.Controllers;
using GameManager.Models;
using System.Net;
using System.Web;
using System.Web.Routing;

namespace GameManager.Helpers
{
    public class AuthorizationHandler
    : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var apikey = "";
            var userid = "";
            var headerstring = request.Headers.ToString();

            string[] headerarray = headerstring.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            foreach (var header in headerarray)
            {
                if (header.Contains("xcmps383authenticationkey"))
                {
                    string[] arr1 = header.Split(':');
                    apikey = arr1[1].Trim();

                }
                if (header.Contains("xcmps383authenticationid"))
                {
                    string[] arr1 = header.Split(':');
                    userid = arr1[1].Trim();
                }
            }

            var isValidRequest = new AccountController().validApiLogin(userid, apikey);

            if (isValidRequest)
            {
                var identity = new GenericIdentity(userid);
                var roles = new AccountController().getRole(userid, apikey);

                var principal = new GenericPrincipal(identity, new[] { roles });
                Thread.CurrentPrincipal = principal;

                if (HttpContext.Current != null)
                {
                    HttpContext.Current.User = principal;
                }
            }
            return base.SendAsync(request, cancellationToken);
        }
    }
}