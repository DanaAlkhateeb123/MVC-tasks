using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using task66.Data;
using task66.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace task66.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            
        var categories = _context.Categories.Include(category => category.Products).ToList();

            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()    
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            _context.Categories.Add(category);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
       
        [HttpPost]

        public IActionResult Edit(int id ,Category category)
        {
            var categoryy = _context.Categories.Find(id);

            if (categoryy == null)
            {
                return NotFound();
            }

            categoryy.Name = category.Name;
            categoryy.Description = category.Description;

            _context.SaveChanges();

            return RedirectToAction("Index");

        }
        public IActionResult Details(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);

        }
        public IActionResult Delete(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction("Index");

        }

    }
}