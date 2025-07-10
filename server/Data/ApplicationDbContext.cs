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

        // Seeder
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = "R_001", Title = "Manager" },
            new Role { Id = "R_002", Title = "Receptionist" },
            new Role { Id = "R_003", Title = "Staff" }
        );
        modelBuilder.Entity<User>().HasData(
            new User { Id = "U_001", Role_id = "R_001", Username = "Manager", Password = BCrypt.Net.BCrypt.HashPassword("manager123"), Description = "Hotel Manager" },
            new User { Id = "U_002", Role_id = "R_002", Username = "Receptionist", Password = BCrypt.Net.BCrypt.HashPassword("receptionist123"), Description = "Front Desk Receptionist" },
            new User { Id = "U_003", Role_id = "R_003", Username = "Staff", Password = BCrypt.Net.BCrypt.HashPassword("staff123"), Description = "Hotel Staff" }
        );
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
        // User
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

        // Role
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