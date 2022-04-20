using Microsoft.AspNetCore.Mvc;

namespace AjNetCore.Modules.Core.Api
{
	public class BaseApiController : ControllerBase
	{
		protected IActionResult Result(dynamic entity)
		{
			return entity == null ? NotFound() : (IActionResult)Ok(entity);
		}
	}
}