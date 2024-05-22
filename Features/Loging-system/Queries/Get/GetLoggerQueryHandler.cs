using CQRSWithDocker_Identity.Data;
using CQRSWithDocker_Identity.Features.Loging_system.Dtos;
using CQRSWithDocker_Identity.Features.Users.Dtos;
using CQRSWithDocker_Identity.Features.Users.Queries.Get;
using MediatR;

namespace CQRSWithDocker_Identity.Features.Loging_system.Queries.Get
{
    public class GetLoggerQueryHandler(DataContext context)
  : IRequestHandler<GetLogsQuery, LoggerDto?>
    {
        public async Task<LoggerDto?> Handle(GetLogsQuery request, CancellationToken cancellationToken)
        {
            var product = await context.Logger.FindAsync(request.Id);
            if (product == null)
            {
                return null;
            }

            return new LoggerDto(product.Id, product.Endpoint, product.Request, product.Response);
        }
    }
}
