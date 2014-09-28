using Daivata.Database;
using Daivata.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daivata.Repository
{
    public class DevalayaListingRepository
    {
        public Devalaya CreateNew(Devalaya createRequest)
        {
            Query query = new StoredProcedure(Procedures.CreateNewDevalaya);
            query["@Title"] = createRequest.Title;
            query["@ShortDescription"] = createRequest.ShortDescription;
            query["@Location"] = createRequest.Location;
            query["@Details"] = createRequest.Details;
            query["@MapLocation"] = createRequest.MapLocation;
            query["@Contact"] = createRequest.Contact;
            query["@FAQ"] = createRequest.FAQ;
            query["@TravelDetails"] = createRequest.TravelDetails;
            query["@CreatedBy"] = createRequest.CreatedBy;
            query["@Status"] = createRequest.Status;
            query["@TimingDetails"] = createRequest.TimingDetails;
            query["@ThumbNailImage"] = createRequest.ThumbNail;
            Devalaya newDevalaya = Database.Database.GetItem<Devalaya>(query);
            Database.Database.ExecuteQuery(query);

            return newDevalaya;
        }

        public Devalaya Get(Guid devalayaId)
        {
            Query query = new StoredProcedure(Procedures.GetDevalayaDetails);
            query["@identifier"] = devalayaId;
            Devalaya devalayaDetails = Database.Database.GetItem<Devalaya>(query);
            Database.Database.ExecuteQuery(query);

            return devalayaDetails;
        }

        public void UpdateDevalayaStatus(Guid devalayaId,string status)
        {

            Query query = new StoredProcedure(Procedures.UpdateDevalayaStatus);
            query["@identifier"] = devalayaId;
            query["@Status"] = status;
            Database.Database.ExecuteQuery(query);

        }

        public IList<DevalayaSummary> GetAllDevalayas()
        {
            Query query = new StoredProcedure(Procedures.GetAllDevalayas);
            IList<DevalayaSummary> devalayaDetails = Database.Database.GetItems<DevalayaSummary>(query);
            Database.Database.ExecuteQuery(query);
            return devalayaDetails;
        }

        public void UpdateThumbnail(Guid devalayaId, string thumbnail)
        {
            Query query = new StoredProcedure(Procedures.UpdateDevalayaThumbnail);
            query["@identifier"] = devalayaId;
            query["@ThumNail"] = thumbnail;
            Database.Database.ExecuteQuery(query);
        }

        public void Follow(Guid devalayaId, Guid userId)
        {
            Query query = new StoredProcedure(Procedures.FollowDevalaya);
            query["@identifier"] = devalayaId;
            query["@profileId"] = userId;
            Database.Database.ExecuteQuery(query);
        }

        public bool IsFollowing(Guid devalayaId, Guid userId)
        {
            Query query = new StoredProcedure(Procedures.IsFollowDevalaya);
            query["@identifier"] = devalayaId;
            query["@profileId"] = userId;
            Follower follower = Database.Database.GetItem<Follower>(query);
            Database.Database.ExecuteQuery(query);

            if (follower.ID > 0)
                return true;
            else
                return false;
        }

    }
}
