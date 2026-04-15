using AnimalClinicLogic.Models;
using AnimalClinicLogic.Services;
using Grpc.Core;

namespace AnimalClinic.Grpc
{
    public class ClinicStatsGrpcService : ClinicStatsService.ClinicStatsServiceBase
    {
        private readonly AdminService adminService;

        public ClinicStatsGrpcService(AdminService adminService)
        {
            this.adminService = adminService;
        }

        public override async Task<StatsResponse> GetClinicStats(StatsRequest request, ServerCallContext context)
        {
            var (stats, fromCache) = await adminService.GetStatistics();

            var response = new StatsResponse
            {
                TotalUsers = stats.TotalUsers,
                TotalAnimals = stats.TotalAnimals,
                AverageAnimalAge = stats.AverageAnimalAge,
                Source = fromCache ? "cache" : "database"
                
            };

            foreach (var item in stats.AnimalsByType)
            {
                response.AnimalsByType.Add(item.Key, item.Value);
            }

            return response;
        }
    }
}
