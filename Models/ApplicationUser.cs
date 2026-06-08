using Microsoft.AspNetCore.Identity;

namespace VotingSystem.Models;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Region { get; set; } = "Владикавказ";
}
