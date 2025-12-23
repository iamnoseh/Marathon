using Application.DTOs;
using Application.Responses;
using MediatR;

namespace Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenCommand : IRequest<Response<AuthResponseDto>>
{
    public string RefreshToken { get; set; } = string.Empty;
}
