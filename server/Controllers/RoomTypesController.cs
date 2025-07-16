using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Yes.Contracts;
using Yes.Services;

namespace Yes.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoomTypesController(
    IRoomTypeService roomTypeService,
    IValidator<CreateRoomTypeContract> createRoomTypeValidator,
    IValidator<UpdateRoomTypeContract> updateRoomTypeValidator
) : ControllerBase
{
    private readonly IRoomTypeService _roomTypeService = roomTypeService;
    private readonly IValidator<CreateRoomTypeContract> _createRoomTypeValidator = createRoomTypeValidator;
    private readonly IValidator<UpdateRoomTypeContract> _updateRoomTypeValidator = updateRoomTypeValidator;

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Get([FromQuery] string? id = null, [FromQuery] string? roomId = null)
    {
        var roomTypes = await _roomTypeService.Get(id, roomId);
        return Ok(roomTypes);
    }

    [HttpPost]
    [Authorize("Manager")]
    public async Task<IActionResult> Create([FromBody] CreateRoomTypeContract createRoomType)
    {
        var validationResult = await _createRoomTypeValidator.ValidateAsync(createRoomType);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        var roomType = await _roomTypeService.Create(createRoomType);
        if (roomType == null) return BadRequest("Room type creation failed.");
        return CreatedAtAction(nameof(Get), new { id = roomType.Id }, roomType);
    }

    [HttpPut("{id}")]
    [Authorize("Manager")]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateRoomTypeContract updateRoomType)
    {
        var validationResult = await _updateRoomTypeValidator.ValidateAsync(updateRoomType);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        var roomType = await _roomTypeService.Update(id, updateRoomType);
        if (roomType == null) return NotFound($"Room type with ID {id} not found.");
        return Ok(roomType);
    }

    [HttpDelete("{id}")]
    [Authorize("Manager")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        var roomType = await _roomTypeService.Delete(id);
        if (roomType == null) return NotFound($"Room type with ID {id} not found.");
        return Ok(roomType);
    }
}
