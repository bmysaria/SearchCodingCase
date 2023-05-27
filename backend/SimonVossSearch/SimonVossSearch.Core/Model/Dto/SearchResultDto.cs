namespace SimonVossSearch.Core.Model;

public class SearchResultDto
{
    public string Type { get; set; }
    public Guid ObjectId { get; set; }
    public string MatchedProperty { get; set; }
    public string MatchedValue { get; set; }
}

