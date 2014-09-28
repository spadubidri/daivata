using Daivata.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Daivata.Entities;
using System.Configuration;
using Daivata.Storage;
using System.Security.Claims;

namespace Daivata.UI
{
    public class DetailsController : Controller
    {
        //
        // GET: /Details/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Devalaya(string id)
        {
            DevalayaListingRepository repository = new DevalayaListingRepository();
            Devalaya devalayaDetails = repository.Get(Guid.Parse(id));

            AlbumRepository albRepo = new AlbumRepository();
            devalayaDetails.Albums =  albRepo.GetAlbums(Guid.Parse(id));

            return View(devalayaDetails);
        }

        public ActionResult Event(string devalayaId)
        {
            return View();
        }

        //public ActionResult CreateAlbum()
        //{
        //    return null;
        //}

        public ActionResult UploadAlbumPhotos()
        {
            return null;
        }

        public ActionResult LinkAVideo(FormCollection data)
        {

            ActionResult result;
            try
            {
                Guid userId = LoggedinUser.GetLoggedinUserProfileId();

                string albumTitle = data["videodescription"];
                
                string youtubelink = data["videolink"];
                Guid associationId = Guid.Parse(data["associationId"]);
                AlbumRepository repository = new AlbumRepository();
                Album album = repository.CreateAlbum(albumTitle, userId);

                if (album.ID > 0)
                {
                    // create gallery Item
                    repository.CreateGallery(album.ID, "V", userId, youtubelink, associationId);
                }
                result = new JsonResult() { Data = JsonHelper.GetStatusForm(true, "success") };
            }
            catch (Exception ex)
            {
                result = new JsonResult() { Data = JsonHelper.GetStatusForm(false, "failure") };
            }

            return result;
        }

        public ActionResult CreateAlbum(FormCollection data)
        {
            ActionResult result;
            try
            {
                string albumTitle = data["albumtitle"];
                Guid userId = LoggedinUser.GetLoggedinUserProfileId(); // take it from context .Guid.Parse(data[""]);
                AlbumRepository repository = new AlbumRepository();
                Album album = repository.CreateAlbum(albumTitle, userId);

                result = new JsonResult() { Data = JsonHelper.GetStatusForm(true, album.ID.ToString()) };
            }
            catch (Exception ex) 
            {
                result = new JsonResult() { Data = JsonHelper.GetStatusForm(false, "failure") };
            }
            return result;
        }

        [HttpPost]
        public ActionResult UploadPhotoGallery(string Id, string associationId)
        {
            //string associationId = "b244b792-518c-4c4e-a3fc-7287875ecfec";
            ActionResult result;
            try
            {
                for (int cntFiles = 0; cntFiles < HttpContext.Request.Files.Count; cntFiles++ )
                {
                    var httpPostedFile = HttpContext.Request.Files[cntFiles];

                    // Get the uploaded image from the Files collection

                    if (httpPostedFile != null)
                    {
                        string[] fileSplitter = httpPostedFile.FileName.Split('.');
                        string fileExtension = fileSplitter[fileSplitter.Length - 1];
                        string fileName = Guid.NewGuid() + "." + fileExtension;
                        string filetoSave = ConfigurationManager.AppSettings["StorageBaseURL"] + fileName;
                        StorageUtils.UploadThumbnail(fileName, httpPostedFile.InputStream);
                        AlbumRepository repository = new AlbumRepository();
                        repository.CreateGallery(Convert.ToInt64(Id), "P", LoggedinUser.GetLoggedinUserProfileId(), filetoSave, Guid.Parse(associationId));

                    }
                }


                result = new JsonResult() { Data = JsonHelper.GetStatusForm(true, "success") };
            }
            catch (Exception ex)
            {
                result = new JsonResult() { Data = JsonHelper.GetStatusForm(false, "failure") };
            }

            return result;
        }


    }
}
