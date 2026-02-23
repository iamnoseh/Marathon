using Application.DTOs;
using Application.Responses;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Admin.Queries.GetUsers;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, Response<List<UserDto>>>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public GetUsersQueryHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Response<List<UserDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var usersQuery = _userManager.Users.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var lowerSearch = request.SearchTerm.ToLower();
            usersQuery = usersQuery.Where(u =>
                u.FullName.ToLower().Contains(lowerSearch) ||
                (u.Email != null && u.Email.ToLower().Contains(lowerSearch)));
        }

        var users = await usersQuery
            .OrderByDescending(u => u.CreatedAt)
            .Select(u => new UserDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email ?? string.Empty,
                ProfilePicture = u.ProfilePicture,
                CreatedAt = u.CreatedAt,
                IsBlocked = u.IsBlocked
            })
            .ToListAsync(cancellationToken);

        return new Response<List<UserDto>>(users);
    }
}
