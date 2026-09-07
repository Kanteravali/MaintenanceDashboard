using System.Text.Json;
using MaintenanceDashboard.Models;

namespace MaintenanceDashboard.Services
{
    public class MaintenanceService
    {
        private readonly string _filePath;

        public MaintenanceService(IWebHostEnvironment environment)
        {
            _filePath = Path.Combine(
                environment.ContentRootPath,
                "Data",
                "maintenance.json");
        }

        public async Task<List<MaintenanceRequest>> GetAllAsync()
        {
            if (!File.Exists(_filePath))
            {
                return new List<MaintenanceRequest>();
            }

            var json = await File.ReadAllTextAsync(_filePath);

            return JsonSerializer.Deserialize<List<MaintenanceRequest>>(json)
                   ?? new List<MaintenanceRequest>();
        }

        public async Task<MaintenanceRequest?> GetByIdAsync(string id)
        {
            var requests = await GetAllAsync();

            return requests.FirstOrDefault(
                r => r.RequestId == id);
        }

        private async Task SaveAllAsync(
            List<MaintenanceRequest> requests)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize(
                requests,
                options);

            await File.WriteAllTextAsync(
                _filePath,
                json);
        }

        public async Task AddAsync(
            MaintenanceRequest request)
        {
            var requests = await GetAllAsync();

            requests.Add(request);

            await SaveAllAsync(requests);
        }

        public async Task<bool> UpdateAsync(
            MaintenanceRequest updatedRequest)
        {
            var requests = await GetAllAsync();

            var existingRequest =
                requests.FirstOrDefault(
                    r => r.RequestId ==
                         updatedRequest.RequestId);

            if (existingRequest == null)
            {
                return false;
            }

            existingRequest.EquipmentId =
                updatedRequest.EquipmentId;

            existingRequest.IssueTitle =
                updatedRequest.IssueTitle;

            existingRequest.Description =
                updatedRequest.Description;

            existingRequest.Priority =
                updatedRequest.Priority;

            existingRequest.Status =
                updatedRequest.Status;

            existingRequest.ReportedBy =
                updatedRequest.ReportedBy;

            existingRequest.CreatedDate =
                updatedRequest.CreatedDate;

            existingRequest.AssignedTo =
                updatedRequest.AssignedTo;

            await SaveAllAsync(requests);

            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var requests = await GetAllAsync();

            var request =
                requests.FirstOrDefault(
                    r => r.RequestId == id);

            if (request == null)
            {
                return false;
            }

            requests.Remove(request);

            await SaveAllAsync(requests);

            return true;
        }
    }
}