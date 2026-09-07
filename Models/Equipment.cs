namespace MaintenanceDashboard.Models
{
    public class Equipment
    {
        public string EquipmentId { get; set; } = string.Empty;

        public string EquipmentName { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public DateTime LastMaintenanceDate { get; set; }

        public string CurrentStatus { get; set; } = string.Empty;
    }
}