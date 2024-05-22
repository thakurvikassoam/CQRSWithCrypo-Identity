using MediatR;

namespace CQRSWithDocker_Identity.Features.Users.Commands.Delete
{
     public record DeleteUserCommand(Guid Id) : IRequest;

}
