using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Web.Memberships.Attributes
{
    public class ValidateAjaxAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //must be ajax request
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
            {
                return;
            }
                
            //model state
            var modelState = filterContext.Controller.ViewData.ModelState;

            //check model state
            if (!modelState.IsValid)
            {
                //get errors from the model state
                var errorModel = from x in modelState.Keys
                                where modelState[x].Errors.Count > 0
                                select new
                                {
                                    key = x,
                                    errors = modelState[x].Errors.Select(y => y.ErrorMessage).ToArray()
                                };

                //build json result
                filterContext.Result = new JsonResult()
                {
                    Data = errorModel
                };

                //bad request
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
        }
    }
}