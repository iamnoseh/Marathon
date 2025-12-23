using Application.Features.Auth.Commands.LoginUser;
using Application.Features.Auth.Commands.LogoutUser;
using Application.Features.Auth.Commands.RefreshToken;
using Application.Features.Auth.Commands.RegisterUser;
using Application.Features.Auth.Commands.UpdateProfile;
using Application.Features.Auth.Queries.GetUserProfile;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(
    IMediator mediator,
    IWebHostEnvironment environment)
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
        var query = new GetUserProfileQuery { UserId = userId ?? string.Empty };

        var result = await mediator.Send(query);
        return StatusCode(result.StatusCode, result);
    }

    [Authorize]
    [HttpPut("update-profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileCommand command)
    {
        var userId = User.FindFirstValue(Application.Constants.ClaimTypes.UserId);
        command.UserId = userId ?? string.Empty;

        var result = await mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }

    [Authorize]
    [HttpPost("upload-profile-picture")]
    public async Task<IActionResult> UploadProfilePicture([FromForm] IFormFile file)
    {
        var userId = User.FindFirstValue(Application.Constants.ClaimTypes.UserId);

        if (file == null || file.Length == 0)
        {
            return BadRequest(new { message = Application.Constants.Messages.Auth.FileNotSelected });
        }

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!allowedExtensions.Contains(extension))
        {
            return BadRequest(new { message = Application.Constants.Messages.Auth.InvalidFileType });
        }

        if (file.Length > 5 * 1024 * 1024)
        {
            return BadRequest(new { message = Application.Constants.Messages.Auth.FileTooLarge });
        }

        var uploadsFolder = Path.Combine(environment.WebRootPath, "uploads", "profiles");

        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        var fileName = $"{userId}_{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(uploadsFolder, fileName);

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var fileUrl = $"/uploads/profiles/{fileName}";

        var command = new Application.Features.Auth.Commands.UploadProfilePicture.UploadProfilePictureCommand
        {
            UserId = userId ?? string.Empty,
            FilePath = filePath,
            FileUrl = fileUrl
        };

        var result = await mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }
}
