using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Daivata.UI
{
    public class SecurityController : Controller
    {
        //
        // GET: /Security/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {

            return View();
        }

        public ActionResult TwitterLogin()
        {
            TwitterIntegration helper = new TwitterIntegration();
            string signinurl = helper.SigninUrl();
            Response.Redirect(signinurl);

            return null;
        }

        public ActionResult TwitterCallback(string oauth_token, string oauth_verifier)
        {

            TwitterIntegration helper = new TwitterIntegration();
            string loggedinUser = helper.GetUserData(oauth_token, oauth_verifier);
            SocialSignin.SigninWithSocial(loggedinUser);
            Response.Redirect("~/Home/MyView");
            return null;

        }

        public ActionResult FbLogin()
        {
            Facebook.FacebookClient client = new Facebook.FacebookClient();
            var loginUrl = client.GetLoginUrl(new
               {

                   client_id = "629340333850453",

                   redirect_uri = ConfigurationManager.AppSettings["FBReturnUrl"],

                   response_type = "code",

                   scope = "email"
               });

            Response.Redirect(loginUrl.AbsoluteUri);
            return null;
        }

        public ActionResult FbLoginStatus()
        {

            if (Request.QueryString["code"] != null)
            {
                string accessCode = Request.QueryString["code"].ToString();
 
                var fb = new Facebook.FacebookClient();
 
                // throws OAuthException 
                dynamic result = fb.Post("oauth/access_token", new
                {
 
                    client_id = "629340333850453",
 
                    client_secret = "707f3e80a351db4c5637d67a8aa5b4b8",

                    redirect_uri = ConfigurationManager.AppSettings["FBReturnUrl"],
 
                    code = accessCode
 
                });
 
                var accessToken = fb.AccessToken =  result.access_token;

                
                dynamic me = fb.Get("me?fields=friends,name,email");

                string id = me.id; // You can store it in the database
                string name = me.name;
                string email = me.email;
                var friends = me.friends;

                SocialSignin.SigninWithSocial(me.name);

                //HttpCookie authcookie = new HttpCookie(ConfigurationManager.AppSettings["authcookie"]);
                //authcookie.Value = "name=" + me.name;

                //Response.Cookies.Add(authcookie);

                Response.Redirect("~/Home/MyView");
              }
            return null;
        }

        public ActionResult SignOut()
        {
            HttpCookie authcookie = Request.Cookies[ConfigurationManager.AppSettings["authcookie"]];
            if (authcookie != null)
            {
                authcookie.Expires = DateTime.Now.AddDays(-30);
                Response.Cookies.Add(authcookie);
            }
            Response.Redirect("~/");

            return null;
        }

    }
}
