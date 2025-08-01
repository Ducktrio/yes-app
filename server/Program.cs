using System.Security.Claims;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Yes.Configurations;
using Yes.Contracts;
using Yes.Data;
using Yes.Mappers;
using Yes.Repositories;
using Yes.Services;
using Yes.Validators;

var builder = WebApplication.CreateBuilder(args);

//Configurations
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(nameof(JwtSettings)));
builder.Services.AddSingleton(serviceProvider => serviceProvider.GetRequiredService<IOptions<JwtSettings>>().Value);

//Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

//Authentication & Authorization
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var service = context.HttpContext.RequestServices.GetRequiredService<IAuthenticationService>();
            service.ConfigureJwtOptions(options);
            return Task.CompletedTask;
        }
    };
});
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("Manager", policy => policy.RequireClaim(ClaimTypes.Role, "R_001"))
    .AddPolicy("Receptionist", policy => policy.RequireClaim(ClaimTypes.Role, "R_002"))
    .AddPolicy("Staff", policy => policy.RequireClaim(ClaimTypes.Role, "R_003"))
    .AddPolicy("ManagerOrReceptionist", policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim(c => c.Type == ClaimTypes.Role && (c.Value == "R_001" || c.Value == "R_002"))));

//Repositories
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IRoomTicketRepository, RoomTicketRepository>();
builder.Services.AddScoped<IRoomTypeRepository, RoomTypeRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IServiceTicketRepository, ServiceTicketRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

//AutoMappers
builder.Services.AddAutoMapper(typeof(CustomerMapper), typeof(RoleMapper), typeof(RoomMapper), typeof(RoomTicketMapper), typeof(RoomTypeMapper), typeof(ServiceMapper), typeof(ServiceTicketMapper), typeof(UserMapper));

//Validators
builder.Services.AddScoped<IValidator<CreateCustomerContract>, CreateCustomerValidator>();
builder.Services.AddScoped<IValidator<UpdateCustomerContract>, UpdateCustomerValidator>();
builder.Services.AddScoped<IValidator<CreateRoomContract>, CreateRoomValidator>();
builder.Services.AddScoped<IValidator<UpdateRoomContract>, UpdateRoomValidator>();
builder.Services.AddScoped<IValidator<CreateRoomTicketContract>, CreateRoomTicketValidator>();
builder.Services.AddScoped<IValidator<UpdateRoomTicketContract>, UpdateRoomTicketValidator>();
builder.Services.AddScoped<IValidator<CreateRoomTypeContract>, CreateRoomTypeValidator>();
builder.Services.AddScoped<IValidator<UpdateRoomTypeContract>, UpdateRoomTypeValidator>();
builder.Services.AddScoped<IValidator<CreateServiceContract>, CreateServiceValidator>();
builder.Services.AddScoped<IValidator<UpdateServiceContract>, UpdateServiceValidator>();
builder.Services.AddScoped<IValidator<CreateServiceTicketContract>, CreateServiceTicketValidator>();
builder.Services.AddScoped<IValidator<UpdateServiceTicketContract>, UpdateServiceTicketValidator>();
builder.Services.AddScoped<IValidator<CreateUserContract>, CreateUserValidator>();
builder.Services.AddScoped<IValidator<UpdateUserContract>, UpdateUserValidator>();

//Services
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IRoomTicketService, RoomTicketService>();
builder.Services.AddScoped<IRoomTypeService, RoomTypeService>();
builder.Services.AddScoped<IServiceService, ServiceService>();
builder.Services.AddScoped<IServiceTicketService, ServiceTicketService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddControllers();

//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Indiego API", Version = "v1" });
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "bearer",
            BearerFormat = "JWT"
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    }
);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

builder.WebHost.ConfigureKestrel(options =>
{
    var port = Environment.GetEnvironmentVariable("PORT");
    if (!string.IsNullOrEmpty(port))
    {
        options.ListenAnyIP(int.Parse(port));
    }
    else
    {
        options.ListenAnyIP(7231, listenOptions =>
        {
            listenOptions.UseHttps("https/aspnetcore-dev-cert.pfx", "password");
        });
        options.ListenAnyIP(5257);
    }
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();