using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace Daivata.UI
{
    public class AuthenticationModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.PostAuthenticateRequest += Context_Load;
        }

        public void Dispose()
        {
            
        }


        private void Context_Load(object sender, EventArgs e)
        {
            var authCookie = HttpContext.Current.Request.Cookies[ConfigurationManager.AppSettings["authcookie"]];
            if (authCookie!= null)
                FillPrincipal(authCookie.Value);
        }

        private void FillPrincipal(string context)
        {
            HttpCookie cookie = new HttpCookie("");
            cookie.Value = context;
            var identity = new DaivataIdentity(true);
            foreach (var key in cookie.Values.AllKeys)
            {
                    identity.AddClaim(new Claim(key, cookie[key]));
            }
            HttpContext.Current.User = new ClaimsPrincipal(identity);
        }
    }
}