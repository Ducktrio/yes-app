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
    Task<ServiceTicketContract?> Take(string id);
    Task<ServiceTicketContract?> Close(string id);
}

public class ServiceTicketService(IServiceTicketRepository serviceTicketRepository, IRoomRepository roomRepository, IMapper mapper) : IServiceTicketService
{
    private readonly IServiceTicketRepository _serviceTicketRepository = serviceTicketRepository;
    private readonly IRoomRepository _roomRepository = roomRepository;
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

    public async Task<ServiceTicketContract?> Take(string id)
    {
        var serviceTicket = (await _serviceTicketRepository.Get(id, null)).FirstOrDefault();
        if (serviceTicket == null) return null;
        serviceTicket.Status = 1;
        var takenServiceTicket = await _serviceTicketRepository.Update(serviceTicket);
        return _mapper.Map<ServiceTicketContract>(takenServiceTicket);
    }

    public async Task<ServiceTicketContract?> Close(string id)
    {
        var serviceTicket = (await _serviceTicketRepository.Get(id, null)).FirstOrDefault();
        if (serviceTicket == null) return null;
        serviceTicket.Status = 2;
        var closedServiceTicket = await _serviceTicketRepository.Update(serviceTicket);
        if (serviceTicket.Service_id == "SVC_001" && serviceTicket.Details == "Room checkout cleaning service")
        {
            var room = (await _roomRepository.Get(serviceTicket.Room_id, null, null, null, null, null)).FirstOrDefault();
            if (room != null)
            {
                room.Status = 0;
                await _roomRepository.Update(room);
            }
        }
        return _mapper.Map<ServiceTicketContract>(closedServiceTicket);
    }
}
