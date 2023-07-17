using Newtonsoft.Json;

namespace SimonVossSearch.Domain;

public class Medium : IEntity
{
    public Guid Id { get; set; }
    [JsonProperty("groupId")]
    public Guid ParentId { get; set; }
    public string Type { get; set; }
    public string Owner { get; set; }
    public string Description { get; set; }
    public string SerialNumber { get; set; }
}