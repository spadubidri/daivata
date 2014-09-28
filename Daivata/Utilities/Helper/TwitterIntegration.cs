using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using TweetSharp;
using TweetSharp.Model;

namespace Daivata.UI
{
    public class TwitterIntegration
    {

        private static string ConsumerSecret
        {
            get { return "mk4quRdKGCKvCZ8V4AOkjtJemS2rijef7WDEG377Gdd5EfYEHQ"; }
        }

        private static string ConsumerKey
        {
            get { return "q6SRL4eA3jr8XUthjdtUIJGTx"; }
        }

        private static string AccessToken
        {
            get { return "197272961-m419lFA0ZqAx4sdo7Am8NLFLQJN3hkxxp3z2AIrU"; }
        }
        private static string AccessTokenSecret
        {
            get { return "wnbqr8KNkdOLsEHkgsDZ9aA6OlgSTb4D0kkJ8dJ6PXHFs"; }
        }

        public string SigninUrl()
        {

            TwitterService service = new TwitterService(ConsumerKey, ConsumerSecret);

            // This is the registered callback URL
            OAuthRequestToken requestToken = service.GetRequestToken(ConfigurationManager.AppSettings["TwitterReturnUrl"]);

            // Step 2 - Redirect to the OAuth Authorization URL
            Uri uri = service.GetAuthorizationUri(requestToken);

            return uri.ToString();

        }

        public TwitterUser GetUserData(string oauthtoken, string oauthverifier)
        {

            var requestToken = new OAuthRequestToken { Token = oauthtoken };

            // Step 3 - Exchange the Request Token for an Access Token
            TwitterService service = new TwitterService(ConsumerKey, ConsumerSecret);
            OAuthAccessToken accessToken = service.GetAccessToken(requestToken, oauthverifier);

            // Step 4 - User authenticates using the Access Token
            service.AuthenticateWith(accessToken.Token, accessToken.TokenSecret);
            
            TwitterUser user = service.VerifyCredentials(new VerifyCredentialsOptions());
            return user;
            
        }
    }
}