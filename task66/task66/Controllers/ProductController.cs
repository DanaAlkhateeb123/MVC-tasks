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
            ValidateCategoryAndSupplier(product);

            if (!ModelState.IsValid)
            {
                LoadDropdowns(product.CategoryId, product.SupplierId);

                return View(product);
            }

            _context.Products.Add(product);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = _context.Products.Find(id);

            if (product == null)
            {
                return NotFound();
            }

            LoadDropdowns(product.CategoryId, product.SupplierId);

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product)
        {
            var existingProduct = _context.Products.Find(product.Id);

            if (existingProduct == null)
            {
                return NotFound();
            }

            ValidateCategoryAndSupplier(product);

            if (!ModelState.IsValid)
            {
                LoadDropdowns(product.CategoryId, product.SupplierId);

                return View(product);
            }

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Quantity = product.Quantity;
            existingProduct.CategoryId = product.CategoryId;
            existingProduct.SupplierId = product.SupplierId;

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var product = _context.Products
                .Include(p => p.category)
                .Include(p => p.supplier)
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);

            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
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

        private void ValidateCategoryAndSupplier(Product product)
        {
            if (!_context.Categories.Any(c => c.Id == product.CategoryId))
            {
                ModelState.AddModelError(
                    nameof(Product.CategoryId),
                    "Please select a valid category.");
            }

            if (!_context.Suppliers.Any(s => s.Id == product.SupplierId))
            {
                ModelState.AddModelError(
                    nameof(Product.SupplierId),
                    "Please select a valid supplier.");
            }
        }
    }
}