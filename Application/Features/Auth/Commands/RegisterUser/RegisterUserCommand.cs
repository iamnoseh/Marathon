using Application.DTOs;
using Application.Responses;
using MediatR;

namespace Application.Features.Auth.Commands.RegisterUser;

public class RegisterUserCommand : IRequest<Response<AuthResponseDto>>
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
