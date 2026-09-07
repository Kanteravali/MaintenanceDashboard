
using MaintenanceDashboard.Services;
using Microsoft.AspNetCore.Mvc;

namespace MaintenanceDashboard.Controllers
{
    public class ApiController : Controller
    {
        private readonly ApiService _apiService;

        public ApiController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var users =
                    await _apiService.GetUsersAsync();

                return View(users);
            }
            catch (HttpRequestException)
            {
                ViewBag.ErrorMessage =
                    "Unable to load data from the API. Please try again later.";

                return View(new List<Models.ApiResponseModel>());
            }
            catch (TaskCanceledException)
            {
                ViewBag.ErrorMessage =
                    "The API request timed out. Please try again.";

                return View(new List<Models.ApiResponseModel>());
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage =
                    "Something went wrong while loading API data.";

                return View(new List<Models.ApiResponseModel>());
            }
        }
    }
}

