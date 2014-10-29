using Daivata.Entities;
using Daivata.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Daivata.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /Admin/User/

        public ActionResult ViewAll()
        {
            AccountRepository repo = new AccountRepository();
            IList<AccountSummary> summary = repo.GetAllAccounts();
            return View(summary);
        }

    }
}
