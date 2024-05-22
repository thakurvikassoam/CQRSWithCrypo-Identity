using MediatR;

namespace CQRSWithDocker_Identity.Features.Loging_system.Commands.Create
{
   
    public record CreateLoggerCommand(string Endpoint, string response, string request) : IRequest<int>;
}
