namespace VotingSystem.Models;

public class ImprovementProject
{
    public int Id { get; set; }
    public int DistrictId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string BudgetEstimate { get; set; } = string.Empty;
    public District? District { get; set; }
    public List<ImprovementVote> Votes { get; set; } = new();
}
