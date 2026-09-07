using System.Text.Json;
using MaintenanceDashboard.Models;

namespace MaintenanceDashboard.Services
{
    public class EquipmentService
    {
        private readonly string _filePath;

        public EquipmentService(IWebHostEnvironment environment)
        {
            _filePath = Path.Combine(
                environment.ContentRootPath,
                "Data",
                "equipment.json");
        }

        public async Task<List<Equipment>> GetAllAsync()
        {
            if (!File.Exists(_filePath))
            {
                return new List<Equipment>();
            }

            var json = await File.ReadAllTextAsync(_filePath);

            return JsonSerializer.Deserialize<List<Equipment>>(json)
                   ?? new List<Equipment>();
        }

        public async Task<Equipment?> GetByIdAsync(string id)
        {
            var equipment = await GetAllAsync();

            return equipment.FirstOrDefault(
                e => e.EquipmentId == id);
        }

        private async Task SaveAllAsync(List<Equipment> equipment)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize(equipment, options);

            await File.WriteAllTextAsync(_filePath, json);
        }

        public async Task AddAsync(Equipment equipment)
        {
            var equipmentList = await GetAllAsync();

            equipmentList.Add(equipment);

            await SaveAllAsync(equipmentList);
        }

        public async Task<bool> UpdateAsync(Equipment updatedEquipment)
        {
            var equipmentList = await GetAllAsync();

            var existingEquipment = equipmentList.FirstOrDefault(
                e => e.EquipmentId == updatedEquipment.EquipmentId);

            if (existingEquipment == null)
            {
                return false;
            }

            existingEquipment.EquipmentName =
                updatedEquipment.EquipmentName;

            existingEquipment.Category =
                updatedEquipment.Category;

            existingEquipment.Location =
                updatedEquipment.Location;

            existingEquipment.LastMaintenanceDate =
                updatedEquipment.LastMaintenanceDate;

            existingEquipment.CurrentStatus =
                updatedEquipment.CurrentStatus;

            await SaveAllAsync(equipmentList);

            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var equipmentList = await GetAllAsync();

            var equipment = equipmentList.FirstOrDefault(
                e => e.EquipmentId == id);

            if (equipment == null)
            {
                return false;
            }

            equipmentList.Remove(equipment);

            await SaveAllAsync(equipmentList);

            return true;
        }
    }
}