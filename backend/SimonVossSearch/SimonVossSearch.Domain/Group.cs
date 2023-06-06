namespace SimonVossSearch.Core.Model.Entities;

public class Group : IEntity
{
    public Guid Id { get; set; }
    public Guid ParentId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}