using CQRSWithDocker_Identity.Data;
using MediatR;

namespace CQRSWithDocker_Identity.Features.Users.Commands.Delete
{
    public class DeleteProductCommandHandler(DataContext context) : IRequestHandler<DeleteUserCommand>
    {
        public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var product = await context.User.FindAsync(request.Id);
            if (product == null) return;
            context.User.Remove(product);
            await context.SaveChangesAsync();
            var log = new Domains.Loggers("DeleteUser", request.Id.ToString(), context.User.Remove(product).ToString());
            await context.Logger.AddAsync(log);
            await context.SaveChangesAsync();
        }
    }
}
