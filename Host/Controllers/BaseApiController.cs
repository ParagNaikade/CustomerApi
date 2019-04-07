using CustomerApi.Contracts.Models.Common;
using Microsoft.AspNetCore.Mvc;

namespace CustomerApi.Host.Controllers
{
    public class BaseApiController : ControllerBase
    {
        public IActionResult Ok<T>(T response)
        {
            var reponse = new ApiSuccessResponse<T>(response);
            return base.Ok(reponse);
        }
    }
}
