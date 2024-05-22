using CQRSWithDocker_Identity.Data;
using MediatR;

namespace CQRSWithDocker_Identity.Features.Users.Commands.Create
{
    public class CreateUserCommandHandler(DataContext context) : IRequestHandler<CreateUserCommand, Guid>
    {
        public async Task<Guid> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var product = new Domains.Users(command.Name, command.Dept, command.Salary);
            await context.User.AddAsync(product);
            await context.SaveChangesAsync();
            var log = new Domains.Loggers("CreateUser", product.ToString(), product.Id.ToString());
            await context.Logger.AddAsync(log);
            await context.SaveChangesAsync();
            return product.Id;
        }
    }
}
