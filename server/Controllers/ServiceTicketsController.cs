using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Yes.Contracts;
using Yes.Services;

namespace Yes.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ServiceTicketsController(
    IServiceTicketService serviceTicketService,
    IValidator<CreateServiceTicketContract> createServiceTicketValidator,
    IValidator<UpdateServiceTicketContract> updateServiceTicketValidator
) : ControllerBase
{
    private readonly IServiceTicketService _serviceTicketService = serviceTicketService;
    private readonly IValidator<CreateServiceTicketContract> _createServiceTicketValidator = createServiceTicketValidator;
    private readonly IValidator<UpdateServiceTicketContract> _updateServiceTicketValidator = updateServiceTicketValidator;

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Get([FromQuery] string? id = null, [FromQuery] string? customerId = null, [FromQuery] string? roomId = null, [FromQuery] string? serviceId = null, [FromQuery] int? status = null)
    {
        var serviceTickets = await _serviceTicketService.Get(id, customerId, roomId, serviceId, status);
        return Ok(serviceTickets);
    }

    [HttpPost]
    [Authorize(Policy = "ManagerOrReceptionist")]
    public async Task<IActionResult> Create([FromBody] CreateServiceTicketContract createServiceTicket)
    {
        var validationResult = await _createServiceTicketValidator.ValidateAsync(createServiceTicket);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        var serviceTicket = await _serviceTicketService.Create(createServiceTicket);
        if (serviceTicket == null) return BadRequest("Service ticket creation failed.");
        return CreatedAtAction(nameof(Get), new { id = serviceTicket.Id }, serviceTicket);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "ManagerOrReceptionist")]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateServiceTicketContract updateServiceTicket)
    {
        var validationResult = await _updateServiceTicketValidator.ValidateAsync(updateServiceTicket);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        var updatedServiceTicket = await _serviceTicketService.Update(id, updateServiceTicket);
        if (updatedServiceTicket == null) return NotFound($"Service ticket with ID {id} not found.");
        return Ok(updatedServiceTicket);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "ManagerOrReceptionist")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        var deletedServiceTicket = await _serviceTicketService.Delete(id);
        if (deletedServiceTicket == null) return NotFound($"Service ticket with ID {id} not found.");
        return Ok(deletedServiceTicket);
    }

    [HttpPut("take/{id}")]
    [Authorize(Policy = "Staff")]
    public async Task<IActionResult> Take([FromRoute] string id)
    {
        var takenServiceTicket = await _serviceTicketService.Take(id);
        if (takenServiceTicket == null) return NotFound($"Service ticket with ID {id} not found.");
        return Ok(takenServiceTicket);
    }

    [HttpPut("close/{id}")]
    [Authorize(Policy = "Staff")]
    public async Task<IActionResult> Close([FromRoute] string id)
    {
        var closedServiceTicket = await _serviceTicketService.Close(id);
        if (closedServiceTicket == null) return NotFound($"Service ticket with ID {id} not found.");
        return Ok(closedServiceTicket);
    }
}
