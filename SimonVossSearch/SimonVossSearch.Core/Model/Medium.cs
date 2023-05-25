namespace SimonVossSearch.Core.Model;

public class Medium : IEntity
{
    public Guid Id { get; set; }
    public Guid GroupId { get; set; }
    public string Type { get; set; }
    public string Owner { get; set; }
    public object Description { get; set; }
    public string SerialNumber { get; set; }
}