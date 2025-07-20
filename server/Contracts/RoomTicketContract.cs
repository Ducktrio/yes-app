using System.Text.Json.Serialization;

namespace Yes.Contracts;

public record class RoomTicketContract
{
    [JsonPropertyName("id")]
    public string Id { get; init; } = null!;
    [JsonPropertyName("customer_id")]
    public string Customer_id { get; init; } = null!;
    [JsonPropertyName("room_id")]
    public string Room_id { get; init; } = null!;
    [JsonPropertyName("check_in_date")]
    public string CheckInDate { get; init; } = null!;
    [JsonPropertyName("check_out_date")]
    public string CheckOutDate { get; init; } = null!;
    [JsonPropertyName("number_of_occupants")]
    public int Number_of_occupants { get; init; } = 1;
    [JsonPropertyName("status")]
    public int Status { get; init; } = 0;
}

public record class CreateRoomTicketContract
{
    [JsonPropertyName("customer_id")]
    public string Customer_id { get; init; } = null!;
    [JsonPropertyName("room_id")]
    public string Room_id { get; init; } = null!;
    [JsonPropertyName("number_of_occupants")]
    public int Number_of_occupants { get; init; } = 1;
}

public record class UpdateRoomTicketContract
{
    [JsonPropertyName("customer_id")]
    public string? Customer_id { get; init; } = null;
    [JsonPropertyName("room_id")]
    public string? Room_id { get; init; } = null;
    [JsonPropertyName("number_of_occupants")]
    public int? Number_of_occupants { get; init; } = null;
}