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
    .AddPolicy("Auth", policy => policy.RequireAuthenticatedUser())
    .AddPolicy("Manager", policy => policy.RequireClaim(ClaimTypes.Role, "R_001"))
    .AddPolicy("Receptionist", policy => policy.RequireClaim(ClaimTypes.Role, "R_002"))
    .AddPolicy("Staff", policy => policy.RequireClaim(ClaimTypes.Role, "R_003"));

//Repositories
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

//AutoMappers
builder.Services.AddAutoMapper(typeof(RoleMapper), typeof(UserMapper));

//Validators
builder.Services.AddScoped<IValidator<CreateUserContract>, CreateUserValidator>();
builder.Services.AddScoped<IValidator<UpdateUserContract>, UpdateUserValidator>();
builder.Services.AddScoped<IValidator<CreateRoleContract>, CreateRoleValidator>();
builder.Services.AddScoped<IValidator<UpdateRoleContract>, UpdateRoleValidator>();

//Services
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();