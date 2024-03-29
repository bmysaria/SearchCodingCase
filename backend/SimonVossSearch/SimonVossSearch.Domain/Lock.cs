namespace SimonVossSearch.Domain;

public class Lock : IEntity
{
    public Guid Id { get; set; }
    [Newtonsoft.Json.JsonProperty("buildingId")]
    public Guid ParentId { get; set; }
    public string Type { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string SerialNumber { get; set; }
    public string Floor { get; set; }
    public string RoomNumber { get; set; }
}