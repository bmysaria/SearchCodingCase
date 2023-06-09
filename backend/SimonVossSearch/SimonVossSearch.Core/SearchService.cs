using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using Newtonsoft.Json;
using SimonVossSearch.Core.Mapper;
using SimonVossSearch.Core.Model;
using SimonVossSearch.Core.Parser;
using SimonVossSearch.Core.Vectorizer;

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
    private readonly TfidfVectorizer _vectorizer;
    private int ngram = 2;
    public SearchService(IDataFileParser parser)
    {
        _vectorizer = TfidfMemoryCache.GetOrCreate("vectorizer", parser);
    }
      public List<SearchResultDto> Execute(string targetString)
    {

        var wordTerms = FindWordTermsInTargetString(targetString);
        
        var wordVector = new double[_vectorizer.Terms.Count];
        var wordSum = 0d;
        var wordIndex = 0;
        var wordTotalSum = 0d;
        
        foreach (var term in _vectorizer.Terms)
        {
            if (wordTerms.ContainsKey(term))
            {
                wordVector[wordIndex] = wordTerms[term] * _vectorizer.idf[wordIndex];
                wordTotalSum += wordVector[wordIndex] * wordVector[wordIndex];
            }
            wordIndex++;
        }
        
        var normal = Math.Sqrt(1 / wordTotalSum);

        for (int j = 0; j < _vectorizer.Terms.Count; j++)
        {
            wordVector[j] *= normal;
        }
        
        var l = new List<Tuple<int, double>>();
        
        for (int i = 0; i < _vectorizer.Fields.Count; i++)
        {
            var v1 = _vectorizer.tf[i];
            var prod = v1.Zip(wordVector, (a, b) => a * b).Sum();
            l.Add(Tuple.Create(i, prod));
        }

        return CalculateWeight(l);
    }

    private Dictionary<string, int>  FindWordTermsInTargetString(string targetString)
    {
        Dictionary<string, int> wordTerms = new Dictionary<string, int>();
        
        for (int i = 0; i < targetString.Length - (ngram - 1); i++)
        {
            var term = targetString.Substring(i, ngram);
            if (wordTerms.TryGetValue(term, out var val))
            {
                wordTerms[term] += val + 1;
            }
            else
            {
                wordTerms[term] = 1;
            }
        }

        return wordTerms;
    }
    private List<SearchResultDto> CalculateWeight(List<Tuple<int, double>> l)
    {
        // calculate self-weight
        foreach (var x in l.OrderByDescending(x => x.Item2))
        {
            _vectorizer.Fields[x.Item1].CalculateWeight(x.Item2);
        }
        var primaryRes = _vectorizer.Fields.Where(x=>x.Weight>0).OrderByDescending(x => x.Weight);

        // find parents 
        Dictionary<Guid, Tuple<string, double>> maxValues = new Dictionary<Guid, Tuple<string, double>>();
        var parents = primaryRes.Where(x => x.ParentId == Guid.Empty).OrderBy(x => x.Weight);
        
        foreach (var field in parents)
        {
            if (!maxValues.ContainsKey(field.Id))
                maxValues.TryAdd(field.Id, Tuple.Create(field.Property, field.WCoef));
            else
                maxValues[field.Id] = Tuple.Create(field.Property, field.WCoef);
        }
        
        // give weight to children 
        foreach (var parentMaxValue in maxValues)
        {
            foreach (var child in _vectorizer.Fields)
            {
                if(child.ParentId == parentMaxValue.Key && (child.Property=="Name" || child.Property=="Owner"))
                    child.CalculateWeight(parentMaxValue.Value.Item1, parentMaxValue.Value.Item2);
            }
        }

        return SearchResultDtoMapper.Map(_vectorizer.Fields.Where(x=>x.Weight>0).OrderByDescending(x => x.Weight).ToList());
    }
    
}