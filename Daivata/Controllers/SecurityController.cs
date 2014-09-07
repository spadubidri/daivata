using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Daivata.Controllers
{
    public class SecurityController : Controller
    {
        //
        // GET: /Security/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {

            return View();
        }

    }
}
