using Microsoft.AspNetCore.Mvc;
using Fourier.Services;
using Microsoft.AspNetCore.Authorization;
using Fourier.DTOs;
using System.Security.Claims;
namespace Fourier.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ProblemController : ControllerBase
{
    private readonly IProblemService problemService;
    private readonly ICancellationTokenService cancellationTokerService;
    private readonly IServiceProvider serviceProvider;
    private readonly IUserService userService;

    public ProblemController(IProblemService problemService, ICancellationTokenService cancellationTokerService, IServiceProvider serviceProvider, IUserService userService)
    {
        this.problemService = problemService;
        this.cancellationTokerService = cancellationTokerService;
        this.serviceProvider = serviceProvider;
        this.userService = userService;
    }

    private Guid GetUserId()
    {
        return Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProblem(Guid id)
    {
        var problem = await problemService.GetTaskByIdAsync(id);
        if (problem == null)
        {
            return NotFound();
        }
        return Ok(problem);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProblem([FromBody] ProblemDto problem)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = GetUserId();
        var createdProblem = await problemService.CreateTaskAsync(problem, userId);
        createdProblem.User = await userService.GetUserByIdAsync(userId);
        /*        // get new context
                using(var scope = serviceProvider.CreateScope())
                {
                    var problemService = scope.ServiceProvider.GetRequiredService<FourierDbContext>();

                }*/
        createdProblem.CancellationToken = await cancellationTokerService.CreateCancellationTokenAsync(createdProblem.Id);
        return CreatedAtAction(nameof(GetProblem), new { id = createdProblem.Id }, createdProblem);
    }

    [HttpPut("{id}/cancel")]
    public async Task<IActionResult> CancelProblem(Guid id)
    {
        var problem = await problemService.GetTaskByIdAsync(id);
        if (problem == null)
        {
            return NotFound();
        }

        await cancellationTokerService.CancelProblemAsync(id);
        return NoContent();
    }
}