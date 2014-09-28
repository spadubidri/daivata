using Daivata.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Daivata.UI
{
    public static class SocialSignin
    {

        public static void SigninWithSocial(Account acct){
           HttpCookie authcookie = new HttpCookie(ConfigurationManager.AppSettings["authcookie"]);

            // Need to encrypt this
           authcookie.Value = "name=" + acct.Profile.FirstName + "&profileid=" + acct.ProfileID;

           HttpContext.Current.Response.Cookies.Add(authcookie);
        }

        public static void SigninWithSocial(string acct)
        {
            HttpCookie authcookie = new HttpCookie(ConfigurationManager.AppSettings["authcookie"]);

            // Need to encrypt this
            authcookie.Value = "name=" + acct;

            HttpContext.Current.Response.Cookies.Add(authcookie);
        }
    }
}