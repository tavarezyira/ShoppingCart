using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ShoppingCartApp.Models;
using System;

public class ItemController : Controller
{
    private readonly ApplicationDbContext _context;

    public ItemController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Items
    public async Task<IActionResult> Index()
    {
         var items = await _context.Items
            .Include(i => i.Product)
            .ToListAsync();

        return View(items);
    }
    
    // GET: Items/Create (para agregar un producto al carrito)
    public IActionResult Create(int? productId)
    {
        if (productId == null)
        {
            return NotFound();
        }

        var product = _context.Products.Find(productId);
        if (product == null)
        {
            return NotFound();
        }

        var itemViewModel = new Item
        {
            Name = product.ProductName,
            Price = product.Price,
            ProductId = product.Id,
            Product = product,
            Quantity = 1
        };

        return View(itemViewModel);
    }

    // POST: Item/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Item item)
    {
        if (ModelState.IsValid)
        {
            _context.Add(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
            
        return View(item);
    }

    // GET: Items/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var item = await _context.Items.Include(i => i.Product).FirstOrDefaultAsync(m => m.Id == id);
        if (item == null)
        {
            return NotFound();
        }

        return View(item);
    }

    // POST: Items/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Quantity,Price,ProductId")] Item item)
    {
        if (id != item.Id)
        {
            return NotFound();
        }

        if(item.Product == null)
        {
            var product = _context.Products.Find(item.ProductId);
            if (product == null)
            {
                return NotFound();
            }
            else
            {
                item.Product = product;
                ModelState.Remove("Product");
            }
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(item);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Items.Any(e => e.Id == item.Id))
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
        return View(item);
    }

    // GET: Items/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var item = await _context.Items.FirstOrDefaultAsync(m => m.Id == id);
        if (item == null)
        {
            return NotFound();
        }

        return View(item);
    }

    // POST: Items/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var item = await _context.Items.FindAsync(id);
        _context.Items.Remove(item);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
