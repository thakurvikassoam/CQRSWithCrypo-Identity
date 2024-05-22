using CQRSWithDocker_Identity.Data;
using CQRSWithDocker_Identity.Features.Users.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CQRSWithDocker_Identity.Features.Users.Queries.Lists
{
    public class ListUserQueryHandler(DataContext context) : IRequestHandler<UserListQuery, List<UserDto>>
    {
        public async Task<List<UserDto>> Handle(UserListQuery request, CancellationToken cancellationToken)
        {
            var product = new Domains.Loggers("GetUserList", "", "");
            await context.Logger.AddAsync(product);
            await context.SaveChangesAsync();
            return await context.User.Select(p => new UserDto(p.Id, p.Name, p.Dept, p.Salary))
                .ToListAsync();
        }
    }
}
