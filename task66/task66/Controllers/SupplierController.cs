using Microsoft.AspNetCore.Mvc;
using task66.Data;
using task66.Models;

namespace task66.Controllers
{
    public class SupplierController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SupplierController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var suppliers = _context.Suppliers.ToList();

            return View(suppliers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Supplier supplier)
        {
            if (!ModelState.IsValid)
            {
                return View(supplier);
            }

            _context.Suppliers.Add(supplier);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}