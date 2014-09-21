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

            TwitterClientInfo twitterClientInfo = new TwitterClientInfo();
            twitterClientInfo.ConsumerKey = ConsumerKey; //Read ConsumerKey out of the app.config
            twitterClientInfo.ConsumerSecret = ConsumerSecret; //Read the ConsumerSecret out the app.config

            TwitterService twitterService = new TwitterService(twitterClientInfo);

            //if (string.IsNullOrEmpty(AccessToken) || string.IsNullOrEmpty(AccessTokenSecret))
            //{
                //Now we need the Token and TokenSecret

                //Firstly we need the RequestToken and the AuthorisationUrl
                OAuthRequestToken requestToken = twitterService.GetRequestToken();
                string authUrl = twitterService.GetAuthorizationUri(requestToken).ToString();
                return authUrl;
            //}
            //return "";
        }

        public string GetUserData()
        {

            TwitterClientInfo twitterClientInfo = new TwitterClientInfo();
            twitterClientInfo.ConsumerKey = ConsumerKey; //Read ConsumerKey out of the app.config
            twitterClientInfo.ConsumerSecret = ConsumerSecret; //Read the ConsumerSecret out the app.config

            //TwitterService twitterService = new TwitterService(twitterClientInfo);

            //OAuthAccessToken accessToken = twitterService.GetAccessToken(requestToken, pin);

            //string token = accessToken.Token; //Attach the Debugger and put a break point here
            //string tokenSecret = accessToken.TokenSecret; //And another Breakpoint here

            return "";
        }
    }
}