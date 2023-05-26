namespace SimonVossSearch.Core.Model;

public class SearchResultDto
{
    public string Type { get; set; }
    public Guid Id { get; set; }
    public string MatchedProperty { get; set; }
    public string MatchedValue { get; set; }
}

public static class SearchResultDtoMapper
{
    public static List<SearchResultDto> Map(List<Field> fields)
    {
        List<SearchResultDto> res = new List<SearchResultDto>();
        
        foreach (var field in fields)
        {
            res.Add(new SearchResultDto(){Type = field.Type, Id = field.Id, MatchedProperty = field.Property,MatchedValue = field.Value});
        }

        return res;
    }
}