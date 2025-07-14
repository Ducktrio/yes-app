using System;
using Microsoft.EntityFrameworkCore;
using Yes.Data;
using Yes.Models;

namespace Yes.Repositories;

public interface IRoomTypeRepository
{
    Task<bool> Exists(string id);
    Task<List<RoomType>> Get(string? id = null, string? roomId = null);
    Task<RoomType?> Create(RoomType entity);
    Task<RoomType?> Update(RoomType entity);
    Task<RoomType?> Delete(string id);
}

public class RoomTypeRepository(ApplicationDbContext context) : IRoomTypeRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<bool> Exists(string id)
    {
        return await _context.RoomTypes.AnyAsync(rt => rt.Id == id);
    }

    public async Task<List<RoomType>> Get(string? id = null, string? roomId = null)
    {
        var query = _context.RoomTypes.AsQueryable();
        if (!string.IsNullOrEmpty(id)) query = query.Where(rt => rt.Id == id);
        if (!string.IsNullOrEmpty(roomId)) query = query.Where(rt => rt.Rooms.Any(r => r.Id == roomId));
        return await query.ToListAsync();
    }

    public async Task<RoomType?> Create(RoomType entity)
    {
        _context.RoomTypes.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<RoomType?> Update(RoomType entity)
    {
        var roomType = await _context.RoomTypes.FindAsync(entity.Id);
        if (roomType == null) return null;

        roomType.Name = entity.Name;
        roomType.Description = entity.Description;
        roomType.Price = entity.Price;
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<RoomType?> Delete(string id)
    {
        var roomType = await _context.RoomTypes.FindAsync(id);
        if (roomType == null) return null;

        _context.RoomTypes.Remove(roomType);
        await _context.SaveChangesAsync();
        return roomType;
    }
}
