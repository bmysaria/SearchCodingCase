using Newtonsoft.Json;

namespace SimonVossSearch.Domain;

public class Group : IEntity
{
    public Guid Id { get; set; }
    public Guid ParentId { get; set; }
    public string Name { get; set; }
    [JsonConverter(typeof(JsonNullToEmptyStringConverter))]
    public string Description { get; set; }
}