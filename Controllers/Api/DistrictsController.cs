using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VotingSystem.Data;
using VotingSystem.DTOs;
using VotingSystem.Models;

namespace VotingSystem.Controllers.Api;

[Route("api/districts")]
[ApiController]
public class DistrictsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public DistrictsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<List<DistrictResponse>>> GetAll()
    {
        var districts = await _context.Districts
            .Include(d => d.Projects)
            .ToListAsync();

        var result = districts.Select(d => new DistrictResponse
        {
            Id = d.Id,
            Name = d.Name,
            Description = d.Description,
            ImageUrl = d.ImageUrl,
            Population = d.Population,
            Area = d.Area,
            CreatedAt = d.CreatedAt,
            ProjectCount = d.Projects.Count
        }).ToList();

        return Ok(result);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<DistrictDetailsResponse>> GetById(int id)
    {
        var district = await _context.Districts
            .Include(d => d.Projects)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (district == null)
            return NotFound(new { message = "Район не найден" });

        var projectVoteCounts = await _context.ImprovementVotes
            .Where(v => v.Project!.DistrictId == id)
            .GroupBy(v => v.ProjectId)
            .ToDictionaryAsync(g => g.Key, g => g.Count());

        var votedProjectIds = new HashSet<int>();
        var userId = _userManager.GetUserId(User);
        if (userId != null)
        {
            votedProjectIds = new HashSet<int>(await _context.ImprovementVotes
                .Where(v => v.UserId == userId && v.Project!.DistrictId == id)
                .Select(v => v.ProjectId)
                .ToListAsync());
        }

        var response = new DistrictDetailsResponse
        {
            District = new DistrictResponse
            {
                Id = district.Id,
                Name = district.Name,
                Description = district.Description,
                ImageUrl = district.ImageUrl,
                Population = district.Population,
                Area = district.Area,
                CreatedAt = district.CreatedAt,
                ProjectCount = district.Projects.Count
            },
            Projects = district.Projects.Select(p => new ImprovementProjectResponse
            {
                Id = p.Id,
                DistrictId = p.DistrictId,
                Title = p.Title,
                Description = p.Description,
                Category = p.Category,
                BudgetEstimate = p.BudgetEstimate,
                VoteCount = projectVoteCounts.GetValueOrDefault(p.Id, 0),
                HasVoted = votedProjectIds.Contains(p.Id)
            }).ToList()
        };

        return Ok(response);
    }

    [Authorize]
    [HttpPost("projects/{projectId}/vote")]
    public async Task<ActionResult> VoteProject(int projectId)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null)
            return Unauthorized();

        var project = await _context.ImprovementProjects.FindAsync(projectId);
        if (project == null)
            return NotFound(new { message = "Проект не найден" });

        var existing = await _context.ImprovementVotes
            .FirstOrDefaultAsync(v => v.UserId == userId && v.ProjectId == projectId);

        if (existing != null)
            return BadRequest(new { message = "Вы уже голосовали за этот проект" });

        var vote = new ImprovementVote
        {
            ProjectId = projectId,
            UserId = userId,
            VoteDate = DateTime.UtcNow
        };

        _context.ImprovementVotes.Add(vote);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Ваш голос за проект учтён" });
    }

    [Authorize]
    [HttpGet("my-votes")]
    public async Task<ActionResult<List<UserVoteResponse>>> MyVotes()
    {
        var userId = _userManager.GetUserId(User);
        var votes = await _context.ImprovementVotes
            .Include(v => v.Project)
            .ThenInclude(p => p!.District)
            .Where(v => v.UserId == userId)
            .OrderByDescending(v => v.VoteDate)
            .ToListAsync();

        var result = votes.Select(v => new UserVoteResponse
        {
            Id = v.Id,
            ProjectTitle = v.Project?.Title,
            ProjectId = v.ProjectId,
            ProjectCategory = v.Project?.Category,
            DistrictName = v.Project?.District?.Name,
            VoteDate = v.VoteDate,
            Type = "improvement"
        }).ToList();

        return Ok(result);
    }
}
