using SimonVossSearch.Core.Model.Entities;

namespace SimonVossSearch.Core.Model;

public class DataFile
{
    public List<Building> Buildings { get; set; }
    public List<Lock> Locks { get; set; }
    public List<Group> Groups { get; set; }
    public List<Medium> Media { get; set; }
}
