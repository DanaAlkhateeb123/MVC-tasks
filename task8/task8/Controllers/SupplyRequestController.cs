
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using task8.Data;
using task8.Models;
using Task8.Models;

[Authorize]
public class SupplyRequestController : Controller
{
    private readonly ApplicationDbContext _context;

    public SupplyRequestController(ApplicationDbContext context)
    {
        _context = context;
    }

    // 1. Index
    public IActionResult Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (User.IsInRole("Admin") || User.IsInRole("SuperAdmin"))
        {
            var requests = _context.supplyRequests
                .Include(r => r.requestItems)
                .ThenInclude(i => i.Product)
                .ToList();

            return View(requests);
        }
        else
        {
            var requests = _context.supplyRequests
                .Include(r => r.requestItems)
                .ThenInclude(i => i.Product)
                .Where(r => r.UserId == userId)
                .ToList();

            return View(requests);
        }
    }

    // 2. Create GET
    [HttpGet]
    public IActionResult Create()
    {
        var products = _context.products.ToList();

        return View(products);
    }

    // 3. Create POST
    [HttpPost]
    public IActionResult Create(int[] productIds, int[] quantities)
    {
        if (productIds == null || quantities == null ||
            productIds.Length != quantities.Length ||
            productIds.Length == 0)
        {
            ViewBag.Error = "Please select products.";
            return View(_context.products.ToList());
        }

        // Check all products before creating the request
        for (int i = 0; i < productIds.Length; i++)
        {
            var product = _context.products.Find(productIds[i]);

            if (product == null || quantities[i] <= 0 ||
                product.Quantity < quantities[i])
            {
                ViewBag.Error = "Invalid product or not enough quantity.";
                return View(_context.products.ToList());
            }
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        SupplyRequest request = new SupplyRequest();

        request.RequestDate = DateOnly.FromDateTime(DateTime.Now);
        request.Status = 0;
        request.UserId = userId;
        request.TotalPrice = 0;

        _context.supplyRequests.Add(request);
        _context.SaveChanges();

        double total = 0;

        for (int i = 0; i < productIds.Length; i++)
        {
            var product = _context.products.Find(productIds[i]);

            RequestItem item = new RequestItem();

            item.Id = Guid.NewGuid().ToString();
            item.SupplyRequestId = request.Id;
            item.ProductId = product.Id;
            item.Quantity = quantities[i];
            item.Price = product.Price;

            total = total + (product.Price * quantities[i]);

            product.Quantity = product.Quantity - quantities[i];

            _context.requestItems.Add(item);
        }

        request.TotalPrice = total;

        _context.SaveChanges();

        return RedirectToAction("Index");
    }

    // 4. Details
    public IActionResult Details(int id)
    {
        var request = _context.supplyRequests
            .Include(r => r.requestItems)
            .ThenInclude(i => i.Product)
            .FirstOrDefault(r => r.Id == id);

        if (request == null)
        {
            return NotFound();
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!User.IsInRole("Admin") &&
            !User.IsInRole("SuperAdmin") &&
            request.UserId != userId)
        {
            return Forbid();
        }

        return View(request);
    }

    // 5. Edit GET
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var request = _context.supplyRequests
            .Include(r => r.requestItems)
            .ThenInclude(i => i.Product)
            .FirstOrDefault(r => r.Id == id);

        if (request == null)
        {
            return NotFound();
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!User.IsInRole("Admin") &&
            !User.IsInRole("SuperAdmin") &&
            request.UserId != userId)
        {
            return Forbid();
        }

        if (request.Status != 0)
        {
            return Forbid();
        }

        ViewBag.Products = _context.products.ToList();

        return View(request);
    }

    // 6. Edit POST
    [HttpPost]
    public IActionResult Edit(int id, int[] productIds, int[] quantities)
    {
        var request = _context.supplyRequests
            .Include(r => r.requestItems)
            .FirstOrDefault(r => r.Id == id);

        if (request == null)
        {
            return NotFound();
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!User.IsInRole("Admin") &&
            !User.IsInRole("SuperAdmin") &&
            request.UserId != userId)
        {
            return Forbid();
        }

        if (request.Status != 0)
        {
            return Forbid();
        }

        if (productIds == null || quantities == null ||
            productIds.Length != quantities.Length ||
            productIds.Length == 0)
        {
            ViewBag.Error = "Please select products.";
            ViewBag.Products = _context.products.ToList();
            return View(request);
        }

        // Restore old quantities temporarily
        foreach (var item in request.requestItems)
        {
            var oldProduct = _context.products.Find(item.ProductId);

            if (oldProduct != null)
            {
                oldProduct.Quantity = oldProduct.Quantity + item.Quantity;
            }
        }

        // Check the new quantities
        for (int i = 0; i < productIds.Length; i++)
        {
            var product = _context.products.Find(productIds[i]);

            if (product == null || quantities[i] <= 0 ||
                product.Quantity < quantities[i])
            {
                // Restore the old stock if validation fails
                foreach (var item in request.requestItems)
                {
                    var oldProduct = _context.products.Find(item.ProductId);

                    if (oldProduct != null)
                    {
                        oldProduct.Quantity = oldProduct.Quantity - item.Quantity;
                    }
                }

                ViewBag.Error = "Invalid product or not enough quantity.";
                ViewBag.Products = _context.products.ToList();

                return View(request);
            }
        }

        _context.requestItems.RemoveRange(request.requestItems);

        double total = 0;

        for (int i = 0; i < productIds.Length; i++)
        {
            var product = _context.products.Find(productIds[i]);

            RequestItem item = new RequestItem();

            item.Id = Guid.NewGuid().ToString();
            item.SupplyRequestId = request.Id;
            item.ProductId = product.Id;
            item.Quantity = quantities[i];
            item.Price = product.Price;

            total = total + (product.Price * quantities[i]);

            product.Quantity = product.Quantity - quantities[i];

            _context.requestItems.Add(item);
        }

        request.TotalPrice = total;

        _context.SaveChanges();

        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var request = _context.supplyRequests
            .Include(r => r.requestItems)
            .ThenInclude(i => i.Product)
            .FirstOrDefault(r => r.Id == id);

        if (request == null)
        {
            return NotFound();
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!User.IsInRole("Admin") &&
            !User.IsInRole("SuperAdmin") &&
            request.UserId != userId)
        {
            return Forbid();
        }

        if (request.Status != 0)
        {
            return Forbid();
        }

        return View(request);
    }

    // 8. Delete POST
    [HttpPost]
    public IActionResult DeleteConfirmed(int id)
    {
        var request = _context.supplyRequests
            .Include(r => r.requestItems)
            .FirstOrDefault(r => r.Id == id);

        if (request == null)
        {
            return NotFound();
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!User.IsInRole("Admin") &&
            !User.IsInRole("SuperAdmin") &&
            request.UserId != userId)
        {
            return Forbid();
        }

        if (request.Status != 0)
        {
            return Forbid();
        }

        foreach (var item in request.requestItems)
        {
            var product = _context.products.Find(item.ProductId);

            if (product != null)
            {
                product.Quantity = product.Quantity + item.Quantity;
            }
        }

        _context.supplyRequests.Remove(request);

        _context.SaveChanges();

        return RedirectToAction("Index");
    }
}