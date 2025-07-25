using System;
using AutoMapper;
using Yes.Contracts;
using Yes.Models;
using Yes.Repositories;

namespace Yes.Services;

public interface ICustomerService
{
    Task<List<CustomerContract>> Get(string? id = null, string? full_name = null, string? roomTicketId = null, string? serviceTicketId = null);
    Task<CustomerContract?> Create(CreateCustomerContract createCustomer);
    Task<CustomerContract?> Update(string id, UpdateCustomerContract updateCustomer);
    Task<CustomerContract?> Delete(string id);
}

public class CustomerService(ICustomerRepository customerRepository, IMapper mapper) : ICustomerService
{
    private readonly ICustomerRepository _customerRepository = customerRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<List<CustomerContract>> Get(string? id = null, string? full_name = null, string? roomTicketId = null, string? serviceTicketId = null)
    {
        return _mapper.Map<List<CustomerContract>>(await _customerRepository.Get(id, full_name, roomTicketId, serviceTicketId));
    }

    public async Task<CustomerContract?> Create(CreateCustomerContract createCustomer)
    {
        var customer = await _customerRepository.Create(_mapper.Map<Customer>(createCustomer));
        return _mapper.Map<CustomerContract>(customer);
    }

    public async Task<CustomerContract?> Update(string id, UpdateCustomerContract updateCustomer)
    {
        var existingCustomer = (await _customerRepository.Get(id, null, null)).FirstOrDefault();
        if (existingCustomer == null) return null;
        _mapper.Map(updateCustomer, existingCustomer);
        var updatedCustomer = await _customerRepository.Update(existingCustomer);
        return _mapper.Map<CustomerContract>(updatedCustomer);
    }

    public async Task<CustomerContract?> Delete(string id)
    {
        var deletedUser = await _customerRepository.Delete(id);
        return _mapper.Map<CustomerContract>(deletedUser);
    }
}
