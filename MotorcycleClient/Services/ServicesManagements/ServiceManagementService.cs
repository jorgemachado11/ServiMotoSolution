#if  WINDOWS
using System.Threading.Tasks;
using ServiMotoServer;

namespace MotorcycleClient.Services.ServicesManagements
{
    public class ServiceManagementService : IServiceManagementService
    {
        public async Task<bool> AssociateServiceAsync(MobilityService.MobilityServiceClient client, string clientId, string serviceId)
        {
            var associateServiceResponse = await client.AssociateServiceAsync(new ServiceRequest { ClientId = clientId, ServiceId = serviceId });
            return associateServiceResponse.Success;
        }

        public async Task<bool> DisassociateServiceAsync(MobilityService.MobilityServiceClient client, string clientId, string serviceId)
        {
            var disassociateServiceResponse = await client.DisassociateServiceAsync(new ServiceRequest { ClientId = clientId, ServiceId = serviceId });
            return disassociateServiceResponse.Success;
        }
    }
}
#endif
