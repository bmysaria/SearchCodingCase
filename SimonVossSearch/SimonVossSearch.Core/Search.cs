using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using SimonVossSearch.Core.Model;

namespace SimonVossSearch.Core;

public class Search
{
    private DataFile _data;
    private List<Field> _fields = new List<Field>();
    private string _targetString;

    public Search(DataFile data, string targetString)
    {
        this._data = data;
        _targetString = targetString;
    }

    public List<Field> Execute()
    {
        List<IEntity> entities = new List<IEntity>();
        entities = entities.Concat(_data.Buildings).Concat(_data.Locks).Concat(_data.Groups).Concat(_data.Media)
            .ToList();

        foreach (var entity in entities)
        {
            var type = entity.GetType();
            IList<PropertyInfo> props = new List<PropertyInfo>(type.GetProperties());
            foreach (var prop in props)
            {
                if (prop.Name == "Id" || prop.Name == "BuildingId" || prop.Name == "GroupId")
                    continue;
                var value = prop.GetValue(entity, null) == null ? "" : prop.GetValue(entity, null).ToString();
                var field = new Field(entity.Id, type.Name, prop.Name, value, _targetString);
                _fields.Add(field);
            }
        }

        return _fields.OrderByDescending(x => x.Weight).ToList();
    }
}