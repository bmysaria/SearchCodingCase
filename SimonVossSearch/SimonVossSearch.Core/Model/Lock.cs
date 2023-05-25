namespace SimonVossSearch.Core.Model;

public class Lock : IEntity
{
    public Guid Id { get; set; }
    public Guid BuildingId { get; set; }
    public string Type { get; set; }
    public string Name { get; set; }
    public object Description { get; set; }
    public string SerialNumber { get; set; }
    public string Floor { get; set; }
    public string RoomNumber { get; set; }
}