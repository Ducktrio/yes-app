using System;
using Microsoft.EntityFrameworkCore;
using Yes.Data;
using Yes.Models;

namespace Yes.Repositories;

public interface IUserRepository
{
    Task<bool> Exists(string id);
    Task<List<User>> Get(string? id = null, string? role_id = null, string? username = null);
    Task<User?> Create(User entity);
    Task<User?> Update(User entity);
    Task<User?> Delete(string id);
}

public class UserRepository(ApplicationDbContext context) : IUserRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<bool> Exists(string id)
    {
        return await _context.Users.AnyAsync(u => u.Id == id);
    }

    public async Task<List<User>> Get(string? id = null, string? role_id = null, string? username = null)
    {
        var query = _context.Users.Include(u => u.Role).AsQueryable();
        if (!string.IsNullOrEmpty(id)) query = query.Where(u => u.Id == id);
        if (!string.IsNullOrEmpty(role_id)) query = query.Where(u => u.Role_id == role_id);
        if (!string.IsNullOrEmpty(username)) query = query.Where(u => u.Username.ToLower().Contains(username.ToLower()));
        return await query.ToListAsync();
    }

    public async Task<User?> Create(User entity)
    {
        _context.Users.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<User?> Update(User entity)
    {
        var user = await _context.Users.FindAsync(entity.Id);
        if (user == null) return null;

        user.Username = entity.Username;
        user.Password = entity.Password;
        user.Description = entity.Description;
        user.Role_id = entity.Role_id;
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User?> Delete(string id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return null;
        
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return user;
    }
}