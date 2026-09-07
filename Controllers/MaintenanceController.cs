
using MaintenanceDashboard.Models;
using MaintenanceDashboard.Services;
using Microsoft.AspNetCore.Mvc;

namespace MaintenanceDashboard.Controllers
{
    public class MaintenanceController : Controller
    {
        private readonly MaintenanceService _maintenanceService;
        private readonly EquipmentService _equipmentService;

        public MaintenanceController(
            MaintenanceService maintenanceService,
            EquipmentService equipmentService)
        {
            _maintenanceService = maintenanceService;
            _equipmentService = equipmentService;
        }

        // Display maintenance requests
        // Supports search, status filter and priority filter
        public async Task<IActionResult> Index(
            string search,
            string status,
            string priority)
        {
            var requests =
                await _maintenanceService.GetAllAsync();

            var equipment =
                await _equipmentService.GetAllAsync();

            // Search by Request ID, Equipment Name or Issue Title
            if (!string.IsNullOrWhiteSpace(search))
            {
                var matchingEquipmentIds = equipment
                    .Where(e =>
                        e.EquipmentName.Contains(
                            search,
                            StringComparison.OrdinalIgnoreCase))
                    .Select(e => e.EquipmentId)
                    .ToList();

                requests = requests
                    .Where(r =>
                        r.RequestId.Contains(
                            search,
                            StringComparison.OrdinalIgnoreCase)
                        ||
                        r.IssueTitle.Contains(
                            search,
                            StringComparison.OrdinalIgnoreCase)
                        ||
                        matchingEquipmentIds.Contains(
                            r.EquipmentId))
                    .ToList();
            }

            // Filter by Status
            if (!string.IsNullOrWhiteSpace(status))
            {
                requests = requests
                    .Where(r => r.Status == status)
                    .ToList();
            }

            // Filter by Priority
            if (!string.IsNullOrWhiteSpace(priority))
            {
                requests = requests
                    .Where(r => r.Priority == priority)
                    .ToList();
            }

            // Create Equipment ID -> Equipment Name dictionary
            ViewBag.EquipmentNames = equipment
                .ToDictionary(
                    e => e.EquipmentId,
                    e => e.EquipmentName);

            // Keep filter values for the view
            ViewBag.Search = search;
            ViewBag.Status = status;
            ViewBag.Priority = priority;

            return View(requests);
        }

        // Display Create Request form
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var equipment =
                await _equipmentService.GetAllAsync();

            ViewBag.Equipment = equipment;

            return View();
        }

        // Create a new maintenance request
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            MaintenanceRequest request)
        {
            // Validate required fields
            if (!ModelState.IsValid)
            {
                var equipment =
                    await _equipmentService.GetAllAsync();

                ViewBag.Equipment = equipment;

                return View(request);
            }

            var existingRequests =
                await _maintenanceService.GetAllAsync();

            // Generate Request ID
            request.RequestId =
                "REQ-" +
                (existingRequests.Count + 1)
                .ToString("D3");

            // Set creation date
            request.CreatedDate = DateTime.Now;

            // New requests start as Open
            request.Status = "Open";

            await _maintenanceService.AddAsync(request);

            return RedirectToAction(nameof(Index));
        }

        // Display maintenance request details
        public async Task<IActionResult> Details(string id)
        {
            var request =
                await _maintenanceService.GetByIdAsync(id);

            if (request == null)
            {
                return NotFound();
            }

            var equipment =
                await _equipmentService.GetByIdAsync(
                    request.EquipmentId);

            ViewBag.Equipment = equipment;

            return View(request);
        }

        // Change status to In Progress
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StartWork(string id)
        {
            var request =
                await _maintenanceService.GetByIdAsync(id);

            if (request == null)
            {
                return NotFound();
            }

            request.Status = "In Progress";

            await _maintenanceService.UpdateAsync(request);

            return RedirectToAction(
                nameof(Details),
                new { id = request.RequestId });
        }

        // Change status to On Hold
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PutOnHold(string id)
        {
            var request =
                await _maintenanceService.GetByIdAsync(id);

            if (request == null)
            {
                return NotFound();
            }

            request.Status = "On Hold";

            await _maintenanceService.UpdateAsync(request);

            return RedirectToAction(
                nameof(Details),
                new { id = request.RequestId });
        }

        // Change status to Completed
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Complete(string id)
        {
            var request =
                await _maintenanceService.GetByIdAsync(id);

            if (request == null)
            {
                return NotFound();
            }

            request.Status = "Completed";

            await _maintenanceService.UpdateAsync(request);

            return RedirectToAction(
                nameof(Details),
                new { id = request.RequestId });
        }
    }
}

