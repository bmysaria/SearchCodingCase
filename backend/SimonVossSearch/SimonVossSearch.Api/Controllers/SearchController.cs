
using Microsoft.AspNetCore.Mvc;
using SimonVossSearch.Core;
using SimonVossSearch.Core.Model;

namespace SimonVossSearch.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class SearchController : Controller
{
    private readonly ISearchService _searchService;
    public SearchController(ISearchService searchService)
    {
        _searchService = searchService;
    }

    [HttpGet("{src}")]
    public List<SearchResultDto> SearchRequest(string src)
    {
        var res = _searchService.Execute(src);
        return res;
    }
}