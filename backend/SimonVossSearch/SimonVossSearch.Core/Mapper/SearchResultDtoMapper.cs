using SimonVossSearch.Core.Model;

namespace SimonVossSearch.Core.Mapper;

public static class SearchResultDtoMapper
{
    public static List<SearchResultDto> Map(List<Field> fields)
    {
        List<SearchResultDto> res = new List<SearchResultDto>();
        
        foreach (var field in fields)
        {
            res.Add(new SearchResultDto(){Type = field.Type, ObjectId = field.Id, MatchedProperty = field.Property,MatchedValue = field.Value});
        }

        return res;
    }
}