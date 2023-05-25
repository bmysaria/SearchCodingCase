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
    private string _targetString;

    public Search(DataFile data, string targetString)
    {
        this._data = data;
        _targetString = targetString.ToLower();
    }

    public /*List<string> */ void Execute()
    {

        var fields = GetFields();
        
        var ngram = _targetString.Length < 3 ? 2 : 3;
        
    var terms = new HashSet<string>();
    var docFreq = new List<Dictionary<string, int>>();
    foreach (var field in fields)
    {
        Dictionary<string, int> docTerms = new Dictionary<string, int>();
        for (int i = 0; i < field.Value.Length - (ngram - 1); i++)
        {
            var term = field.Value.Substring(i, ngram);
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
    var tf = new double[docFreq.Count][];
    for (int i = 0; i < docFreq.Count; i++)
    {
        var docTf = docFreq[i];
        tf[i] = new double[terms.Count];
        var index = 0;
        foreach (var term in terms)
        {
            if (docTf.TryGetValue(term, out var val))
            {
                tf[i][index] = val;
            }

            index++;
        }
    }
    var idf = new double[terms.Count];
    var idfIndex = 0;
    foreach (var t in terms)
    {
        var docsCount = 0;
        foreach (var docF in docFreq)
        {
            if (docF.ContainsKey(t))
            {
                docsCount++;
            }
        }

        idf[idfIndex] = Math.Log((fields.Count + 1) / (double) (docsCount + 1)) + 1;
        idfIndex++;
    }

    for (int i = 0; i < fields.Count; i++)
    {
        var totalSum = 0d;
        for (int j = 0; j < terms.Count; j++)
        {
            tf[i][j] *= idf[j];
            totalSum += tf[i][j] * tf[i][j];
        }

        var normal = Math.Sqrt(1 / totalSum);
        for (int j = 0; j < terms.Count; j++)
        {
            tf[i][j] *= normal;
        }
    }


    Dictionary<string, int> wordTerms = new Dictionary<string, int>();
    for (int i = 0; i < _targetString.Length - (ngram - 1); i++)
    {
        var term = _targetString.Substring(i, ngram); // ignore case ??
        if (wordTerms.TryGetValue(term, out var val))
        {
            wordTerms[term] += val + 1;
        }
        else
        {
            wordTerms[term] = 1;
        }
    }

    var wordVector = new double[terms.Count];
    var wordSum = 0d;
    var wordIndex = 0;
    var wordTotalSum = 0d;
    foreach (var term in terms)
    {
        if (wordTerms.ContainsKey(term))
        {
            wordVector[wordIndex] = wordTerms[term] * idf[wordIndex];
            wordTotalSum += wordVector[wordIndex] * wordVector[wordIndex];
        }
        wordIndex++;
        
    }

    {
        var normal = Math.Sqrt(1 / wordTotalSum);

        for (int j = 0; j < terms.Count; j++)
        {
            wordVector[j] *= normal;
        }
    }


    var l = new List<Tuple<int, double>>();
    for (int i = 0; i < fields.Count; i++)
    {
        var v1 = tf[i];
        var prod = v1.Zip(wordVector, (a, b) => a * b).Sum();
        l.Add(Tuple.Create(i, prod));
    }

    var ord = l.OrderByDescending(x => x.Item2);
    foreach (var x in ord)
    {
        fields[x.Item1].CalculateWeight(x.Item2);
    }
    
    foreach (var x in fields.OrderByDescending(x=>x.Weight).Take(10))
    {
        Console.WriteLine(x.Weight + " " + x.Value);
    }
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
                if (prop.Name == "Id" || prop.Name == "BuildingId" || prop.Name == "GroupId" || prop.Name == "Mediums" || prop.Name == "Locks")
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