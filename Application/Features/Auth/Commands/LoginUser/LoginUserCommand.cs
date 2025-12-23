using Application.DTOs;
using Application.Responses;
using MediatR;

namespace Application.Features.Auth.Commands.LoginUser;

public class LoginUserCommand : IRequest<Response<AuthResponseDto>>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
