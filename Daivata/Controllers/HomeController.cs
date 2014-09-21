using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Daivata.Repository;
using Daivata.Entities;


namespace Daivata.UI
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            // populate sldiers
            if (HomeSliderRepository.sliders == null)
            {
                
                HomeSliderRepository.Refresh();
            }

            return View(HomeSliderRepository.sliders);
        }

        [AuthorizeAccess]
        public ActionResult MyView()
        {
            return View();
        }

        [AuthorizeAccess]
        public ActionResult MyActiveView()
        {
            return View();
        }
    }
}
