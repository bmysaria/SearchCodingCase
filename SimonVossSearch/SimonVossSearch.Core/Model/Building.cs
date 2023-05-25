namespace SimonVossSearch.Core.Model;

public class Building : IEntity
{
    public Guid Id { get; set; }
    public string ShortCut { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}