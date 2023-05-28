using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using Newtonsoft.Json;
using SimonVossSearch.Core.Model;
using SimonVossSearch.Core.Model.Mapper;
using SimonVossSearch.Core.Model.Parser;

namespace SimonVossSearch.Core;
using System;
using System.Collections.Generic;
using System.Linq;

public interface ISearchService
{
    public List<SearchResultDto> Execute(string targetString);
}
public class SearchService : ISearchService
{ 
    public List<SearchResultDto> Execute(string targetString)
    {

        var parser = new DataFileParser();
        var fields = parser.Execute();

        var ngram = targetString.Length < 3 ? 2 : 3;

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

            idf[idfIndex] = Math.Log((fields.Count + 1) / (double)(docsCount + 1)) + 1;
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
        for (int i = 0; i < targetString.Length - (ngram - 1); i++)
        {
            var term = targetString.Substring(i, ngram); // ignore case ??
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

        var res = fields.Where(x=>x.Weight>0).OrderByDescending(x => x.Weight);

        Dictionary<Guid, Tuple<string, double>> maxValues = new Dictionary<Guid, Tuple<string, double>>();

        var parents = res.Where(x => x.ParentId == Guid.Empty).OrderBy(x => x.Weight);
        foreach (var field in parents)
        {
            if (!maxValues.ContainsKey(field.Id))
                maxValues.TryAdd(field.Id, Tuple.Create(field.Property, field.WCoef));
            else
                maxValues[field.Id] = Tuple.Create(field.Property, field.WCoef);
        }
        
        
        foreach (var parentMaxValue in maxValues)
        {
            foreach (var child in fields)
            {
                if(child.ParentId == parentMaxValue.Key && (child.Property=="Name" || child.Property=="Owner"))
                    child.CalculateWeight(parentMaxValue.Value.Item1, parentMaxValue.Value.Item2);
            }
        }
        
        res = fields.Where(x=>x.Weight>0).OrderByDescending(x => x.Weight);
        
        return SearchResultDtoMapper.Map(res.ToList());
    }

    
}