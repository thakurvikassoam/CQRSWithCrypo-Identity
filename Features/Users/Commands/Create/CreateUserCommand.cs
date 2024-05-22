using MediatR;

namespace CQRSWithDocker_Identity.Features.Users.Commands.Create
{
    public record CreateUserCommand(string Name, string Dept, decimal Salary) : IRequest<Guid>;
}
