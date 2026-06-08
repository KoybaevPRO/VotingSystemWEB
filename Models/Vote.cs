namespace VotingSystem.Models;

public class Vote
{
    public int Id { get; set; }
    public int CandidateId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public DateTime VoteDate { get; set; } = DateTime.UtcNow;
    public string? Comment { get; set; }

    public Candidate? Candidate { get; set; }
}
