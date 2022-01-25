using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using WebStore.Interfaces;

namespace WebStore.WebAPI.Controllers.Identity
{
    [ApiController]
    [Route(WebAPIAddresses.Identity.Users)]
    public class UsersApiController : ControllerBase
    {
    }
}
