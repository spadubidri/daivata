using Daivata.Entities;
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
using Daivata.Models;


namespace Daivata.UI
{
    public class ListingsController : Controller
    {
        //
        // GET: /Listings/

        static IDictionary<string, IList <KeyValuePair<int, DevalayaSummary>>> devalayaSummary = new Dictionary<string, IList <KeyValuePair<int, DevalayaSummary>>>();
        static int PageCount;


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Devalayas()
        {
            DevalayaListings listing = new DevalayaListings();
            int pageItems = 8;
            string cacheKeyfilterRequest = "default";

            if (devalayaSummary == null)
                devalayaSummary = new Dictionary<string, IList<KeyValuePair<int, DevalayaSummary>>>();

            if (!String.IsNullOrEmpty(Request.QueryString["title"]) || !String.IsNullOrEmpty(Request.QueryString["duration"]) || !String.IsNullOrEmpty(Request.QueryString["state"]) || !String.IsNullOrEmpty(Request.QueryString["location"]))
            {
                FilterRequest request = new FilterRequest();
                cacheKeyfilterRequest = Request.QueryString["title"] + "~" + Request.QueryString["duration"] + Request.QueryString["state"] + "~" + Request.QueryString["location"];
                if (!devalayaSummary.Keys.Contains(cacheKeyfilterRequest))
                {
                    DevalayaListingRepository repository = new DevalayaListingRepository();
                    request.Title = Request.QueryString["title"];
                    request.Duration = Request.QueryString["duration"];
                    request.State = Request.QueryString["state"];
                    request.Location = Request.QueryString["location"];
                    IList<DevalayaSummary> allOutput = repository.GetFilteredDevalayas(request);
                    PageCount = 0;
                    IList<KeyValuePair<int, DevalayaSummary>> defaultSummary = new List<KeyValuePair<int, DevalayaSummary>>();    
                    int itemCount = 0;
                    // put it in disctionary with page
                    foreach (DevalayaSummary summary in allOutput)
                    {
                        defaultSummary.Add(new KeyValuePair<int, DevalayaSummary>(PageCount, summary));
                        itemCount++;
                        if (itemCount % pageItems == 0)
                        {
                            PageCount++;
                        }

                    }
                    devalayaSummary.Add(cacheKeyfilterRequest, defaultSummary);
                }
            }



            if (!devalayaSummary.Keys.Contains("default")) {
                PageCount = 0;
                IList<KeyValuePair<int, DevalayaSummary>> defaultSummary = new List<KeyValuePair<int, DevalayaSummary>>();
                DevalayaListingRepository repository = new DevalayaListingRepository();
                IList <DevalayaSummary> allOutput = repository.GetAllDevalayas();

                
                int itemCount = 0;
                // put it in disctionary with page
                foreach (DevalayaSummary summary in allOutput)
                {
                    defaultSummary.Add(new KeyValuePair<int, DevalayaSummary>(PageCount, summary));
                    itemCount++;
                    if (itemCount % pageItems == 0) {
                        PageCount++;
                    }
                        
                }
                devalayaSummary.Add("default", defaultSummary);
            }

            IList<KeyValuePair<int, DevalayaSummary>> selectedSummary = devalayaSummary.SingleOrDefault(i => i.Key == cacheKeyfilterRequest).Value;
            if (selectedSummary != null)
            {

                var selectedList = selectedSummary.TakeWhile(i => i.Key == 0);
                foreach (var summary in selectedList)
                {
                    listing.Listings.Add(summary.Value);
                }


                listing.PageCount = (selectedList.Count() / pageItems) + 1;
                listing.Filter = cacheKeyfilterRequest;
            }

            return View(listing);
        }
        

        public ActionResult DevalayasOnScroll(string filter,int pagenumber)
        {

            DevalayaListings listing = new DevalayaListings();

            var selectedList = devalayaSummary.SingleOrDefault(i => i.Key == filter).Value;
            foreach (var summary in selectedList)
            {
                if(summary.Key == pagenumber)
                    listing.Listings.Add(summary.Value);
            }
            listing.PageCount = PageCount;
            return View(listing);
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
                createRequest.State = data["state"];
                createRequest.Country = data["country"];
                createRequest.Details = data["devalayaDetails"];
                createRequest.TimingDetails = data["timings"];
                createRequest.MapLocation = data["maplocation"];
                createRequest.Contact = data["contact"];
                createRequest.FAQ = data["faq"];
                createRequest.TravelDetails = data["traveldirection"];
                createRequest.Status = "N";
                createRequest.References = data["references"];
                createRequest.CreatedBy = LoggedinUser.GetLoggedinUserProfileId().ToString(); // to do update logged in user GUID

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

                        //Stream thumNailImage = ImageThumbNail.CreateThumbnail(httpPostedFile.InputStream, 100, 100);


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
