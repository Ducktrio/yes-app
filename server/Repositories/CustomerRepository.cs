using System;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Yes.Data;
using Yes.Models;

namespace Yes.Repositories;

public interface ICustomerRepository
{
    Task<bool> Exists(string id);
    Task<List<Customer>> Get(string? id = null, string? roomTicketId = null, string? serviceTicketId = null);
    Task<Customer?> Create(Customer entity);
    Task<Customer?> Update(Customer entity);
    Task<Customer?> Delete(string id);
}

public class CustomerRepository(ApplicationDbContext context) : ICustomerRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<bool> Exists(string id)
    {
        return await _context.Customers.AnyAsync(c => c.Id == id);
    }

    public async Task<List<Customer>> Get(string? id = null, string? roomTicketId = null, string? serviceTicketId = null)
    {
        var query = _context.Customers.AsQueryable();
        if (!string.IsNullOrEmpty(id)) query = query.Where(c => c.Id == id);
        if (!string.IsNullOrEmpty(roomTicketId)) query = query.Where(c => c.RoomTickets.Any(rt => rt.Id == roomTicketId));
        if (!string.IsNullOrEmpty(serviceTicketId)) query = query.Where(c => c.ServiceTickets.Any(st => st.Id == serviceTicketId));
        return await query.ToListAsync();
    }

    public async Task<Customer?> Create(Customer entity)
    {
        _context.Customers.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Customer?> Update(Customer entity)
    {
        var customer = await _context.Customers.FindAsync(entity.Id);
        if (customer == null) return null;

        customer.Courtesy_title = entity.Courtesy_title;
        customer.Full_name = entity.Full_name;
        customer.Age = entity.Age;
        customer.Phone_number = entity.Phone_number;
        customer.Contact_info = entity.Contact_info;
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Customer?> Delete(string id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null) return null;

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();
        return customer;
    }
}
