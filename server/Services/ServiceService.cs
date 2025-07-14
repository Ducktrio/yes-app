using System;
using AutoMapper;
using Yes.Contracts;
using Yes.Models;
using Yes.Repositories;

namespace Yes.Services;

public interface IServiceService
{
    Task<List<ServiceContract>> Get(string? id = null, string? serviceTicketId = null);
    Task<ServiceContract?> Create(CreateServiceContract createService);
    Task<ServiceContract?> Update(string id, UpdateServiceContract updateService);
    Task<ServiceContract?> Delete(string id);
}

public class ServiceService(IServiceRepository serviceRepository, IMapper mapper) : IServiceService
{
    private readonly IServiceRepository _serviceRepository = serviceRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<List<ServiceContract>> Get(string? id = null, string? serviceTicketId = null)
    {
        return _mapper.Map<List<ServiceContract>>(await _serviceRepository.Get(id, serviceTicketId));
    }

    public async Task<ServiceContract?> Create(CreateServiceContract createService)
    {
        var service = await _serviceRepository.Create(_mapper.Map<Service>(createService));
        return _mapper.Map<ServiceContract>(service);
    }

    public async Task<ServiceContract?> Update(string id, UpdateServiceContract updateService)
    {
        var existingService = (await _serviceRepository.Get(id, null)).FirstOrDefault();
        if (existingService == null) return null;
        _mapper.Map(updateService, existingService);
        var updatedService = await _serviceRepository.Update(existingService);
        return _mapper.Map<ServiceContract>(updatedService);
    }

    public async Task<ServiceContract?> Delete(string id)
    {
        var deletedService = await _serviceRepository.Delete(id);
        return _mapper.Map<ServiceContract>(deletedService);
    }
}
