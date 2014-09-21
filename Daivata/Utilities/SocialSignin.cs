using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Daivata.UI
{
    public static class SocialSignin
    {
        public static void SigninWithSocial(string name){
           HttpCookie authcookie = new HttpCookie(ConfigurationManager.AppSettings["authcookie"]);
           authcookie.Value = "name=" + name;

           HttpContext.Current.Response.Cookies.Add(authcookie);
        }

    }
}