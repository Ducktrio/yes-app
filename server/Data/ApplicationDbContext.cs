using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Yes.Models;

namespace Yes.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.Role_id);
    }

    public override int SaveChanges()
    {
        AssignCustomIds();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        AssignCustomIds();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void AssignCustomIds()
    {
        // Assign User IDs
        var newUsers = ChangeTracker.Entries<User>()
            .Where(e => e.State == EntityState.Added && string.IsNullOrEmpty(e.Entity.Id))
            .Select(e => e.Entity);

        if (newUsers.Any())
        {
            int max = Users
                .Where(u => u.Id.StartsWith("U_"))
                .AsEnumerable()
                .Select(u => int.TryParse(u.Id.AsSpan(2), out var n) ? n : 0)
                .DefaultIfEmpty(0)
                .Max();

            foreach (var user in newUsers)
            {
                max++;
                user.Id = $"U_{max:D3}";
            }
        }

        // Assign Role IDs
        var newRoles = ChangeTracker.Entries<Role>()
            .Where(e => e.State == EntityState.Added && string.IsNullOrEmpty(e.Entity.Id))
            .Select(e => e.Entity);

        if (newRoles.Any())
        {
            int max = Roles
                .Where(r => r.Id.StartsWith("R_"))
                .AsEnumerable()
                .Select(r => int.TryParse(r.Id.AsSpan(2), out var n) ? n : 0)
                .DefaultIfEmpty(0)
                .Max();

            foreach (var role in newRoles)
            {
                max++;
                role.Id = $"R_{max:D3}";
            }
        }
    }
}