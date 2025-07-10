using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using Yes.Configurations;
using Yes.Models;

namespace Yes.Services;

public interface IAuthenticationService
{
    string GenerateToken(User user);
    Task HandleTokenValidation(TokenValidatedContext context);
    Task HandleTokenReceived(MessageReceivedContext context);
    void ConfigureJwtOptions(JwtBearerOptions options);
    string? GetId(string token);
    string? GetRole(string token);
}

public class AuthenticationService(IDistributedCache cache, JwtSettings settings) : IAuthenticationService
{
    private readonly IDistributedCache _cache = cache;
    private readonly JwtSettings _settings = settings;

    public string GenerateToken(User user)
    {
        List<Claim> claims = [
            new Claim(ClaimTypes.NameIdentifier, user.Id)
        ];
        if (user is { Role_id: not null })
        {
            claims.Add(new Claim(ClaimTypes.Role, user.Role_id));
        }
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(1),
            Issuer = _settings.Issuer,
            Audience = _settings.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key)), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task HandleTokenValidation(TokenValidatedContext context)
    {
        var userId = context.Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!string.IsNullOrEmpty(userId))
        {
            var lastActiveKey = $"LastActive_{userId}";
            await _cache.SetStringAsync(lastActiveKey, DateTime.UtcNow.ToString(), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
            });
        }
    }

    public async Task HandleTokenReceived(MessageReceivedContext context)
    {
        var userId = context.Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!string.IsNullOrEmpty(userId))
        {
            var lastActiveKey = $"LastActive_{userId}";
            var lastActive = await _cache.GetStringAsync(lastActiveKey);
            if (!string.IsNullOrEmpty(lastActive) && DateTime.UtcNow - DateTime.Parse(lastActive) > TimeSpan.FromDays(1))
            {
                context.Fail("Token is expired");
            }
        }
    }

    public void ConfigureJwtOptions(JwtBearerOptions options)
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = _settings.Issuer,
            ValidAudience = _settings.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = HandleTokenValidation,
            OnMessageReceived = HandleTokenReceived
        };
    }
    
    public string? GetId(string token)
    {
        if (string.IsNullOrEmpty(token))
            return null;
        
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var jwtToken = tokenHandler.ReadJwtToken(token);
            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid");
            return userIdClaim?.Value;
        }
        catch
        {
            return null;
        }
    }

    public string? GetRole(string token)
    {
        if (string.IsNullOrEmpty(token))
            return null;
        
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var jwtToken = tokenHandler.ReadJwtToken(token);
            var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "role");
            return roleClaim?.Value;
        }
        catch
        {
            return null;
        }
    }
}
