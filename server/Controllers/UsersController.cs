using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Yes.Contracts;
using Yes.Services;

namespace Yes.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(
    IAuthenticationService authenticationService,
    IUserService userService,
    IValidator<CreateUserContract> createUserValidator,
    IValidator<UpdateUserContract> updateUserValidator
) : ControllerBase
{
    private readonly IAuthenticationService _authenticationService = authenticationService;
    private readonly IUserService _userService = userService;
    private readonly IValidator<CreateUserContract> _createUserValidator = createUserValidator;
    private readonly IValidator<UpdateUserContract> _updateUserValidator = updateUserValidator;

    [HttpGet("me")]
    public async Task<IActionResult> GetMe([FromHeader(Name = "Authorization")] string token)
    {
        var tokenArr = token.Split(' ');
        if (tokenArr.Length != 2 || tokenArr[0] != "Bearer") return BadRequest();
        var user = await _userService.GetLoggedInUser(tokenArr[1]);
        if (user == null) return Unauthorized();
        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginContract loginContract)
    {
        var token = await _userService.Login(loginContract);
        if (token == null) return BadRequest();
        return Ok(token);
    }
}
