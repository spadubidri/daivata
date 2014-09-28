using Daivata.Entities;
using Daivata.Models;
using Daivata.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace Daivata.UI
{
    public class LoggedinUser
    {
        public static Guid GetLoggedinUserProfileId()
        {
            Guid userId = new Guid();

            var identity = System.Web.HttpContext.Current.User != null ? (System.Web.HttpContext.Current.User as ClaimsPrincipal).Identity as ClaimsIdentity : null;
            if (identity != null)
            {
                Claim userIdClaim = identity.Claims.FirstOrDefault(i => i.Type == "profileid");
                if (userIdClaim != null)
                {
                    userId = Guid.Parse(userIdClaim.Value);
                }
            }

            return userId;
        
        }

        public static bool IsFolllowing(Guid associationID)
        {
            DevalayaListingRepository repository = new DevalayaListingRepository();
            Guid devalayaId = associationID;
            return repository.IsFollowing(devalayaId, LoggedinUser.GetLoggedinUserProfileId());
            
        }

        public static UserDashboard PopulateMyView(IList<Follower> followingItems, IList<DevalayaSummary> devalaya)
        {
            IList<DevalayaSummary> summary = new List<DevalayaSummary>();
            foreach (Follower foll in followingItems)
            {
                summary.Add(devalaya.FirstOrDefault(c => c.Identifier == foll.AssociationId));
            }


            UserDashboard dahboard = new UserDashboard();
            dahboard.ListingDetails = summary;

            return dahboard;
        }

        
    }
}