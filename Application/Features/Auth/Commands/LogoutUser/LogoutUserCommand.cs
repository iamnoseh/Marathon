using Application.Responses;
using MediatR;

namespace Application.Features.Auth.Commands.LogoutUser;

public class LogoutUserCommand : IRequest<Response<bool>>
{
    public string RefreshToken { get; set; } = string.Empty;
}
