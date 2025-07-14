using System;
using Microsoft.EntityFrameworkCore;
using Yes.Data;
using Yes.Models;

namespace Yes.Repositories;

public interface IRoomRepository
{
    Task<bool> Exists(string id);
    Task<List<Room>> Get(string? id = null, string? roomTypeId = null, string? label = null, int? status = null, string? roomTicketId = null, string? serviceTicketId = null);
    Task<Room?> Create(Room entity);
    Task<Room?> Update(Room entity);
    Task<Room?> Delete(string id);
}

public class RoomRepository(ApplicationDbContext context) : IRoomRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<bool> Exists(string id)
    {
        return await _context.Rooms.AnyAsync(r => r.Id == id);
    }

    public async Task<List<Room>> Get(string? id = null, string? roomTypeId = null, string? label = null, int? status = null, string? roomTicketId = null, string? serviceTicketId = null)
    {
        var query = _context.Rooms.Include(r => r.RoomType).AsQueryable();
        if (!string.IsNullOrEmpty(id)) query = query.Where(r => r.Id == id);
        if (!string.IsNullOrEmpty(roomTypeId)) query = query.Where(r => r.RoomType_id == roomTypeId);
        if (!string.IsNullOrEmpty(label)) query = query.Where(r => r.Label.ToLower().Contains(label.ToLower()));
        if (status.HasValue) query = query.Where(r => r.Status == status.Value);
        if (!string.IsNullOrEmpty(roomTicketId))
            query = query.Where(r => r.RoomTickets.Any(rt => rt.Id == roomTicketId));
        if (!string.IsNullOrEmpty(serviceTicketId))
            query = query.Where(r => r.ServiceTickets.Any(st => st.Id == serviceTicketId));
        return await query.ToListAsync();
    }

    public async Task<Room?> Create(Room entity)
    {
        _context.Rooms.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Room?> Update(Room entity)
    {
        var room = await _context.Rooms.FindAsync(entity.Id);
        if (room == null) return null;

        room.Label = entity.Label;
        room.Status = entity.Status;
        room.RoomType_id = entity.RoomType_id;
        await _context.SaveChangesAsync();
        return room;
    }

    public async Task<Room?> Delete(string id)
    {
        var room = await _context.Rooms.FindAsync(id);
        if (room == null) return null;

        _context.Rooms.Remove(room);
        await _context.SaveChangesAsync();
        return room;
    }
}