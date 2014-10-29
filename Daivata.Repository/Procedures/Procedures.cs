using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daivata.Repository
{
    public class Procedures
    {

        public const string GetHomeSliders = "dbo.proc_GetHomeSliders";
        public const string CreateNewDevalaya = "dbo.proc_CreateNewDevalaya";
        public const string GetDevalayaDetails = "dbo.proc_GetDevalayaDetails";
        public const string UpdateDevalayaStatus = "dbo.proc_UpdateDevalayaStatus";
        public const string GetAllDevalayas = "dbo.proc_GetAllDevalayas";
        public const string GetFilteredDevalayas = "dbo.proc_GetFilteredDevalayas";
        public const string UpdateDevalayaThumbnail = "dbo.proc_UpdateDevalayaThumbnail";
        public const string CreateNewAlbum = "dbo.proc_CreateNewAlbum";
        public const string CreateNewGallery = "dbo.proc_CreateNewGallery";
        public const string GetAlbums = "dbo.proc_GetAlbums";
        public const string CreateAccountProfile = "dbo.proc_CreateAccountProfile";
        public const string GetAccountByAliasId = "dbo.proc_GetAccountByAliasId";
        public const string GetAccountDetailsByProfileId = "dbo.proc_GetAccountDetailsByProfileId";
        public const string FollowDevalaya = "dbo.proc_FollowDevalaya";
        public const string IsFollowDevalaya = "dbo.pro_IsFollowDevalaya";
        public const string GetAllFollowingAssociations = "dbo.proc_GetAllFollowingAssociations";
        public const string UpdateAccountProfile = "dbo.proc_UpdateAccountProfile";
        public const string GetAccountSummary = "dbo.proc_GetAccountSummary";

    }
}
