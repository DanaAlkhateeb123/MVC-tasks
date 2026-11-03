using Microsoft.AspNetCore.Mvc;

namespace task3.Controllers
{
    public class SupplyController : Controller
    {
        public IActionResult Index()
        {
         string[ ] Categories = 
        {
            "Drinks",
            "Food",
            "Snacks",
            "Hot Drinks",
            "Cold Drinks"
        };
            ViewBag.Categories=Categories;
            return View();
        }
        public IActionResult Products()
        {
            ViewData["Page Title"] = "Cafeteria Products";
            ViewData["Manager Name"] = "Ahmad Ali";
            ViewData["Number of Products"] = 5;
            string[] arr = { "Water", "Orange Juice", "Coffee", "Sandwich", "Chocolate" };
            ViewData ["array"] = arr;
            return View();
        }
        public IActionResult Suppliers()
        {
            ViewBag.PageTitle = "Our Suppliers";
            ViewBag.NumberOfSuppliers = 3;

            string[] arr =
            {
        "Fresh Food Company",
        "Happy Drinks Company",
        "Daily Supplies Company"
            };

            ViewBag.array = arr;

            return View();
        }

        [HttpGet]
        public IActionResult CreateRequest()
        {

            return View();
        }

        [HttpPost]
        public IActionResult SubmitRequest()
        {
            string employeeName = Request.Form["EmployeeName"].ToString();
            string productName = Request.Form["ProductName"].ToString();
            string quantityText = Request.Form["Quantity"].ToString();
            string requiredDate = Request.Form["RequiredDate"].ToString();
            if (string.IsNullOrWhiteSpace(employeeName))
            {
                ViewBag.Error = "Please enter the employee name.";

                return View("CreateRequest");
            }

            TempData["Message"] = "Request submitted successfully!";

            return RedirectToAction("About");
        }
        public IActionResult About()
        {
            return View();
        }
    }
}
