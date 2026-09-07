using System.ComponentModel.DataAnnotations;

namespace MaintenanceDashboard.Models
{
    public class MaintenanceRequest
    {
        public string RequestId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select equipment.")]
        public string EquipmentId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Issue title is required.")]
        public string IssueTitle { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select priority.")]
        public string Priority { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        [Required(ErrorMessage = "Reported By is required.")]
        public string ReportedBy { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }

        public string AssignedTo { get; set; } = string.Empty;
    }
}