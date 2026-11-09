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

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var supplier = _context.Suppliers.Find(id);
            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        [HttpPost]

        public IActionResult Edit(int id, Supplier supplier)
        {
            var supplierr = _context.Suppliers.Find(id);

            if (supplierr == null)
            {
                return NotFound();
            }

            supplierr.Name = supplier.Name;
            supplierr.Phone = supplier.Phone;
            supplierr.Email = supplier.Email;

            _context.SaveChanges();

            return RedirectToAction("Index");

        }
        public IActionResult Details(int id)
        {
            var supplier = _context.Suppliers.Find(id);
            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);

        }
        public IActionResult Delete(int id)
        {
            var supplier = _context.Suppliers.Find(id);
            if (supplier == null)
            {
                return NotFound();
            }
            _context.Suppliers.Remove(supplier);
            _context.SaveChanges();
            return RedirectToAction("Index");

        }

    }
}