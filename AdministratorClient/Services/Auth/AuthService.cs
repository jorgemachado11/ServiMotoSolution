using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdministratorClient.Services.Auth
{
    public class AuthService : IAuthService
    {
        public async Task<AuthResponse> AuthenticateAsync(MobilityService.MobilityServiceClient client)
        {
            Console.WriteLine("Enter Administrator Username:");
            var username = Console.ReadLine();
            Console.WriteLine("Enter Password:");
            var password = Console.ReadLine();

            var authResponse = await client.AuthenticateAsync(new AuthRequest { Username = username, Password = password });

            if (authResponse.Success)
            {
                Console.WriteLine("Authentication successful!");
                Console.WriteLine($"Associated Service: {authResponse.ServiceName}");
            }
            else
            {
                Console.WriteLine("Authentication failed: " + authResponse.Message);
            }

            return authResponse;
        }
    }
}
