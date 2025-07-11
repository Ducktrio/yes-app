using System;
using Microsoft.EntityFrameworkCore;
using Yes.Data;
using Yes.Models;

namespace Yes.Repositories;

public interface IRoleRepository
{
    Task<bool> Exists(string id);
    Task<List<Role>> Get(string? id = null);
    Task<Role?> Create(Role entity);
    Task<Role?> Update(Role entity);
    Task<Role?> Delete(string id);
}

public class RoleRepository(ApplicationDbContext context) : IRoleRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<bool> Exists(string id)
    {
        return await _context.Roles.AnyAsync(r => r.Id == id);
    }

    public async Task<List<Role>> Get(string? id = null)
    {
        var query = _context.Roles.AsQueryable();
        if (!string.IsNullOrEmpty(id)) query = query.Where(r => r.Id == id);
        return await query.ToListAsync();
    }

    public async Task<Role?> Create(Role entity)
    {
        _context.Roles.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Role?> Update(Role entity)
    {
        var role = await _context.Roles.FindAsync(entity.Id);
        if (role == null) return null;

        role.Title = entity.Title;
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Role?> Delete(string id)
    {
        var role = await _context.Roles.FindAsync(id);
        if (role == null) return null;

        _context.Roles.Remove(role);
        await _context.SaveChangesAsync();
        return role;
    }
}
