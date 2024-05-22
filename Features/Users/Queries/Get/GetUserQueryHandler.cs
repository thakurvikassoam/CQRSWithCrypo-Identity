using CQRSWithDocker_Identity.Data;
using CQRSWithDocker_Identity.Features.Users.Dtos;
using MediatR;

namespace CQRSWithDocker_Identity.Features.Users.Queries.Get
{
    public class GetUSerQueryHandler(DataContext context)
  : IRequestHandler<GetUserQuery, UserDto?>
    {
        public async Task<UserDto?> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var product = await context.User.FindAsync(request.Id);
            if (product == null)
            {
                return null;
            }
            var log = new Domains.Loggers("getUserbyid", request.Id.ToString(), new UserDto(product.Id, product.Name, product.Dept, product.Salary).ToString());
            await context.Logger.AddAsync(log);
            await context.SaveChangesAsync();
            return new UserDto(product.Id, product.Name, product.Dept, product.Salary);
        }
    }
}
