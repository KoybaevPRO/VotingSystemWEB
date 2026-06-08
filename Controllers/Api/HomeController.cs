using Microsoft.AspNetCore.Mvc;
using VotingSystem.Models;

namespace VotingSystem.Controllers.Api;

[Route("api/home")]
[ApiController]
public class HomeController : ControllerBase
{
    [HttpGet("about")]
    public ActionResult<AboutResponse> About()
    {
        var developers = new List<DeveloperInfo>
        {
            new() { FullName = "Неграш Никита", Role = "Архитектор системы", Description = "Разработчик основной архитектуры приложения и базы данных" },
            new() { FullName = "Дзагоев Марат", Role = "Backend-разработчик", Description = "Разработчик серверной логики и API" },
            new() { FullName = "Тотоев Александр", Role = "Frontend-разработчик", Description = "Разработчик пользовательского интерфейса" }
        };

        return Ok(new AboutResponse
        {
            Developers = developers,
            Technologies = "Vue.js, ASP.NET Core Web API, EF Core, SQLite, Bootstrap"
        });
    }
}

public class AboutResponse
{
    public List<DeveloperInfo> Developers { get; set; } = new();
    public string Technologies { get; set; } = string.Empty;
}
