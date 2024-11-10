using Microsoft.AspNetCore.Mvc;
using Fourier.Services;
using Microsoft.AspNetCore.Authorization;
using Fourier.Models;
namespace Fourier.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ProblemController : ControllerBase
{
    private readonly IProblemService problemService;
    private readonly ICancellationTokenService cancellationTokerService;

    public ProblemController(IProblemService problemService, ICancellationTokenService cancellationTokerService)
    {
        this.problemService = problemService;
        this.cancellationTokerService = cancellationTokerService;
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
    public async Task<IActionResult> CreateProblem([FromBody] Problem problem)
    {
        var createdProblem = await problemService.CreateTaskAsync(problem);
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