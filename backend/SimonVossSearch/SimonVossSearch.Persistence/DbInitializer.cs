using Newtonsoft.Json;
using Persistence.Model;

namespace Persistence;

public interface IDbInitializer
{
    public void Initialize();
}
public class DbInitializer : IDbInitializer
{
    private readonly SearchDbContext _context;

    public DbInitializer(SearchDbContext context)
    {
        _context = context;
    }
    
    public void Initialize()
    {
        if (!_context.Buildings.Any())
        {
            string path = File.ReadAllText("./data.json");
            var data = JsonConvert.DeserializeObject<DataFile>(path);
            
            data.Buildings.ForEach(x=>_context.Buildings.Add(x));
            data.Locks.ForEach(x => _context.Locks.Add(x));
            data.Groups.ForEach(x=>_context.Groups.Add(x));
            data.Media.ForEach(x=>_context.Media.Add(x));

            _context.SaveChanges();
        }
    }
}
