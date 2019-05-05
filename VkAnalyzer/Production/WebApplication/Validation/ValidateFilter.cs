using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApplication.Validation
{
	/// <inheritdoc />
	/// <summary>
	/// Filter for returning a result if the given model to a controller does not pass validation
	/// </summary>
	public class ValidatorActionFilter : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (!filterContext.ModelState.IsValid)
			{
				filterContext.Result = new BadRequestObjectResult(filterContext.ModelState);
			}
		}
	}
}