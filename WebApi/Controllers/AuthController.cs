using Application.Features.Auth.Commands.LoginUser;
using Application.Features.Auth.Commands.LogoutUser;
using Application.Features.Auth.Commands.RefreshToken;
using Application.Features.Auth.Commands.RegisterUser;
using Application.Features.Auth.Commands.UpdateProfile;
using Application.Features.Auth.Queries.GetUserProfile;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(
    IMediator mediator,
    IFileStorageService fileStorageService)
    : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
    {
        var result = await mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
    {
        var result = await mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
    {
        var result = await mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutUserCommand command)
    {
        var result = await mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }

    [Authorize]
    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        var userId = User.FindFirstValue(Application.Constants.ClaimTypes.UserId);

        var query = new GetUserProfileQuery
        {
            UserId = userId ?? string.Empty
        };

        var result = await mediator.Send(query);
        return StatusCode(result.StatusCode, result);
    }

    [Authorize]
    [HttpPatch("update-profile")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UpdateProfile([FromForm] UpdateProfileRequest request)
    {
        var userId = User.FindFirstValue(Application.Constants.ClaimTypes.UserId);

        var command = new UpdateProfileCommand
        {
            UserId = userId ?? string.Empty,
            FullName = request.FullName
        };

        if (request.ProfilePicture != null)
        {
            await using var stream = request.ProfilePicture.OpenReadStream();

            command.ProfilePicturePath =
                await fileStorageService.SaveFileAsync(
                    stream,
                    request.ProfilePicture.FileName,
                    "profiles");
        }

        var result = await mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }
    public class UpdateProfileRequest
    {
        public string? FullName { get; set; }
        public IFormFile? ProfilePicture { get; set; }
    }
}
