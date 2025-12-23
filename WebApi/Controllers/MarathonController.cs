using System.Security.Claims;
using Application.Features.Marathon.Commands.SubmitAttempt;
using Application.Features.Marathon.Queries.GetLeaderboard;
using Application.Features.Marathon.Queries.GetMyBestResult;
using Application.Features.Marathon.Queries.GetMyRank;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers;

[ApiController]
[Route("marathon")]
public class MarathonController(IMediator mediator) : ControllerBase
{
    [Authorize]
    [HttpPost("attempts")]
    public async Task<IActionResult> SubmitAttempt([FromBody] SubmitMarathonAttemptCommand command)
    {
        var userId = User.FindFirstValue(Application.Constants.ClaimTypes.UserId);
        command.UserId = userId ?? string.Empty;

        var result = await mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }

    [Authorize]
    [HttpGet("my-best")]
    public async Task<IActionResult> GetMyBestResult()
    {
        var userId = User.FindFirstValue(Application.Constants.ClaimTypes.UserId);
        var query = new GetMyBestResultQuery { UserId = userId ?? string.Empty };

        var result = await mediator.Send(query);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("leaderboard")]
    public async Task<IActionResult> GetLeaderboard()
    {
        var query = new GetLeaderboardQuery();
        var result = await mediator.Send(query);

        return Ok(result);
    }

    [Authorize]
    [HttpGet("my-rank")]
    public async Task<IActionResult> GetMyRank()
    {
        var userId = User.FindFirstValue(Application.Constants.ClaimTypes.UserId);
        var query = new GetMyRankQuery { UserId = userId ?? string.Empty };

        var result = await mediator.Send(query);
        return StatusCode(result.StatusCode, result);
    }
}
