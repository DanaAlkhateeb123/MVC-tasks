using Microsoft.AspNetCore.Mvc;

namespace task3.Controllers
{
    public class SupplyRequestController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {  

            return View();
        }
        [HttpPost]
        public IActionResult AddItem(string PName, string category, int quantity,
            double price, string priority)
        {

            return View();
        }
        public IActionResult Review()
        {
            return View();
        }
        public IActionResult ConfirmRequest()
        {
            return View();
        }
        public IActionResult ClearDraft()
        {
            return View();
        }
        public IActionResult SavePreference()
        {
            return View();
        }
        public IActionResult MyRequests()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }

    }
}
