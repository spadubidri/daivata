using Daivata.Database;
using Daivata.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daivata.Repository
{
    public class AlbumRepository
    {
        public Album CreateAlbum(string albumTitle, Guid userId)
        {
            Query query = new StoredProcedure(Procedures.CreateNewAlbum);
            query["@title"] = albumTitle;
            query["@userId"] = userId;

            Album albumDetails = Database.Database.GetItem<Album>(query);
           // Database.Database.ExecuteQuery(query);

            return albumDetails; 
        }

        public void CreateGallery(long albumId, string galleryType, Guid userId, string fileName, Guid associationId)
        {
            Query query = new StoredProcedure(Procedures.CreateNewGallery);
            query["@albumId"] = albumId;
            query["@type"] = galleryType;
            query["@userId"] = userId;
            query["@fileName"] = fileName;
            query["@associationId"] = associationId;

            Database.Database.ExecuteQuery(query);

        }

        public IList<Album> GetAlbums(Guid associationId)
        {
            Query query = new StoredProcedure(Procedures.GetAlbums);
            query["@associationId"] = associationId;

            IList<Album> albumDetails = new List<Album>();
            IList<Gallery> galleryItems = new List<Gallery>();
           // Database.Database.ExecuteQuery(query);

            Database.Database.ForEach(query, (rs, lc) =>
            {
                //enumerate each of the resultset's returned by the query
                switch (rs.Index)
                {
                    case 0: //accounts
                        albumDetails = rs.GetItems<Album>();
                        break;
                    case 1: //aliases
                        if ((albumDetails != null) && (albumDetails.Count > 0))
                        {
                            galleryItems = rs.GetItems<Gallery>();
                        }
                        break;
                }
            }, System.Data.CommandBehavior.CloseConnection | System.Data.CommandBehavior.SequentialAccess);

            MergeGalleryData(albumDetails, galleryItems);
            return albumDetails; 
        }

        private void MergeGalleryData(IList<Album> album, IList<Gallery> gallery){


            IDictionary<long, IList<Gallery>> aliasesDict = new Dictionary<long, IList<Gallery>>();
            foreach (Gallery gal in gallery)
            {
                if (!aliasesDict.ContainsKey(gal.AlbumId))
                {
                    aliasesDict.Add(gal.AlbumId, new List<Gallery>());
                }
                aliasesDict[gal.AlbumId].Add(gal);
            }

            foreach (Album alb in album)
            {
                alb.GalleryItems = aliasesDict[alb.ID];
            }
				
        }

    }
}
