using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VotingSystem.Models;

namespace VotingSystem.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Candidate> Candidates => Set<Candidate>();
    public DbSet<Vote> Votes => Set<Vote>();
    public DbSet<District> Districts => Set<District>();
    public DbSet<ImprovementProject> ImprovementProjects => Set<ImprovementProject>();
    public DbSet<ImprovementVote> ImprovementVotes => Set<ImprovementVote>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Vote>()
            .HasOne(v => v.Candidate)
            .WithMany()
            .HasForeignKey(v => v.CandidateId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Vote>()
            .HasIndex(v => new { v.UserId, v.CandidateId })
            .IsUnique();

        builder.Entity<ImprovementProject>()
            .HasOne(p => p.District)
            .WithMany(d => d.Projects)
            .HasForeignKey(p => p.DistrictId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ImprovementVote>()
            .HasOne(v => v.Project)
            .WithMany(p => p.Votes)
            .HasForeignKey(v => v.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ImprovementVote>()
            .HasIndex(v => new { v.UserId, v.ProjectId })
            .IsUnique();
    }
}
