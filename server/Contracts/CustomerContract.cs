using System.Text.Json.Serialization;

namespace Yes.Contracts;

public record class CustomerContract
{
    [JsonPropertyName("id")]
    public string Id { get; init; } = null!;
    [JsonPropertyName("courtesy_title")]
    public string Courtesy_title { get; init; } = null!;
    [JsonPropertyName("full_name")]
    public string Full_name { get; init; } = null!;
    [JsonPropertyName("age")]
    public int Age { get; init; } = 0;
    [JsonPropertyName("phone_number")]
    public string? Phone_number { get; init; } = null;
    [JsonPropertyName("contact_info")]
    public string? Contact_info { get; init; } = null;
}

public record class CreateCustomerContract
{
    [JsonPropertyName("courtesy_title")]
    public string Courtesy_title { get; init; } = null!;
    [JsonPropertyName("full_name")]
    public string Full_name { get; init; } = null!;
    [JsonPropertyName("age")]
    public int Age { get; init; } = 0;
    [JsonPropertyName("phone_number")]
    public string? Phone_number { get; init; } = null;
    [JsonPropertyName("contact_info")]
    public string? Contact_info { get; init; } = null;
}

public record class UpdateCustomerContract
{
    [JsonPropertyName("courtesy_title")]
    public string? Courtesy_title { get; init; } = null;
    [JsonPropertyName("full_name")]
    public string? Full_name { get; init; } = null;
    [JsonPropertyName("age")]
    public int? Age { get; init; } = 0;
    [JsonPropertyName("phone_number")]
    public string? Phone_number { get; init; } = null;
    [JsonPropertyName("contact_info")]
    public string? Contact_info { get; init; } = null;
}
