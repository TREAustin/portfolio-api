using System.Text.Json.Serialization;

namespace project_portfolio_api.Models;

public class Image
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    [JsonPropertyName("exampleId")]
    public string? ExampleId { get; set; }
    [JsonPropertyName("url")]
    public string? Url { get; set; }
}