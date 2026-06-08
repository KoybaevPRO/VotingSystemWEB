namespace VotingSystem.DTOs;

public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class RegisterRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Region { get; set; } = "Владикавказ";
}

public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}

public class VoteRequest
{
    public int CandidateId { get; set; }
    public string? Comment { get; set; }
}

public class ProjectVoteRequest
{
    public int ProjectId { get; set; }
}

public class CandidateResponse
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Biography { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string PhotoUrl { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int VoteCount { get; set; }
    public bool HasVoted { get; set; }
}

public class CandidatesListResponse
{
    public List<CandidateResponse> Candidates { get; set; } = new();
    public List<string> Categories { get; set; } = new();
    public string? SelectedCategory { get; set; }
}

public class DistrictResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public int Population { get; set; }
    public double Area { get; set; }
    public DateTime CreatedAt { get; set; }
    public int ProjectCount { get; set; }
}

public class ImprovementProjectResponse
{
    public int Id { get; set; }
    public int DistrictId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string BudgetEstimate { get; set; } = string.Empty;
    public int VoteCount { get; set; }
    public bool HasVoted { get; set; }
}

public class DistrictDetailsResponse
{
    public DistrictResponse District { get; set; } = null!;
    public List<ImprovementProjectResponse> Projects { get; set; } = new();
}

public class UserVoteResponse
{
    public int Id { get; set; }
    public string? CandidateName { get; set; }
    public int? CandidateId { get; set; }
    public string? CandidateCategory { get; set; }
    public string? ProjectTitle { get; set; }
    public int? ProjectId { get; set; }
    public string? ProjectCategory { get; set; }
    public string? DistrictName { get; set; }
    public DateTime VoteDate { get; set; }
    public string? Comment { get; set; }
    public string Type { get; set; } = string.Empty;
}
