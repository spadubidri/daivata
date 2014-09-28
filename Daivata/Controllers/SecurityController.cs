using Daivata.Entities;
using Daivata.Repository;
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

                try
                {
                    AccountRepository acctRepo = new AccountRepository();
                    // first check if alias already exists 

                    Account acct = acctRepo.SearchAccount(AliasType.Facebook, me.id);

                    if (acct.AccountId == 0)
                    {
                        // Create a user profile 

                        AccountProfile profile = new AccountProfile();
                        profile.Email = me.email;
                        profile.FirstName = me.name;
                        profile.Source = "Facebook";                        

                        acct = acctRepo.CreateProfile(profile, me.id);
                        SocialSignin.SigninWithSocial(acct);
                        Response.Redirect("~/Security/ConfirmRegistration?ph=new");
                    }
                    else
                    {
                        SocialSignin.SigninWithSocial(acct);
                        Response.Redirect("~/Home/MyView");
                    }
                    

                }catch (Exception ex)
                {

                }
              }
            return null;
        }

        public ActionResult ConfirmRegistration()
        {
            AccountRepository repository = new AccountRepository();
            Account account = repository.GetAccountDetails(LoggedinUser.GetLoggedinUserProfileId());

            return View(account);
        }

        [HttpPost]
        public ActionResult UpdateUser(FormCollection data)
        {

            ActionResult result;
            try
            {
                Guid userId = LoggedinUser.GetLoggedinUserProfileId();

                AccountProfile profile = new AccountProfile();
                profile.FirstName = data["firstname"];
                profile.LastName = data["lastname"];
                profile.Email = data["email"];
                profile.ContactNumber = data["contact"];

                AccountRepository repository = new AccountRepository();

                repository.UpdateProfile(profile, userId);
                
                result = new JsonResult() { Data = JsonHelper.GetStatusForm(true, "success") };
            }
            catch (Exception ex)
            {
                result = new JsonResult() { Data = JsonHelper.GetStatusForm(false, "failure") };
            }
            return result;

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
