using System.Text.Json.Serialization;
using project_portfolio_api.Models;
using OS = project_portfolio_api.Models.OperatingSystem;

namespace project_project_portfolio_api.Models;

public class Example
{
    [JsonPropertyName("id")]
    public int? Id { get; set; }
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    [JsonPropertyName("subDescription")]
    public string? SubDescription { get; set; }
    [JsonPropertyName("frameworks")]
    public List<Framework>? Frameworks { get; set; }
    [JsonPropertyName("languages")]
    public List<Langauge>? Langauges { get; set; }
    [JsonPropertyName("operatingSystems")]
    public List<OS>? OperatingSystems { get; set; }
    [JsonPropertyName("images")]
    public List<Image>? Images { get; set; }

}