namespace VotingSystem.Models;

public class ImprovementVote
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public DateTime VoteDate { get; set; } = DateTime.UtcNow;
    public ImprovementProject? Project { get; set; }
}
