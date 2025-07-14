using System.Text.Json.Serialization;

namespace Yes.Contracts;

public record class RoomTypeContract
{
    [JsonPropertyName("id")]
    public string Id { get; init; } = null!;
    [JsonPropertyName("name")]
    public string Name { get; init; } = null!;
    [JsonPropertyName("description")]
    public string? Description { get; init; } = null;
    [JsonPropertyName("price")]
    public int Price { get; init; } = 0;
}

public record class CreateRoomTypeContract
{
    [JsonPropertyName("name")]
    public string Name { get; init; } = null!;
    [JsonPropertyName("description")]
    public string? Description { get; init; } = null;
    [JsonPropertyName("price")]
    public int Price { get; init; } = 0;
}

public record class UpdateRoomTypeContract
{
    [JsonPropertyName("name")]
    public string? Name { get; init; } = null;
    [JsonPropertyName("description")]
    public string? Description { get; init; } = null;
    [JsonPropertyName("price")]
    public int? Price { get; init; } = 0;
}