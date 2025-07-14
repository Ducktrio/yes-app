using System.Text.Json.Serialization;

namespace Yes.Contracts;

public record class RoomContract
{
    [JsonPropertyName("id")]
    public string Id { get; init; } = null!;
    [JsonPropertyName("roomType_id")]
    public string RoomType_id { get; init; } = null!;
    [JsonPropertyName("label")]
    public string Label { get; init; } = null!;
    [JsonPropertyName("Status")]
    public int Status { get; init; } = 0;
}

public record class CreateRoomContract
{
    [JsonPropertyName("roomType_id")]
    public string RoomType_id { get; init; } = null!;
    [JsonPropertyName("label")]
    public string Label { get; init; } = null!;
    [JsonPropertyName("Status")]
    public int Status { get; init; } = 0;
}

public record class UpdateRoomContract
{
    [JsonPropertyName("roomType_id")]
    public string? RoomType_id { get; init; } = null;
    [JsonPropertyName("label")]
    public string? Label { get; init; } = null;
    [JsonPropertyName("Status")]
    public int? Status { get; init; } = 0;
}