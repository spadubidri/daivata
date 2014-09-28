﻿using Daivata.Entities;
using Daivata.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using Daivata.Storage;
using System.Configuration;


namespace Daivata.UI
{
    public class ListingsController : Controller
    {
        //
        // GET: /Listings/

        static IList<DevalayaSummary> devalayaSummary;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Devalayas()
        {
            if (devalayaSummary == null) {
                DevalayaListingRepository repository = new DevalayaListingRepository();
                devalayaSummary = repository.GetAllDevalayas();
            
            }
            return View(devalayaSummary);
        }
        public ActionResult Events()
        {
            return View();
        }

        [AuthorizeAccess]
        public ActionResult CreateNewDevalaya()
        {
            return View(); 
        }

        
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveNewDevalaya(FormCollection data)
        {
            ActionResult result = null;
            try
            {
                
                //Dictionary<string, string> data = Request.Form["data"].Split('&').Select(s => s.Split('=')).GroupBy(o => o.GetValue(0)).Select(g => g.First()).ToDictionary(key => key[0].Trim(), value =>  value[1].Trim());

                Devalaya createRequest = new Devalaya();
                createRequest.Identifier = Guid.NewGuid();
                createRequest.Title = data["title"];
                createRequest.ShortDescription = data["shortDescription"];
                createRequest.Location = data["location"];
                createRequest.Details = data["devalayaDetails"];
                createRequest.TimingDetails = data["timings"];
                createRequest.MapLocation = data["maplocation"];
                createRequest.Contact = data["contact"];
                createRequest.FAQ = data["faq"];
                createRequest.TravelDetails = data["traveldirection"];
                createRequest.Status = "N";
                createRequest.CreatedBy = Guid.NewGuid().ToString(); // to do update logged in user GUID

                // check if there is thumbnail image

                createRequest.ThumbNail = "http://daivata.blob.core.windows.net/gallery/NoImage.jpg";

                DevalayaListingRepository repository = new DevalayaListingRepository();
                Devalaya newDevalaya = repository.CreateNew(createRequest);

                devalayaSummary = null;

                result = new JsonResult() { Data = JsonHelper.GetStatusForm(true, newDevalaya.Identifier.ToString()) };    
            }
            catch (Exception ex)
            {
                // return error
                result = new JsonResult() { Data = JsonHelper.GetStatusForm(false, "failure") };

            }
            return result;
        }


        public ActionResult UploadThumbnail(Guid id)
        {
            ActionResult result;
            var httpPostedFile = HttpContext.Request.Files["uploadedimage"];
            try
            {
                if (HttpContext.Request.Files.AllKeys.Any())
                {
                    // Get the uploaded image from the Files collection
                    
                    if (httpPostedFile != null)
                    {
                        string[] fileSplitter = httpPostedFile.FileName.Split('.');
                        string fileExtension = fileSplitter[fileSplitter.Length-1];
                        string fileName = id + "." + fileExtension;
                        string thumnail = ConfigurationManager.AppSettings["StorageBaseURL"] + fileName;
                        StorageUtils.UploadThumbnail(fileName, httpPostedFile.InputStream);

                        DevalayaListingRepository repository = new DevalayaListingRepository();
                        repository.UpdateThumbnail(id, thumnail);
                    }
                }
                result = new JsonResult() { Data = JsonHelper.GetStatusForm(true, id.ToString()) };   
            }
            catch (Exception ex)
            {
                result = new JsonResult() { Data = JsonHelper.GetStatusForm(false, "failure") };
            }

            return result;
        }

        [HttpPost]
        public ActionResult ConfirmDealayaSubmission(FormCollection data)
        {
            ActionResult result;
            try
            {
                DevalayaListingRepository repository = new DevalayaListingRepository();
                Guid devalayaId = Guid.Parse(data["devalayaId"]);
                repository.UpdateDevalayaStatus(devalayaId,"S");
                result = new JsonResult() { Data = JsonHelper.GetStatusForm(true, "success") };    
            }
            catch (Exception ex)
            {
                result = new JsonResult() { Data = JsonHelper.GetStatusForm(false, "failure") };    
            }

            return result;
        }


        public ActionResult Follow(FormCollection data)
        {
            ActionResult result;
            try
            {
                DevalayaListingRepository repository = new DevalayaListingRepository();
                Guid devalayaId = Guid.Parse(data["devalayaId"]);
                repository.Follow(devalayaId, LoggedinUser.GetLoggedinUserProfileId());
                result = new JsonResult() { Data = JsonHelper.GetStatusForm(true, "success") };
            }
            catch (Exception ex)
            {
                result = new JsonResult() { Data = JsonHelper.GetStatusForm(false, "failure") };
            }

            return result;
        }

        [HttpGet]
        public ActionResult Thankyou(string id)
        {
            return View();
        }
    }
}
