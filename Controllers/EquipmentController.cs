using MaintenanceDashboard.Models;
using MaintenanceDashboard.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MaintenanceDashboard.Controllers
{
    public class EquipmentController : Controller
    {
        private readonly EquipmentService _equipmentService;

        public EquipmentController(EquipmentService equipmentService)
        {
            _equipmentService = equipmentService;
        }

        public async Task<IActionResult> Index(
            string search,
            string category)
        {
            var equipment = await _equipmentService.GetAllAsync();

            // Search
            if (!string.IsNullOrWhiteSpace(search))
            {
                equipment = equipment
                    .Where(e =>
                        e.EquipmentId.Contains(search,
                            StringComparison.OrdinalIgnoreCase)
                        ||
                        e.EquipmentName.Contains(search,
                            StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Category filter
            if (!string.IsNullOrWhiteSpace(category))
            {
                equipment = equipment
                    .Where(e => e.Category == category)
                    .ToList();
            }

            ViewBag.Search = search;
            ViewBag.Category = category;

            ViewBag.Categories = (await _equipmentService.GetAllAsync())
                .Select(e => e.Category)
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            return View(equipment);


        }
        //pagination//







        public async Task<IActionResult> Details(string id)
        {
            var equipment = await _equipmentService.GetByIdAsync(id);

            if (equipment == null)
            {
                return NotFound();
            }

            return View(equipment);
        }

        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> Edit(string id)
        {
            var equipment = await _equipmentService.GetByIdAsync(id);

            if (equipment == null)
            {
                return NotFound();
            }

            return View(equipment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            string id,
            Equipment equipment)
        {
            if (id != equipment.EquipmentId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(equipment);
            }

            var updated = await _equipmentService.UpdateAsync(equipment);

            if (!updated)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(string id)
        {
            var equipment = await _equipmentService.GetByIdAsync(id);

            if (equipment == null)
            {
                return NotFound();
            }

            return View(equipment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var deleted = await _equipmentService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Equipment equipment)
        {
            if (!ModelState.IsValid)
            {
                return View(equipment);
            }

            var existingEquipment =
                await _equipmentService.GetByIdAsync(equipment.EquipmentId);

            if (existingEquipment != null)
            {
                ModelState.AddModelError(
                    "EquipmentId",
                    "Equipment ID already exists.");

                return View(equipment);
            }

            await _equipmentService.AddAsync(equipment);

            return RedirectToAction(nameof(Index));
        }
    }
}