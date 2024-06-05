#if WINDOWS
using System.Threading.Tasks;
using ServiMotoServer;

namespace MotorcycleClient.Services.Auth
{
    public class AuthService : IAuthService
    {
        public async Task<AuthResponse> AuthenticateAsync(MobilityService.MobilityServiceClient client, string username, string password)
        {
            try
            {
                var authResponse = await client.AuthenticateMotorcycleAsync(new AuthRequest { Username = username, Password = password });

                if (authResponse.Success)
                {
                    return authResponse;
                }
                else
                {
                    return new AuthResponse { Success = false, Message = "Authentication failed" };
                }
            }
            catch (Exception ex)
            {
              return new AuthResponse { Success = false, Message = "Authentication failed" };
            }
        }
    }
}
#endif
