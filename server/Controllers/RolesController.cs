using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Yes.Contracts;
using Yes.Services;

namespace Yes.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolesController(
    IRoleService roleService
) : ControllerBase
{
    private readonly IRoleService _roleService = roleService;

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string? id = null, [FromQuery] string? userId = null)
    {
        var roles = await _roleService.Get(id, userId);
        return Ok(roles);
    }
}
