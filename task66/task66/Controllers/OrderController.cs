using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using task66.Data;
using task66.Models;

namespace task66.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var orders = _context.Orders
                .Include(order => order.Employee)
                .ToList();

            return View(orders);
        }

        [HttpGet]
        public IActionResult Create()
        {
            LoadEmployees();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(
            [Bind("CustomerName,OrderDate,TotalPrice,Status,EmployeeId")] Order order)
        {
            ValidateEmployee(order.EmployeeId);

            if (!ModelState.IsValid)
            {
                LoadEmployees(order.EmployeeId);
                return View(order);
            }

            _context.Orders.Add(order);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var order = _context.Orders.Find(id);

            if (order == null)
            {
                return NotFound();
            }

            LoadEmployees(order.EmployeeId);

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Order order)
        {
            var existingOrder = _context.Orders.Find(order.Id);

            if (existingOrder == null)
            {
                return NotFound();
            }

            ValidateEmployee(order.EmployeeId);

            if (!ModelState.IsValid)
            {
                LoadEmployees(order.EmployeeId);
                return View(order);
            }

            existingOrder.CustomerName = order.CustomerName;
            existingOrder.OrderDate = order.OrderDate;
            existingOrder.TotalPrice = order.TotalPrice;
            existingOrder.Status = order.Status;
            existingOrder.EmployeeId = order.EmployeeId;

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var order = _context.Orders
                .Include(order => order.Employee)
                .FirstOrDefault(order => order.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var order = _context.Orders.Find(id);

            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        private void LoadEmployees(int? employeeId = null)
        {
            ViewBag.EmployeeOptions = new SelectList(
                _context.Employees.ToList(),
                nameof(Employee.Id),
                nameof(Employee.FullName),
                employeeId);
        }

        private void ValidateEmployee(int employeeId)
        {
            if (!_context.Employees.Any(employee => employee.Id == employeeId))
            {
                ModelState.AddModelError(
                    nameof(Order.EmployeeId),
                    "Please select a valid employee.");
            }
        }
    }
}