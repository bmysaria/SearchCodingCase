using Microsoft.AspNetCore.Mvc;
using Persistence;
using SimonVossSearch.Domain;

namespace SimonVossSearch.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DetalizationController : Controller
{
    private readonly SearchDbContext _dbContext;

    public DetalizationController(SearchDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    [HttpGet("building/{id}")]
    public Building GetBuilding(Guid id)
    {
        return _dbContext.Buildings.Find(id);
    }

    [HttpGet("lock/{id}")]
    public Lock GetLock(Guid id)
    {
        return _dbContext.Locks.Find(id);
    }

    [HttpGet("medium/{if}")]
    public Medium GetMedium(Guid id)
    {
        return _dbContext.Media.Find(id);
    }

    [HttpGet("group/{id}")]
    public Group GetGroup(Guid id)
    {
        return _dbContext.Groups.Find(id);
    }
}