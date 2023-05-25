using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using SimonVossSearch.Core.Model;
namespace SimonVossSearch.Core;
using System;
using System.Collections.Generic;
using System.Linq;

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

    private void AssignChildren()
    {
        foreach (var building in _data.Buildings)
        {
            building.Locks = _data.Locks.Where(x => x.BuildingId == building.Id).ToList() ;
        }

        foreach (var group_ in _data.Groups)
        {
            group_.Mediums = _data.Media.Where(x => x.GroupId == group_.Id).ToList() ;
        }
    }

    public /*List<string> */ void Execute()
    {
        var ngram = 3;
        var terms = new HashSet<string>();
        var docFreq = new List<Dictionary<string, int>>();

        var documents = GetDocuments();
        
        foreach (var document in documents)
        {
            Dictionary<string, int> docTerms = new Dictionary<string, int>();
            for (int i = 0; i < document.Length - (ngram - 1); i++)
            {
                var term = document.Substring(i, ngram);
                terms.Add(term);
                if (docTerms.TryGetValue(term, out var val))
                {
                    docTerms[term] += val + 1;
                }
                else
                {
                    docTerms[term] = 1;
                }
            }
            docFreq.Add(docTerms);
        }
        
    }

    public List<string> GetDocuments()
    {
        List<string> documents = new List<string>();
        
        List<IEntity> entities = new List<IEntity>();
        entities = entities.Concat(_data.Buildings).Concat(_data.Locks).Concat(_data.Groups).Concat(_data.Media)
            .ToList();
        
        foreach (var entity in entities)
        {
            var type = entity.GetType();
            IList<PropertyInfo> props = new List<PropertyInfo>(type.GetProperties());
            foreach (var prop in props)
            {
                if (prop.Name == "Id" || prop.Name == "BuildingId" || prop.Name == "GroupId" || prop.Name == "Mediums" || prop.Name == "Locks")
                    continue;
                var value = prop.GetValue(entity, null) == null ? "" : prop.GetValue(entity, null).ToString();
                if (string.IsNullOrEmpty(value))
                    continue;
                documents.Add(prop.GetValue(entity, null).ToString());
            }
        }

        return documents;

    }
    /*public List<Field> Execute()
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
                if (prop.Name == "Id" || prop.Name == "BuildingId" || prop.Name == "GroupId" || prop.Name == "Mediums" || prop.Name == "Locks")
                    continue;
                var value = prop.GetValue(entity, null) == null ? "" : prop.GetValue(entity, null).ToString();
                if (string.IsNullOrEmpty(value))
                    continue;
                var field = new Field(entity.Id, type.Name, prop.Name, value, _targetString);
                _fields.Add(field);
            }
        }
        
        return _fields.OrderByDescending(x=>x.Distance).ToList();
    }*/
}