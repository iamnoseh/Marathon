using Application.Responses;
using MediatR;

namespace Application.Features.Admin.Commands.UpdateUserStatus;

public class UpdateUserStatusCommand : IRequest<Response<Unit>>
{
    public string UserId { get; set; } = string.Empty;
    public bool IsBlocked { get; set; }
}
