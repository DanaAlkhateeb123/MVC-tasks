using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using task66.Data;
using task66.Models;

namespace task66.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var products = _context.Products
                .Include(p => p.category)
                .Include(p => p.supplier)
                .ToList();

            return View(products);
        }

        [HttpGet]
        public IActionResult Create()
        {
            LoadDropdowns();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (!_context.Categories.Any(
                c => c.Id == product.CategoryId))
            {
                ModelState.AddModelError(
                    "CategoryId", "Please select a valid category.");
            }

            if (!_context.Suppliers.Any(
                s => s.Id == product.SupplierId))
            {
                ModelState.AddModelError(
                    "SupplierId", "Please select a valid supplier.");
            }

            if (!ModelState.IsValid)
            {
                LoadDropdowns(
                    product.CategoryId,
                    product.SupplierId);

                return View(product);
            }

            _context.Products.Add(product);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        private void LoadDropdowns(
            int? categoryId = null,
            int? supplierId = null)
        {
            ViewBag.CategoryOptions = new SelectList(
                _context.Categories.ToList(),
                "Id",
                "Name",
                categoryId);

            ViewBag.SupplierOptions = new SelectList(
                _context.Suppliers.ToList(),
                "Id",
                "Name",
                supplierId);
        }
    }
}