using System.Text.Json;
using VotingSystem.Models;

namespace VotingSystem.Services;

public class JsonDataService
{
    private readonly string _dataDir;

    public JsonDataService(IWebHostEnvironment env)
    {
        _dataDir = Path.Combine(env.ContentRootPath, "json-data");
        Directory.CreateDirectory(_dataDir);
    }

    public async Task<List<Candidate>> LoadCandidatesAsync()
    {
        var path = Path.Combine(_dataDir, "candidates.json");
        if (!File.Exists(path))
            return new List<Candidate>();

        var json = await File.ReadAllTextAsync(path);
        return JsonSerializer.Deserialize<List<Candidate>>(json) ?? new List<Candidate>();
    }

    public async Task SaveCandidatesAsync(List<Candidate> candidates)
    {
        var path = Path.Combine(_dataDir, "candidates.json");
        var json = JsonSerializer.Serialize(candidates, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(path, json);
    }

    public async Task<List<Vote>> LoadVotesAsync()
    {
        var path = Path.Combine(_dataDir, "votes.json");
        if (!File.Exists(path))
            return new List<Vote>();

        var json = await File.ReadAllTextAsync(path);
        return JsonSerializer.Deserialize<List<Vote>>(json) ?? new List<Vote>();
    }

    public async Task SaveVotesAsync(List<Vote> votes)
    {
        var path = Path.Combine(_dataDir, "votes.json");
        var json = JsonSerializer.Serialize(votes, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(path, json);
    }
}
