using System.Text.Json.Serialization;

namespace Yes.Contracts;

public record class RoleContract
{
    [JsonPropertyName("id")]
    public string Id { get; init; } = null!;
    [JsonPropertyName("title")]
    public string Title { get; init; } = null!;
}

public record class CreateRoleContract
{
    [JsonPropertyName("title")]
    public string Title { get; init; } = null!;
}

public record class UpdateRoleContract
{
    [JsonPropertyName("title")]
    public string? Title { get; init; } = null;
}