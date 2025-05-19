using CleanArchitecture.Api.Framework.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiResultFilter]
    [ApiController]
    public class BaseController : ControllerBase
    {
    }
}
