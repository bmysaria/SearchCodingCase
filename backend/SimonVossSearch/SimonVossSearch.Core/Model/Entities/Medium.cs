using Newtonsoft.Json;

namespace SimonVossSearch.Core.Model.Entities;

public class Medium : IEntity
{
    public Guid Id { get; set; }
    [JsonProperty("groupId")]
    public Guid ParentId { get; set; }
    public string Type { get; set; }
    public string Owner { get; set; }
    public object Description { get; set; }
    public string SerialNumber { get; set; }
}