
using MaintenanceDashboard.Services;
using Microsoft.AspNetCore.Mvc;

namespace MaintenanceDashboard.Controllers
{
    public class HomeController : Controller
    {
        private readonly EquipmentService _equipmentService;
        private readonly MaintenanceService _maintenanceService;

        public HomeController(
            EquipmentService equipmentService,
            MaintenanceService maintenanceService)
        {
            _equipmentService = equipmentService;
            _maintenanceService = maintenanceService;
        }

        public async Task<IActionResult> Index()
        {
            // Get all equipment
            var equipment =
                await _equipmentService.GetAllAsync();

            // Get all maintenance requests
            var requests =
                await _maintenanceService.GetAllAsync();

            // Dashboard statistics
            ViewBag.TotalEquipment = equipment.Count;

            ViewBag.OpenRequests =
                requests.Count(r => r.Status == "Open");

            ViewBag.InProgress =
                requests.Count(r => r.Status == "In Progress");

            ViewBag.Completed =
                requests.Count(r => r.Status == "Completed");

            ViewBag.CriticalIssues =
                requests.Count(r => r.Priority == "Critical");


            //-------------------------
            ViewBag.OnHold =
    requests.Count(r => r.Status == "On Hold");

            ViewBag.PriorityCritical =
                requests.Count(r => r.Priority == "Critical");

            ViewBag.PriorityHigh =
                requests.Count(r => r.Priority == "High");

            ViewBag.PriorityMedium =
                requests.Count(r => r.Priority == "Medium");

            ViewBag.PriorityLow =
                requests.Count(r => r.Priority == "Low");
            //------------------//

            // Get recent maintenance requests
            ViewBag.RecentRequests =
                requests
                    .OrderByDescending(r => r.CreatedDate)
                    .Take(5)
                    .ToList();

            // Equipment ID -> Equipment Name
            ViewBag.EquipmentNames =
                equipment.ToDictionary(
                    e => e.EquipmentId,
                    e => e.EquipmentName);

            return View();
        }
    }
}
