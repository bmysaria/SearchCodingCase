using System.Reflection;
using Newtonsoft.Json;
using Persistence.Model;
using SimonVossSearch.Domain;

namespace SimonVossSearch.Core.Parser;

public class DataFileParser
{
    private readonly DataFile _data;
    public DataFileParser()
    {
        string path = File.ReadAllText("./data.json");
        _data = JsonConvert.DeserializeObject<DataFile>(path);
    }
    public List<Field> GetFields()
    { List<Field> fields = new List<Field>(); 
        List<IEntity> entities = new List<IEntity>();
        entities = entities.Concat(_data.Buildings).Concat(_data.Locks).Concat(_data.Groups).Concat(_data.Media)
            .ToList();
        foreach (var entity in entities)
        {
            var type = entity.GetType();
            IList<PropertyInfo> props = new List<PropertyInfo>(type.GetProperties());
            foreach (var prop in props)
            {
                if (prop.Name == "Id" || prop.Name == "BuildingId" || prop.Name == "GroupId" || prop.Name == "Mediums" || prop.Name == "Locks" || prop.Name == "ParentId")
                    continue;
                var value = prop.GetValue(entity, null) == null ? "" : prop.GetValue(entity, null).ToString().ToLower();
                if (string.IsNullOrEmpty(value))
                    continue;
                var field = new Field(entity.Id, type.Name, prop.Name, value, entity.ParentId);
                fields.Add(field);
            }
        }

        return fields;

    }
}