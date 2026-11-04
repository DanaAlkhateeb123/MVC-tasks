using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace UniversityCafeteria.Controllers
{
    public class SupplyRequestController : Controller
    {
        static List<string[]> confirmedRequests = new List<string[]>();
        static int requestCounter = 1;
        private List<string[]> GetDraft()
        {
            string data = HttpContext.Session.GetString("SupplyRequests");

            if (data == null)
            {
                return new List<string[]>();
            }

            return JsonSerializer.Deserialize<List<string[]>>(data);
        }
        private void SaveDraft(List<string[]> products)
        {
            string data = JsonSerializer.Serialize(products);

            HttpContext.Session.SetString("SupplyRequests", data);
        }
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.products = GetDraft();
            ViewBag.PreferredPriority = Request.Cookies["PreferredPriority"];
            return View();
        }
        [HttpPost]
        public IActionResult AddItem(string PName, string category, int quantity, double price, string priority)
        {
            if (string.IsNullOrWhiteSpace(PName))
            {
                ViewBag.Error = "Product name is required";
                return View("Index");
            }

            if (quantity <= 0)
            {
                ViewBag.Error = "Quantity must be greater than zero";
                return View("Index");
            }

            if (price <= 0)
            {
                ViewBag.Error = "Unit price must be greater than zero";
                return View("Index");
            }

            double total = quantity * price;


            string[] items =
            {
                PName,
                category,
                quantity.ToString(),
                price.ToString(),
                priority,
                total.ToString()
            };

            List<string[]> products = GetDraft();
            products.Add(items);
            SaveDraft(products);


            return RedirectToAction("Index");
        }
        public IActionResult Review()
        {
            List<string[]> products = GetDraft();
            if (products.Count == 0)
            {
                TempData["Error"] = "Your supply request is empty!";
                return RedirectToAction("Index");
            }

            double subtotal = 0;
            foreach (var item in products)
            {
                subtotal += Convert.ToDouble(item[5]);
            }

            double preparation = 0;
            switch (products[0][4])
            {
                case "normal":
                    preparation = 2;
                    break;
                case "urgent":
                    preparation = 5;
                    break;
                case "emergency":
                    preparation = 10;
                    break;
            }

            double discount = 0;
            if (subtotal > 200)
            {
                discount = subtotal * 0.15;
            }
            else if (subtotal > 100)
            {
                discount = subtotal * 0.10;
            }
            else
            {
                discount = 0;
            }

            double finalTotal = subtotal + preparation - discount;

            ViewBag.SubTotal = subtotal;
            ViewBag.PreparationFee = preparation;
            ViewBag.Discount = discount;
            ViewBag.Total = finalTotal;
            ViewBag.Products = products;
            return View();
        }
        public IActionResult ConfirmRequests()
        {
            List<string[]> products = GetDraft();

            if (products.Count == 0)
            {
                TempData["Error"] = "Your request is empty!";
                return RedirectToAction("Index");
            }

            double subtotal = 0;

            foreach (var item in products)
            {
                subtotal += Convert.ToDouble(item[5]);
            }

            double preparation = 0;

            switch (products[0][4])
            {
                case "normal":
                    preparation = 2;
                    break;

                case "urgent":
                    preparation = 5;
                    break;

                case "emergency":
                    preparation = 10;
                    break;
            }

            double discount = 0;

            if (subtotal > 200)
                discount = subtotal * 0.15;
            else if (subtotal > 100)
                discount = subtotal * 0.10;

            double finalTotal = subtotal + preparation - discount;

            string[] request =
            {

                requestCounter.ToString(),
                products.Count.ToString(),
                subtotal.ToString(),
                preparation.ToString(),
                discount.ToString(),
                finalTotal.ToString(),
                "Confirmed"
            };

            confirmedRequests.Add(request);

            requestCounter++;

            HttpContext.Session.Remove("SupplyRequests");

            return RedirectToAction("MyRequests");
        }

        public IActionResult ClearDraft()
        {
            HttpContext.Session.Remove("SupplyRequests");
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult SavePreference(string priority)
        {
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddDays(7);
            Response.Cookies.Append("PreferredPriority", priority, options);

            return RedirectToAction("Index");
        }
        public IActionResult MyRequests()
        {
            ViewBag.requests = confirmedRequests;

            return View();
        }
        public IActionResult About()
        {
            return View();
        }
    }
}