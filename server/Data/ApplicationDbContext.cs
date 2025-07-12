using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Yes.Models;

namespace Yes.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<Room> Rooms { get; set; } = null!;
    public DbSet<RoomTicket> RoomTickets { get; set; } = null!;
    public DbSet<RoomType> RoomTypes { get; set; } = null!;
    public DbSet<Service> Services { get; set; } = null!;
    public DbSet<ServiceTicket> ServiceTickets { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Room>()
            .HasOne(r => r.RoomType)
            .WithMany(rt => rt.Rooms)
            .HasForeignKey(r => r.RoomType_id);
        modelBuilder.Entity<RoomTicket>()
            .HasOne(rt => rt.Customer)
            .WithMany(c => c.RoomTickets)
            .HasForeignKey(rt => rt.Customer_id);
        modelBuilder.Entity<RoomTicket>()
            .HasOne(rt => rt.Room)
            .WithMany(r => r.RoomTickets)
            .HasForeignKey(rt => rt.Room_id);
        modelBuilder.Entity<ServiceTicket>()
            .HasOne(st => st.Customer)
            .WithMany(c => c.ServiceTickets)
            .HasForeignKey(st => st.Customer_id);
        modelBuilder.Entity<ServiceTicket>()
            .HasOne(st => st.Room)
            .WithMany(r => r.ServiceTickets)
            .HasForeignKey(st => st.Room_id);
        modelBuilder.Entity<ServiceTicket>()
            .HasOne(st => st.Service)
            .WithMany(s => s.ServiceTickets)
            .HasForeignKey(st => st.Service_id);
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
        modelBuilder.Entity<Service>().HasData(
            new Service { Id = "SVC_001", Name = "Cleaning", Price = 0 },
            new Service { Id = "SVC_002", Name = "Laundry", Price = 0 },
            new Service { Id = "SVC_003", Name = "Spa", Price = 75000 },
            new Service { Id = "SVC_004", Name = "Restaurant", Price = 0 }
        );
        modelBuilder.Entity<RoomType>().HasData(
            new RoomType { Id = "RT_001", Name = "Single", Description = "A room for one person, equipped with a single bed.", Price = 500000 },
            new RoomType { Id = "RT_002", Name = "Double", Description = "A room for two people, equipped with a double bed.", Price = 750000 },
            new RoomType { Id = "RT_003", Name = "Deluxe", Description = "A more spacious room with additional amenities, suitable for couples or small families.", Price = 900000 },
            new RoomType { Id = "RT_004", Name = "Suite", Description = "A luxurious room with separate living and sleeping areas, ideal for longer stays or special occasions.", Price = 1000000 }
        );
        modelBuilder.Entity<Room>().HasData(
            new Room { Id = "RM_001", Label = "A1", RoomType_id = "RT_001", Status = 0 },
            new Room { Id = "RM_002", Label = "A2", RoomType_id = "RT_001", Status = 0 },
            new Room { Id = "RM_003", Label = "A3", RoomType_id = "RT_001", Status = 0 }
        );
        modelBuilder.Entity<Customer>().HasData(
            new Customer { Id = "C_0001", Courtesy_title = "Mr.", Full_name = "John Doe", Age = 30, Phone_number = "1234567890" },
            new Customer { Id = "C_0002", Courtesy_title = "Ms.", Full_name = "Jane Smith", Age = 28, Phone_number = "0987654321" },
            new Customer { Id = "C_0003", Courtesy_title = "Mrs.", Full_name = "Emily Johnson", Age = 35, Phone_number = "1112223333" },
            new Customer { Id = "C_0004", Courtesy_title = "Mr.", Full_name = "Michael Brown", Age = 40, Phone_number = "2223334444" },
            new Customer { Id = "C_0005", Courtesy_title = "Ms.", Full_name = "Linda Davis", Age = 27, Phone_number = "3334445555" },
            new Customer { Id = "C_0006", Courtesy_title = "Dr.", Full_name = "Robert Wilson", Age = 50, Phone_number = "4445556666" },
            new Customer { Id = "C_0007", Courtesy_title = "Miss", Full_name = "Sophia Martinez", Age = 22, Phone_number = "5556667777" }
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
        // Customer
        var newCustomers = ChangeTracker.Entries<Customer>()
            .Where(e => e.State == EntityState.Added && string.IsNullOrEmpty(e.Entity.Id))
            .Select(e => e.Entity);

        if (newCustomers.Any())
        {
            int max = Customers
                .Where(c => c.Id.StartsWith("C_"))
                .AsEnumerable()
                .Select(c => int.TryParse(c.Id.AsSpan(2), out var n) ? n : 0)
                .DefaultIfEmpty(0)
                .Max();

            foreach (var customer in newCustomers)
            {
                max++;
                customer.Id = $"C_{max:D4}";
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

        // Room
        var newRooms = ChangeTracker.Entries<Room>()
            .Where(e => e.State == EntityState.Added && string.IsNullOrEmpty(e.Entity.Id))
            .Select(e => e.Entity);

        if (newRooms.Any())
        {
            int max = Rooms
                .Where(r => r.Id.StartsWith("RM_"))
                .AsEnumerable()
                .Select(r => int.TryParse(r.Id.AsSpan(3), out var n) ? n : 0)
                .DefaultIfEmpty(0)
                .Max();

            foreach (var room in newRooms)
            {
                max++;
                room.Id = $"RM_{max:D3}";
            }
        }

        // RoomTicket
        var newRoomTickets = ChangeTracker.Entries<RoomTicket>()
            .Where(e => e.State == EntityState.Added && string.IsNullOrEmpty(e.Entity.Id))
            .Select(e => e.Entity);

        if (newRoomTickets.Any())
        {
            int max = RoomTickets
                .Where(rt => rt.Id.StartsWith("TKTRM_"))
                .AsEnumerable()
                .Select(rt => int.TryParse(rt.Id.AsSpan(6), out var n) ? n : 0)
                .DefaultIfEmpty(0)
                .Max();

            foreach (var roomTicket in newRoomTickets)
            {
                max++;
                roomTicket.Id = $"TKTRM_{max:D4}";
            }
        }

        // RoomType
        var newRoomTypes = ChangeTracker.Entries<RoomType>()
            .Where(e => e.State == EntityState.Added && string.IsNullOrEmpty(e.Entity.Id))
            .Select(e => e.Entity);

        if (newRoomTypes.Any())
        {
            int max = RoomTypes
                .Where(rt => rt.Id.StartsWith("RT_"))
                .AsEnumerable()
                .Select(rt => int.TryParse(rt.Id.AsSpan(3), out var n) ? n : 0)
                .DefaultIfEmpty(0)
                .Max();

            foreach (var roomType in newRoomTypes)
            {
                max++;
                roomType.Id = $"RT_{max:D3}";
            }
        }

        // Service
        var newServices = ChangeTracker.Entries<Service>()
            .Where(e => e.State == EntityState.Added && string.IsNullOrEmpty(e.Entity.Id))
            .Select(e => e.Entity);

        if (newServices.Any())
        {
            int max = Services
                .Where(s => s.Id.StartsWith("SVC_"))
                .AsEnumerable()
                .Select(s => int.TryParse(s.Id.AsSpan(4), out var n) ? n : 0)
                .DefaultIfEmpty(0)
                .Max();

            foreach (var service in newServices)
            {
                max++;
                service.Id = $"SVC_{max:D3}";
            }
        }

        // ServiceTicket
        var newServiceTickets = ChangeTracker.Entries<ServiceTicket>()
            .Where(e => e.State == EntityState.Added && string.IsNullOrEmpty(e.Entity.Id))
            .Select(e => e.Entity);

        if (newServiceTickets.Any())
        {
            int max = ServiceTickets
                .Where(st => st.Id.StartsWith("TKTSVC_"))
                .AsEnumerable()
                .Select(st => int.TryParse(st.Id.AsSpan(7), out var n) ? n : 0)
                .DefaultIfEmpty(0)
                .Max();

            foreach (var serviceTicket in newServiceTickets)
            {
                max++;
                serviceTicket.Id = $"TKTSVC_{max:D4}";
            }
        }

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
    }
}