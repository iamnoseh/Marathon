using Application.Features.Auth.Commands.LoginUser;
using Application.Features.Auth.Commands.LogoutUser;
using Application.Features.Auth.Commands.RefreshToken;
using Application.Features.Auth.Commands.RegisterUser;
using Application.Features.Auth.Commands.UpdateProfile;
using Application.Features.Auth.Queries.GetUserProfile;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(IMediator mediator,
    IFileStorageService fileStorageService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
    {
        var result = await mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("login")]
    [Consumes("multipart/form-data")]
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
        var query = new GetUserProfileQuery { UserId = userId ?? string.Empty };

        var result = await mediator.Send(query);
        return StatusCode(result.StatusCode, result);
    }

    [Authorize]
    [HttpPost("update-profile")]
    public async Task<IActionResult> UpdateProfile([FromForm] string? fullName, [FromForm] IFormFile? profilePicture)
    {
        var userId = User.FindFirstValue(Application.Constants.ClaimTypes.UserId);
        
        var command = new UpdateProfileCommand
        {
            UserId = userId ?? string.Empty,
            FullName = fullName
        };


        if (profilePicture != null)
        {
            await using var stream = profilePicture.OpenReadStream();
            command.ProfilePicturePath = await fileStorageService.SaveFileAsync(stream, profilePicture.FileName, "profiles");
        }

        var result = await mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }
}
