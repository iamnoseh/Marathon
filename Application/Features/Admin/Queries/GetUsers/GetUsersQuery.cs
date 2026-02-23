using Application.DTOs;
using Application.Responses;
using MediatR;

namespace Application.Features.Admin.Queries.GetUsers;

public class GetUsersQuery : IRequest<Response<List<UserDto>>>
{
    public string? SearchTerm { get; set; }
}
