namespace SimonVossSearch.Core.Model;

public interface IEntity
{
    public Guid Id { get; set; }
    public Guid ParentId { get; set; }
}