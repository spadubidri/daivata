using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Daivata.Controllers
{
    public class DetailsController : Controller
    {
        //
        // GET: /Details/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Devalaya(string devalayaId)
        {

            return View();
        }

        public ActionResult Event(string devalayaId)
        {
            return View();
        }
    }
}
