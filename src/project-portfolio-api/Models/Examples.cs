using System.Text.Json.Serialization;

namespace project_project_portfolio_api.Models;

public class Examples{
    [JsonPropertyName("examples")]
    public List<Example>? ExampleList {get;set;}
}