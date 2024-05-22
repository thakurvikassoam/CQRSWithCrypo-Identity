using CQRSWithDocker_Identity.Data;
using CQRSWithDocker_Identity.Features.Loging_system.Dtos;
using CQRSWithDocker_Identity.Features.Users.Dtos;
using CQRSWithDocker_Identity.Features.Users.Queries.Lists;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CQRSWithDocker_Identity.Features.Loging_system.Queries.Lists
{
    
    public class ListLoggerQueryHandler(DataContext context) : IRequestHandler<LoggerQueryList, List<LoggerDto>>
    {
        public async Task<List<LoggerDto>> Handle(LoggerQueryList request, CancellationToken cancellationToken)
        {
            return await context.Logger
                .Select(p => new LoggerDto(p.Id, p.Endpoint, p.Request, p.Response))
                .ToListAsync();
        }
    }
}
