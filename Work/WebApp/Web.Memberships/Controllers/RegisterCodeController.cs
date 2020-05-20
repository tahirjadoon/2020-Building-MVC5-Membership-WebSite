using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Web.Memberships.Extensions;

namespace Web.Memberships.Controllers
{
    public class RegisterCodeController : Controller
    {
        // GET: RegisterCode
        public async Task<ActionResult> Register(string code)
        {
            if (Request.IsAuthenticated)
            {
                var userId = HttpContext.GetUserId();

                var registred = await SubscriptionExtensions.RegisterUserSubscriptionCode(userId, code);

                if (!registred) throw new ApplicationException();

                return PartialView("_SiteRegisterCodePartial");
            }
            return View();
        }
    }
}