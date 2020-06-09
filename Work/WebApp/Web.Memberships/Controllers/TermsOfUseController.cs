using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Memberships.Controllers
{
    public class TermsOfUseController : Controller
    {
        // GET: TermsOfUse
        public ActionResult Legal()
        {
            return View();
        }
    }
}