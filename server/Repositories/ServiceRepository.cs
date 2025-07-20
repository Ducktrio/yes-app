using System;
using Microsoft.EntityFrameworkCore;
using Yes.Data;
using Yes.Models;

namespace Yes.Repositories;

public interface IServiceRepository
{
    Task<bool> Exists(string id);
    Task<List<Service>> Get(string? id = null, string? serviceTicketId = null);
    Task<Service?> Create(Service entity);
    Task<Service?> Update(Service entity);
    Task<Service?> Delete(string id);
}

public class ServiceRepository(ApplicationDbContext context) : IServiceRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<bool> Exists(string id)
    {
        return await _context.Services.AnyAsync(s => s.Id == id);
    }

    public async Task<List<Service>> Get(string? id = null, string? serviceTicketId = null)
    {
        var query = _context.Services.AsQueryable();
        if (!string.IsNullOrEmpty(id)) query = query.Where(s => s.Id == id);
        if (!string.IsNullOrEmpty(serviceTicketId)) query = query.Where(s => s.ServiceTickets.Any(st => st.Id == serviceTicketId));
        return await query.ToListAsync();
    }

    public async Task<Service?> Create(Service entity)
    {
        _context.Services.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Service?> Update(Service entity)
    {
        var service = await _context.Services.FindAsync(entity.Id);
        if (service == null) return null;

        service.Name = entity.Name;
        service.Price = entity.Price;
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Service?> Delete(string id)
    {
        var service = await _context.Services.FindAsync(id);
        if (service == null) return null;

        _context.Services.Remove(service);
        await _context.SaveChangesAsync();
        return service;
    }
}
