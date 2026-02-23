using System.Net;
using Application.Responses;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Admin.Commands.UpdateUserStatus;

public class UpdateUserStatusCommandHandler : IRequestHandler<UpdateUserStatusCommand, Response<Unit>>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UpdateUserStatusCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Response<Unit>> Handle(UpdateUserStatusCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);

        if (user == null)
        {
            return new Response<Unit>(HttpStatusCode.NotFound, "Пользователь не найден.");
        }

        user.IsBlocked = request.IsBlocked;
        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            return new Response<Unit>(HttpStatusCode.BadRequest, "Не удалось обновить статус пользователя.");
        }

        var message = user.IsBlocked ? "Пользователь заблокирован." : "Пользователь разблокирован.";
        return new Response<Unit>(Unit.Value, message);
    }
}
