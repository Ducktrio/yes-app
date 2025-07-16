using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Yes.Contracts;
using Yes.Services;

namespace Yes.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomersController(
    ICustomerService customerService,
    IValidator<CreateCustomerContract> createCustomerValidator,
    IValidator<UpdateCustomerContract> updateCustomerValidator
) : ControllerBase
{
    private readonly ICustomerService _customerService = customerService;
    private readonly IValidator<CreateCustomerContract> _createCustomerValidator = createCustomerValidator;
    private readonly IValidator<UpdateCustomerContract> _updateCustomerValidator = updateCustomerValidator;

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Get([FromQuery] string? id = null, [FromQuery] string? roomTicketId = null, [FromQuery] string? serviceTicketId = null)
    {
        var customers = await _customerService.Get(id, roomTicketId, serviceTicketId);
        return Ok(customers);
    }

    [HttpPost]
    [Authorize("ManagerOrReceptionist")]
    public async Task<IActionResult> Create([FromBody] CreateCustomerContract createCustomer)
    {
        var validationResult = await _createCustomerValidator.ValidateAsync(createCustomer);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        var customer = await _customerService.Create(createCustomer);
        if (customer == null) return BadRequest("Customer creation failed.");
        return CreatedAtAction(nameof(Get), new { id = customer.Id }, customer);
    }

    [HttpPut("{id}")]
    [Authorize("ManagerOrReceptionist")]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateCustomerContract updateCustomer)
    {
        var validationResult = await _updateCustomerValidator.ValidateAsync(updateCustomer);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        var customer = await _customerService.Update(id, updateCustomer);
        if (customer == null) return NotFound($"Customer with ID {id} not found.");
        return Ok(customer);
    }

    [HttpDelete("{id}")]
    [Authorize("ManagerOrReceptionist")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        var customer = await _customerService.Delete(id);
        if (customer == null) return NotFound($"Customer with ID {id} not found.");
        return Ok(customer);
    }
}
