using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Yes.Contracts;
using Yes.Services;

namespace Yes.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(
    IUserService userService,
    IValidator<CreateUserContract> createUserValidator,
    IValidator<UpdateUserContract> updateUserValidator
) : ControllerBase
{
    private readonly IUserService _userService = userService;
    private readonly IValidator<CreateUserContract> _createUserValidator = createUserValidator;
    private readonly IValidator<UpdateUserContract> _updateUserValidator = updateUserValidator;

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetMe([FromHeader(Name = "Authorization")] string token)
    {
        var tokenArr = token.Split(' ');
        if (tokenArr.Length != 2 || tokenArr[0] != "Bearer") return BadRequest();
        var user = await _userService.GetLoggedInUser(tokenArr[1]);
        if (user == null) return Unauthorized();
        return Ok(user);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Get([FromQuery] string? id = null, [FromQuery] string? role_id = null, [FromQuery] string? username = null)
    {
        var users = await _userService.Get(id, role_id, username);
        return Ok(users);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginContract loginContract)
    {
        var token = await _userService.Login(loginContract);
        if (token == null) return BadRequest();
        return Ok(token);
    }

    [HttpPost]
    [Authorize(Policy = "Manager")]
    public async Task<IActionResult> Create([FromBody] CreateUserContract createUser)
    {
        var validationResult = await _createUserValidator.ValidateAsync(createUser);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        var user = await _userService.Create(createUser);
        if (user == null) return BadRequest("User creation failed.");
        return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "Manager")]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateUserContract updateUser)
    {
        var validationResult = await _updateUserValidator.ValidateAsync(updateUser);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        var user = await _userService.Update(id, updateUser);
        if (user == null) return NotFound($"User with ID {id} not found.");
        return Ok(user);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "Manager")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        var user = await _userService.Delete(id);
        if (user == null) return NotFound($"User with ID {id} not found.");
        return Ok(user);
    }
}
