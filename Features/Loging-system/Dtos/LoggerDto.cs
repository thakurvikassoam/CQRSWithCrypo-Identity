namespace CQRSWithDocker_Identity.Features.Loging_system.Dtos
{
    
    public record LoggerDto(int Id, string endpoint, string request, string response);
}
