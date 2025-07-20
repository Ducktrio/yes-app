using System;
using Microsoft.EntityFrameworkCore;
using Yes.Data;
using Yes.Models;

namespace Yes.Repositories;

public interface IRoomTicketRepository
{
    Task<bool> Exists(string id);
    Task<List<RoomTicket>> Get(string? id = null, string? customerId = null, string? roomId = null, int? status = null, bool? checkedIn = null);
    Task<RoomTicket?> Create(RoomTicket entity);
    Task<RoomTicket?> Update(RoomTicket entity);
    Task<RoomTicket?> Delete(string id);
}

public class RoomTicketRepository(ApplicationDbContext context) : IRoomTicketRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<bool> Exists(string id)
    {
        return await _context.RoomTickets.AnyAsync(rt => rt.Id == id);
    }

    public async Task<List<RoomTicket>> Get(string? id = null, string? customerId = null, string? roomId = null, int? status = null, bool? checkedIn = null)
    {
        var query = _context.RoomTickets.Include(rt => rt.Customer).Include(rt => rt.Room).AsQueryable();
        if (!string.IsNullOrEmpty(id)) query = query.Where(rt => rt.Id == id);
        if (!string.IsNullOrEmpty(customerId)) query = query.Where(rt => rt.Customer_id == customerId);
        if (!string.IsNullOrEmpty(roomId)) query = query.Where(rt => rt.Room_id == roomId);
        if (status.HasValue) query = query.Where(rt => rt.Status == status.Value);
        if (checkedIn.HasValue)
        {
            if (checkedIn.Value)
                query = query.Where(rt => rt.CheckInDate.HasValue);
            else
                query = query.Where(rt => !rt.CheckInDate.HasValue);
        }
        return await query.ToListAsync();
    }

    public async Task<RoomTicket?> Create(RoomTicket entity)
    {
        _context.RoomTickets.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<RoomTicket?> Update(RoomTicket entity)
    {
        var roomTicket = await _context.RoomTickets.FindAsync(entity.Id);
        if (roomTicket == null) return null;

        roomTicket.Customer_id = entity.Customer_id;
        roomTicket.Room_id = entity.Room_id;
        roomTicket.CheckInDate = entity.CheckInDate;
        roomTicket.CheckOutDate = entity.CheckOutDate;
        roomTicket.Number_of_occupants = entity.Number_of_occupants;
        roomTicket.Status = entity.Status;
        await _context.SaveChangesAsync();
        return roomTicket;
    }

    public async Task<RoomTicket?> Delete(string id)
    {
        var roomTicket = await _context.RoomTickets.FindAsync(id);
        if (roomTicket == null) return null;

        _context.RoomTickets.Remove(roomTicket);
        await _context.SaveChangesAsync();
        return roomTicket;
    }
}
