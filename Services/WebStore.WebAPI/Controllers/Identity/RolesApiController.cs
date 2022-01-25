using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces;

namespace WebStore.WebAPI.Controllers.Identity;

[ApiController]
[Route(WebAPIAddresses.Identity.Roles)]
public class RolesApiController : ControllerBase
{
    private readonly RoleStore<Role> _RoleStore;

    public RolesApiController(WebStoreDB db)
    {
        _RoleStore = new RoleStore<Role>(db);
    }

    [HttpGet("all")]
    public async Task<IEnumerable<Role>> GetAll() => await _RoleStore.Roles.ToArrayAsync();
}
