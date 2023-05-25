﻿using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using SimonVossSearch.Core.Model;

namespace SimonVossSearch.Core;

public class Field
{
    public Guid Id { get; set; }
    public string Type { get; set; }
    public string Property { get; set; } // (??)
    public string Value { get; set; } 
    public float StrLen { get; set; }
    public float Distance { get; set; }
    public float Match { get; set; }
    
    public Field(Guid id, string type, string property, string value, string targetString)
    {
        Id = id;
        Type = type;
        Property = property;
        Value = value;
        Distance = CalculateDistance(value, targetString);
        StrLen = value.Length;
        Match = 1 - Distance / StrLen;
    }
    private int CalculateDistance(string src, string str)
    {
        var srcLength = src.Length;
        var strLength = str.Length;
        
        int[,] mtx = new int[srcLength + 1, strLength + 1];
        
        for (int i = 0; i <= srcLength; i++)
            mtx[i, 0] = i;
        for (int j = 0; j <= strLength; j++)
            mtx[0, j] = j;
        
        for (int i = 1; i <= srcLength; i++)
        {
            for (int j = 1; j <= strLength; j++)
            {
                int cost = (src[i - 1] == str[j - 1]) ? 0 : 1;
                mtx[i, j] = Math.Min(
                    Math.Min(mtx[i - 1, j] + 1, // delete
                        mtx[i, j - 1] + 1), // insert
                    mtx[i - 1, j - 1] + cost); // substitute
            }
        }

        return mtx[srcLength, strLength];
    
    }

    /*private int Weight()
    {
        switch (Type)
        {
            case "Building":
                switch (Property)
                {
                    case "ShortCut":
                        return 7;
                    case "Description":
                        return 5;
                    case "Name":
                        return 9;
                }

                ; break;
        }

        return 0;
    }*/
}

public static class Search
{
    public static void Execute(DataFile data, string str)
    {
        var allFieldsInAllBuildings = new List<Field>();

        foreach (var building in data.Buildings)
        {
            allFieldsInAllBuildings = allFieldsInAllBuildings.Concat(SearchInObject(building, str, building.Id)).ToList();
        }

        var res = allFieldsInAllBuildings.Count();
    }
    public static List<Field> SearchInObject(Object obj, string targetString, Guid id)
    {
        var allFieldsInAnObj = new List<Field>();
        Type type = obj.GetType();
        IList<PropertyInfo> props = new List<PropertyInfo>(type.GetProperties());

        foreach (var prop in props)
        {
            var field = new Field(id, type.Name, prop.Name, prop.GetValue(obj, null).ToString(), targetString);
            allFieldsInAnObj.Add(field);
        }

        return allFieldsInAnObj;
    }
}