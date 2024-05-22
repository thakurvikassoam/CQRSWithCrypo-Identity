using CQRSWithDocker_Identity.Data;
using MediatR;

namespace CQRSWithDocker_Identity.Features.Loging_system.Commands.Create
{

    public class CreateLoggerCommandHandler(DataContext context) : IRequestHandler<CreateLoggerCommand, int>
    {
        public async Task<int> Handle(CreateLoggerCommand command, CancellationToken cancellationToken)
        {
            var product = new Domains.Loggers(command.Endpoint, command.request, command.response);
            await context.Logger.AddAsync(product);
            await context.SaveChangesAsync();
            return product.Id;
        }
    }
}
