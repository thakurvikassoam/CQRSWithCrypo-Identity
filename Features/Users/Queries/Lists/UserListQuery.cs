using CQRSWithDocker_Identity.Features.Users.Dtos;
using MediatR;

namespace CQRSWithDocker_Identity.Features.Users.Queries.Lists
{
    public record UserListQuery : IRequest<List<UserDto>>;
}
