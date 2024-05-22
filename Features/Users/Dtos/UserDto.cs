namespace CQRSWithDocker_Identity.Features.Users.Dtos
{
    public record UserDto(Guid Id, string Name, string Dept, decimal Salary);
}
