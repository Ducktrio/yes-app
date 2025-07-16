using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Yes.Contracts;
using Yes.Services;

namespace Yes.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoomTicketsController(
    IRoomTicketService roomTicketService,
    IValidator<CreateRoomTicketContract> createRoomTicketValidator,
    IValidator<UpdateRoomTicketContract> updateRoomTicketValidator
) : ControllerBase
{
    private readonly IRoomTicketService _roomTicketService = roomTicketService;
    private readonly IValidator<CreateRoomTicketContract> _createRoomTicketValidator = createRoomTicketValidator;
    private readonly IValidator<UpdateRoomTicketContract> _updateRoomTicketValidator = updateRoomTicketValidator;

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Get([FromQuery] string? id = null, [FromQuery] string? customerId = null, [FromQuery] string? roomId = null, [FromQuery] int? status = null, [FromQuery] bool? checkedIn = null)
    {
        var roomTickets = await _roomTicketService.Get(id, customerId, roomId, status, checkedIn);
        return Ok(roomTickets);
    }

    [HttpPost]
    [Authorize(Policy = "ManagerOrReceptionist")]
    public async Task<IActionResult> Create([FromBody] CreateRoomTicketContract createRoomTicket)
    {
        var validationResult = await _createRoomTicketValidator.ValidateAsync(createRoomTicket);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        var roomTicket = await _roomTicketService.Create(createRoomTicket);
        if (roomTicket == null) return BadRequest("Room ticket creation failed.");
        return CreatedAtAction(nameof(Get), new { id = roomTicket.Id }, roomTicket);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "ManagerOrReceptionist")]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateRoomTicketContract updateRoomTicket)
    {
        var validationResult = await _updateRoomTicketValidator.ValidateAsync(updateRoomTicket);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        var updatedRoomTicket = await _roomTicketService.Update(id, updateRoomTicket);
        if (updatedRoomTicket == null) return NotFound();
        return Ok(updatedRoomTicket);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "ManagerOrReceptionist")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        var deletedRoomTicket = await _roomTicketService.Delete(id);
        if (deletedRoomTicket == null) return NotFound();
        return Ok(deletedRoomTicket);
    }

    [HttpPut("checkin/{id}")]
    [Authorize(Policy = "ManagerOrReceptionist")]
    public async Task<IActionResult> CheckIn([FromRoute] string id)
    {
        var checkedInRoomTicket = await _roomTicketService.CheckIn(id);
        if (checkedInRoomTicket == null) return NotFound();
        return Ok(checkedInRoomTicket);
    }

    [HttpPut("checkout/{id}")]
    [Authorize(Policy = "ManagerOrReceptionist")]
    public async Task<IActionResult> CheckOut([FromRoute] string id)
    {
        var checkedOutRoomTicket = await _roomTicketService.CheckOut(id);
        if (checkedOutRoomTicket == null) return NotFound();
        return Ok(checkedOutRoomTicket);
    }
}
