
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using task8.Data;
using task8.Models;
using Task8.Models;

public class RequestItemController : Controller
{
    private readonly ApplicationDbContext _context;

    public RequestItemController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: REQUESTITEMS
    public async Task<IActionResult> Index()
    {
        return View(await _context.requestItems.ToListAsync());
    }

    // GET: REQUESTITEMS/Details/5
    public async Task<IActionResult> Details(string? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var requestitem = await _context.requestItems
            .FirstOrDefaultAsync(m => m.Id == id);
        if (requestitem == null)
        {
            return NotFound();
        }

        return View(requestitem);
    }

    // GET: REQUESTITEMS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: REQUESTITEMS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Quantity,Price,ProductId,SupplyRequestId,Product,SupplyRequest")] RequestItem requestitem)
    {
        if (ModelState.IsValid)
        {
            _context.Add(requestitem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(requestitem);
    }

    // GET: REQUESTITEMS/Edit/5
    public async Task<IActionResult> Edit(string? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var requestitem = await _context.requestItems.FindAsync(id);
        if (requestitem == null)
        {
            return NotFound();
        }
        return View(requestitem);
    }

    // POST: REQUESTITEMS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string? id, [Bind("Id,Quantity,Price,ProductId,SupplyRequestId,Product,SupplyRequest")] RequestItem requestitem)
    {
        if (id != requestitem.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(requestitem);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestItemExists(requestitem.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(requestitem);
    }

    // GET: REQUESTITEMS/Delete/5
    public async Task<IActionResult> Delete(string? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var requestitem = await _context.requestItems
            .FirstOrDefaultAsync(m => m.Id == id);
        if (requestitem == null)
        {
            return NotFound();
        }

        return View(requestitem);
    }

    // POST: REQUESTITEMS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string? id)
    {
        var requestitem = await _context.requestItems.FindAsync(id);
        if (requestitem != null)
        {
            _context.requestItems.Remove(requestitem);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool RequestItemExists(string? id)
    {
        return _context.requestItems.Any(e => e.Id == id);
    }
}