using System;
using AutoMapper;
using Yes.Contracts;
using Yes.Models;
using Yes.Repositories;

namespace Yes.Services;

public interface IServiceTicketService
{
    Task<List<ServiceTicketContract>> Get(string? id = null, string? customerId = null, string? roomId = null, string? serviceId = null, int? status = null);
    Task<ServiceTicketContract?> Create(CreateServiceTicketContract createServiceTicket);
    Task<ServiceTicketContract?> Update(string id, UpdateServiceTicketContract updateServiceTicket);
    Task<ServiceTicketContract?> Delete(string id);
}

public class ServiceTicketService(IServiceTicketRepository serviceTicketRepository, IMapper mapper) : IServiceTicketService
{
    private readonly IServiceTicketRepository _serviceTicketRepository = serviceTicketRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<List<ServiceTicketContract>> Get(string? id = null, string? customerId = null, string? roomId = null, string? serviceId = null, int? status = null)
    {
        return _mapper.Map<List<ServiceTicketContract>>(await _serviceTicketRepository.Get(id, customerId));
    }

    public async Task<ServiceTicketContract?> Create(CreateServiceTicketContract createServiceTicket)
    {
        var serviceTicket = await _serviceTicketRepository.Create(_mapper.Map<ServiceTicket>(createServiceTicket));
        return _mapper.Map<ServiceTicketContract>(serviceTicket);
    }

    public async Task<ServiceTicketContract?> Update(string id, UpdateServiceTicketContract updateServiceTicket)
    {
        var existingServiceTicket = (await _serviceTicketRepository.Get(id, null)).FirstOrDefault();
        if (existingServiceTicket == null) return null;
        _mapper.Map(updateServiceTicket, existingServiceTicket);
        var updatedServiceTicket = await _serviceTicketRepository.Update(existingServiceTicket);
        return _mapper.Map<ServiceTicketContract>(updatedServiceTicket);
    }

    public async Task<ServiceTicketContract?> Delete(string id)
    {
        var deletedServiceTicket = await _serviceTicketRepository.Delete(id);
        return _mapper.Map<ServiceTicketContract>(deletedServiceTicket);
    }
}
