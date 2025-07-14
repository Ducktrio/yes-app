using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;

namespace Yes.Contracts;

public record class ServiceTicketContract
{
    [JsonPropertyName("id")]
    public string Id { get; init; } = null!;
    [JsonPropertyName("customer_id")]
    public string Customer_id { get; init; } = null!;
    [JsonPropertyName("room_id")]
    public string Room_id { get; init; } = null!;
    [JsonPropertyName("service_id")]
    public string Service_id { get; init; } = null!;
    [JsonPropertyName("details")]
    public string Details { get; init; } = null!;
    [JsonPropertyName("status")]
    public int Status { get; init; } = 0;
}

public record class CreateServiceTicketContract
{
    [JsonPropertyName("customer_id")]
    public string Customer_id { get; init; } = null!;
    [JsonPropertyName("room_id")]
    public string Room_id { get; init; } = null!;
    [JsonPropertyName("service_id")]
    public string Service_id { get; init; } = null!;
    [JsonPropertyName("details")]
    public string Details { get; init; } = null!;
}

public record class UpdateServiceTicketContract
{
    [JsonPropertyName("customer_id")]
    public string? Customer_id { get; init; } = null;
    [JsonPropertyName("room_id")]
    public string? Room_id { get; init; } = null;
    [JsonPropertyName("service_id")]
    public string? Service_id { get; init; } = null;
    [JsonPropertyName("details")]
    public string? Details { get; init; } = null;
    [JsonPropertyName("status")]
    public int? Status { get; init; } = null;
}