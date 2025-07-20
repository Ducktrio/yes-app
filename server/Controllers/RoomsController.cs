using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Yes.Contracts;
using Yes.Services;

namespace Yes.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoomsController(
    IRoomService roomService,
    IValidator<CreateRoomContract> createRoomValidator,
    IValidator<UpdateRoomContract> updateRoomValidator
) : ControllerBase
{
    private readonly IRoomService _roomService = roomService;
    private readonly IValidator<CreateRoomContract> _createRoomValidator = createRoomValidator;
    private readonly IValidator<UpdateRoomContract> _updateRoomValidator = updateRoomValidator;

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Get([FromQuery] string? id = null, [FromQuery] string? roomTypeId = null, [FromQuery] string? label = null, [FromQuery] int? status = null, [FromQuery] string? roomTicketId = null, [FromQuery] string? serviceTicketId = null)
    {
        var rooms = await _roomService.Get(id, roomTypeId, label, status, roomTicketId, serviceTicketId);
        return Ok(rooms);
    }

    [HttpPost]
    [Authorize(Policy = "Manager")]
    public async Task<IActionResult> Create([FromBody] CreateRoomContract createRoom)
    {
        var validationResult = await _createRoomValidator.ValidateAsync(createRoom);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        var room = await _roomService.Create(createRoom);
        if (room == null) return BadRequest("Room creation failed.");
        return CreatedAtAction(nameof(Get), new { id = room.Id }, room);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "Manager")]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateRoomContract updateRoom)
    {
        var validationResult = await _updateRoomValidator.ValidateAsync(updateRoom);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        var room = await _roomService.Update(id, updateRoom);
        if (room == null) return NotFound($"Room with ID {id} not found.");
        return Ok(room);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "Manager")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        var room = await _roomService.Delete(id);
        if (room == null) return NotFound($"Room with ID {id} not found.");
        return Ok(room);
    }
}
