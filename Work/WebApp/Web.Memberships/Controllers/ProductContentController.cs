using System.Threading.Tasks;
using System.Web.Mvc;
using Web.Memberships.Extensions;

namespace Web.Memberships.Controllers
{
    [Authorize]
    public class ProductContentController : Controller
    {
        // GET: ProductContent
        public async Task<ActionResult> Index(int id)
        {
            var userId = Request.IsAuthenticated ? HttpContext.GetUserId() : null;
            var sections = await SectionExtensions.GetProductSectionsAsync(id, userId);
            return View(sections);
        }

        [Route("ProductContent/Details/{productId:int}/{itemId:int}")]
        public async Task<ActionResult> Details(int productId, int itemId)
        {
            var model = await SectionExtensions.GetContentAsync(productId, itemId);
            return View(model);
        }
    }
}