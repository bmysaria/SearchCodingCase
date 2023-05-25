using System.Text.Json.Serialization;

namespace SimonVossSearch.Core.Model;

public class Medium : IEntity
{
    public Guid Id { get; set; }
    [JsonPropertyName("GroupId")]
    public Guid ParentId { get; set; }
    public string Type { get; set; }
    public string Owner { get; set; }
    public object Description { get; set; }
    public string SerialNumber { get; set; }
}