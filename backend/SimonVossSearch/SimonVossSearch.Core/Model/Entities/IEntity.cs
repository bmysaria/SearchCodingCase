namespace SimonVossSearch.Core.Model.Entities;

public interface IEntity
{
    public Guid Id { get; set; }
    public Guid ParentId { get; set; }
}