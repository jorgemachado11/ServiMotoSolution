using System.Threading.Tasks;

namespace MotorcycleClient.Services.ServicesManagements
{
    public interface IServiceManagementService
    {
#if  WINDOWS
        Task<bool> AssociateServiceAsync(MobilityService.MobilityServiceClient client, string clientId, string serviceId);
        Task<bool> DisassociateServiceAsync(MobilityService.MobilityServiceClient client, string clientId, string serviceId);
#endif
    }
}
