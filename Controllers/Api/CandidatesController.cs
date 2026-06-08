using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VotingSystem.Data;
using VotingSystem.DTOs;
using VotingSystem.Models;
using VotingSystem.Services;

namespace VotingSystem.Controllers.Api;

[Route("api/candidates")]
[ApiController]
public class CandidatesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JsonDataService _jsonService;

    public CandidatesController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        JsonDataService jsonService)
    {
        _context = context;
        _userManager = userManager;
        _jsonService = jsonService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<CandidatesListResponse>> GetAll([FromQuery] string? category)
    {
        var query = _context.Candidates.AsQueryable();

        if (!string.IsNullOrEmpty(category))
            query = query.Where(c => c.Category == category);

        var candidates = await query.ToListAsync();

        var voteCounts = await _context.Votes
            .GroupBy(v => v.CandidateId)
            .ToDictionaryAsync(g => g.Key, g => g.Count());

        var categories = await _context.Candidates
            .Select(c => c.Category)
            .Distinct()
            .OrderBy(c => c)
            .ToListAsync();

        var userId = _userManager.GetUserId(User);
        var userVoteIds = new HashSet<int>();
        if (userId != null)
        {
            userVoteIds = new HashSet<int>(await _context.Votes
                .Where(v => v.UserId == userId)
                .Select(v => v.CandidateId)
                .ToListAsync());
        }

        var result = candidates.Select(c => new CandidateResponse
        {
            Id = c.Id,
            FullName = c.FullName,
            Description = c.Description,
            Biography = c.Biography,
            Category = c.Category,
            PhotoUrl = c.PhotoUrl,
            CreatedAt = c.CreatedAt,
            VoteCount = voteCounts.GetValueOrDefault(c.Id, 0),
            HasVoted = userVoteIds.Contains(c.Id)
        }).ToList();

        return Ok(new CandidatesListResponse
        {
            Candidates = result,
            Categories = categories,
            SelectedCategory = category
        });
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<CandidateResponse>> GetById(int id)
    {
        var candidate = await _context.Candidates.FindAsync(id);
        if (candidate == null)
            return NotFound(new { message = "Кандидат не найден" });

        var voteCount = await _context.Votes.CountAsync(v => v.CandidateId == id);

        var hasVoted = false;
        var userId = _userManager.GetUserId(User);
        if (userId != null)
        {
            hasVoted = await _context.Votes.AnyAsync(v => v.CandidateId == id && v.UserId == userId);
        }

        return Ok(new CandidateResponse
        {
            Id = candidate.Id,
            FullName = candidate.FullName,
            Description = candidate.Description,
            Biography = candidate.Biography,
            Category = candidate.Category,
            PhotoUrl = candidate.PhotoUrl,
            CreatedAt = candidate.CreatedAt,
            VoteCount = voteCount,
            HasVoted = hasVoted
        });
    }

    [Authorize]
    [HttpPost("{id}/vote")]
    public async Task<ActionResult> Vote(int id, [FromBody] VoteRequest? request)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null)
            return Unauthorized();

        var candidate = await _context.Candidates.FindAsync(id);
        if (candidate == null)
            return NotFound(new { message = "Кандидат не найден" });

        var existingVote = await _context.Votes
            .FirstOrDefaultAsync(v => v.UserId == userId && v.CandidateId == id);

        if (existingVote != null)
            return BadRequest(new { message = "Вы уже голосовали за этого кандидата" });

        var vote = new Vote
        {
            CandidateId = id,
            UserId = userId,
            VoteDate = DateTime.UtcNow,
            Comment = request?.Comment
        };

        _context.Votes.Add(vote);
        await _context.SaveChangesAsync();

        await ExportToJson();

        return Ok(new { message = "Ваш голос учтён" });
    }

    [Authorize]
    [HttpGet("my-votes")]
    public async Task<ActionResult<List<UserVoteResponse>>> MyVotes()
    {
        var userId = _userManager.GetUserId(User);
        var votes = await _context.Votes
            .Include(v => v.Candidate)
            .Where(v => v.UserId == userId)
            .OrderByDescending(v => v.VoteDate)
            .ToListAsync();

        var result = votes.Select(v => new UserVoteResponse
        {
            Id = v.Id,
            CandidateName = v.Candidate?.FullName,
            CandidateId = v.CandidateId,
            CandidateCategory = v.Candidate?.Category,
            VoteDate = v.VoteDate,
            Comment = v.Comment,
            Type = "candidate"
        }).ToList();

        return Ok(result);
    }

    private async Task ExportToJson()
    {
        var candidates = await _context.Candidates.ToListAsync();
        var votes = await _context.Votes.ToListAsync();
        await _jsonService.SaveCandidatesAsync(candidates);
        await _jsonService.SaveVotesAsync(votes);
    }
}
