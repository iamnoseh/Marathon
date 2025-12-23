using Application.DTOs;
using Application.Responses;
using MediatR;

namespace Application.Features.Auth.Queries.GetUserProfile;

public class GetUserProfileQuery : IRequest<Response<UserProfileDto>>
{
    public string UserId { get; set; } = string.Empty;
}
