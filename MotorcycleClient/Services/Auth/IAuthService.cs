using System.Threading.Tasks;

namespace MotorcycleClient.Services.Auth
{
    public interface IAuthService
    {
#if  WINDOWS
        Task<AuthResponse> AuthenticateAsync(MobilityService.MobilityServiceClient client, string username, string password);
#endif
    }
}
