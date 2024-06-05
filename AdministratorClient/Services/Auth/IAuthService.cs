namespace AdministratorClient.Services.Auth
{
    public interface IAuthService
    {
        Task<AuthResponse> AuthenticateAsync(MobilityService.MobilityServiceClient client);
    }
}