using System.Text.Json.Serialization;

namespace Yes.Contracts;

public record class UserContract
{
    [JsonPropertyName("id")]
    public string Id { get; init; } = null!;
    [JsonPropertyName("role_id")]
    public string Role_id { get; init; } = null!;
    [JsonPropertyName("username")]
    public string Username { get; init; } = null!;
    [JsonPropertyName("description")]
    public string Description { get; init; } = null!;
}

public record class CreateUserContract
{
    [JsonPropertyName("role_id")]
    public string Role_id { get; init; } = null!;
    [JsonPropertyName("username")]
    public string Username { get; init; } = null!;
    [JsonPropertyName("password")]
    public string Password { get; init; } = null!;
    [JsonPropertyName("description")]
    public string Description { get; init; } = null!;
}

public record class UpdateUserContract
{
    [JsonPropertyName("role_id")]
    public string? Role_id { get; init; } = null;
    [JsonPropertyName("username")]
    public string? Username { get; init; } = null;
    [JsonPropertyName("password")]
    public string? Password { get; init; } = null;
    [JsonPropertyName("description")]
    public string? Description { get; init; } = null;
}

public record class LoginContract
{
    [JsonPropertyName("username")]
    public string Username { get; init; } = null!;
    [JsonPropertyName("password")]
    public string Password { get; init; } = null!;
}

public record class LoginResponseContract
{
    [JsonPropertyName("token")]
    public string Token { get; init; } = null!;
    [JsonPropertyName("user")]
    public UserContract User { get; init; } = null!;
}