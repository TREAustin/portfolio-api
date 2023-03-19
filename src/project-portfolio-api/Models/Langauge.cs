using System.Text.Json.Serialization;

namespace project_portfolio_api.Models;

public class Langauge
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    [JsonPropertyName("exampleId")]
    public string? ExampleId { get; set; }
    [JsonPropertyName("type")]
    public string? Type { get; set; }
}