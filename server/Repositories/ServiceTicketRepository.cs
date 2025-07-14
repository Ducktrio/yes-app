using System;
using Microsoft.EntityFrameworkCore;
using Yes.Data;
using Yes.Models;

namespace Yes.Repositories;

public interface IServiceTicketRepository
{
    Task<bool> Exists(string id);
    Task<List<ServiceTicket>> Get(string? id = null, string? customerId = null, string? roomId = null, string? serviceId = null, int? status = null);
    Task<ServiceTicket?> Create(ServiceTicket entity);
    Task<ServiceTicket?> Update(ServiceTicket entity);
    Task<ServiceTicket?> Delete(string id);
}

public class ServiceTicketRepository(ApplicationDbContext context) : IServiceTicketRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<bool> Exists(string id)
    {
        return await _context.ServiceTickets.AnyAsync(st => st.Id == id);
    }

    public async Task<List<ServiceTicket>> Get(string? id = null, string? customerId = null, string? roomId = null, string? serviceId = null, int? status = null)
    {
        var query = _context.ServiceTickets.Include(st => st.Customer).Include(st => st.Room).Include(st => st.Service).AsQueryable();
        if (!string.IsNullOrEmpty(id)) query = query.Where(st => st.Id == id);
        if (!string.IsNullOrEmpty(customerId)) query = query.Where(st => st.Customer_id == customerId);
        if (!string.IsNullOrEmpty(roomId)) query = query.Where(st => st.Room_id == roomId);
        if (!string.IsNullOrEmpty(serviceId)) query = query.Where(st => st.Service_id == serviceId);
        if (status.HasValue) query = query.Where(st => st.Status == status.Value);
        return await query.ToListAsync();
    }

    public async Task<ServiceTicket?> Create(ServiceTicket entity)
    {
        _context.ServiceTickets.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<ServiceTicket?> Update(ServiceTicket entity)
    {
        var serviceTicket = await _context.ServiceTickets.FindAsync(entity.Id);
        if (serviceTicket == null) return null;

        serviceTicket.Customer_id = entity.Customer_id;
        serviceTicket.Room_id = entity.Room_id;
        serviceTicket.Service_id = entity.Service_id;
        serviceTicket.Details = entity.Details;
        serviceTicket.Status = entity.Status;
        await _context.SaveChangesAsync();
        return serviceTicket;
    }

    public async Task<ServiceTicket?> Delete(string id)
    {
        var serviceTicket = await _context.ServiceTickets.FindAsync(id);
        if (serviceTicket == null) return null;

        _context.ServiceTickets.Remove(serviceTicket);
        await _context.SaveChangesAsync();
        return serviceTicket;
    }
}
