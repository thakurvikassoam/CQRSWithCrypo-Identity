using CQRSWithDocker_Identity.Features.Users.Dtos;
using MediatR;

namespace CQRSWithDocker_Identity.Features.Users.Queries.Get
{
    public record GetUserQuery(Guid Id) : IRequest<UserDto>;
}
