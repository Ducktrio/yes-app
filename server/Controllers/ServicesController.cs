using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Yes.Contracts;
using Yes.Services;

namespace Yes.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ServicesController(
    IServiceService serviceService,
    IValidator<CreateServiceContract> createServiceValidator,
    IValidator<UpdateServiceContract> updateServiceValidator
) : ControllerBase
{
    private readonly IServiceService _serviceService = serviceService;
    private readonly IValidator<CreateServiceContract> _createServiceValidator = createServiceValidator;
    private readonly IValidator<UpdateServiceContract> _updateServiceValidator = updateServiceValidator;

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Get([FromQuery] string? id = null, [FromQuery] string? serviceTicketId = null)
    {
        var services = await _serviceService.Get(id, serviceTicketId);
        return Ok(services);
    }

    [HttpPost]
    [Authorize(Policy = "Manager")]
    public async Task<IActionResult> Create([FromBody] CreateServiceContract createService)
    {
        var validationResult = await _createServiceValidator.ValidateAsync(createService);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        var service = await _serviceService.Create(createService);
        if (service == null) return BadRequest("Service creation failed.");
        return CreatedAtAction(nameof(Get), new { id = service.Id }, service);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "Manager")]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateServiceContract updateService)
    {
        var validationResult = await _updateServiceValidator.ValidateAsync(updateService);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        var updatedService = await _serviceService.Update(id, updateService);
        if (updatedService == null) return NotFound($"Service with ID {id} not found.");
        return Ok(updatedService);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "Manager")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        var deletedService = await _serviceService.Delete(id);
        if (deletedService == null) return NotFound($"Service with ID {id} not found.");
        return Ok(deletedService);
    }
}
