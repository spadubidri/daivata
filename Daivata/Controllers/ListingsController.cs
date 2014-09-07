using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Daivata.Controllers
{
    public class ListingsController : Controller
    {
        //
        // GET: /Listings/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Devalayas()
        {
            return View();
        }
    }
}
