using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Daivata.Repository;
using Daivata.Entities;
using Daivata.Models;


namespace Daivata.UI
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Start()
        {
            HomePage homedata = new HomePage();
            // populate sldiers
            if (HomeSliderRepository.sliders == null)
            {
                
                HomeSliderRepository.Refresh();
            }

            homedata.sliders = HomeSliderRepository.sliders;

            // Need to take this from cache later
            //DevalayaListingRepository repository = new DevalayaListingRepository();
            //IList<DevalayaSummary> devalayaSummary = repository.GetAllDevalayas();
            //IList<DevalayaSummary> devalayaLatest = new List<DevalayaSummary>();
            //for (int cnt = 0; cnt < 13; cnt++)
            //{
            //    devalayaLatest.Add(devalayaSummary[cnt]);
            //}


            //homedata.latestListings = devalayaLatest;

            return View(homedata);
        }

        public ActionResult Index()
        {
            return View();
        }

        [AuthorizeAccess]
        public ActionResult MyView()
        {
            AccountRepository repo = new AccountRepository();
            IList<Follower> following =  repo.GetAllAssociations(LoggedinUser.GetLoggedinUserProfileId());

            // Need to take this from cache later
            DevalayaListingRepository repository = new DevalayaListingRepository();
            IList<DevalayaSummary> devalayaSummary = repository.GetAllDevalayas();

            return View(LoggedinUser.PopulateMyView(following, devalayaSummary));
        }

        [AuthorizeAccess]
        public ActionResult MyActiveView()
        {
            return View();
        }
    }
}
