using System.Text.Json.Serialization;

namespace Yes.Contracts;

public record class ServiceContract
{
    [JsonPropertyName("id")]
    public string Id { get; init; } = null!;
    [JsonPropertyName("name")]
    public string Name { get; init; } = null!;
    [JsonPropertyName("price")]
    public int Price { get; init; } = 0;
}

public record class CreateServiceContract
{
    [JsonPropertyName("name")]
    public string Name { get; init; } = null!;
    [JsonPropertyName("price")]
    public int Price { get; init; } = 0;
}

public record class UpdateServiceContract
{
    [JsonPropertyName("name")]
    public string? Name { get; init; } = null;
    [JsonPropertyName("price")]
    public int? Price { get; init; } = 0;
}