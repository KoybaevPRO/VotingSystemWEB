namespace VotingSystem.Models;

public class District
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public int Population { get; set; }
    public double Area { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<ImprovementProject> Projects { get; set; } = new();
}
