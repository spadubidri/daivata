using Daivata.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Daivata.Entities;

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

            return View(devalayaDetails);
        }

        public ActionResult Event(string devalayaId)
        {
            return View();
        }
    }
}
