using Application.Features.Reviews.Commands.AddReview;
using Application.Features.Reviews.Commands.ApproveReview;
using Application.Features.Reviews.Queries.GetApprovedReviews;
using Application.Features.Reviews.Queries.GetPendingReviews;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers;

[ApiController]
[Route("reviews")]
public class ReviewController(IMediator mediator) : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddReview([FromBody] AddReviewCommand command)
    {
        var userId = User.FindFirstValue(Application.Constants.ClaimTypes.UserId);
        command.UserId = userId ?? string.Empty;

        var result = await mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetApprovedReviews()
    {
        var query = new GetApprovedReviewsQuery();
        var result = await mediator.Send(query);
        return StatusCode(result.StatusCode, result);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("pending")]
    public async Task<IActionResult> GetPendingReviews()
    {
        var query = new GetPendingReviewsQuery();
        var result = await mediator.Send(query);
        return StatusCode(result.StatusCode, result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("{id}/approve")]
    public async Task<IActionResult> ApproveReview(int id)
    {
        var command = new ApproveReviewCommand { ReviewId = id };
        var result = await mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }
}
