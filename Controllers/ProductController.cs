using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ShoppingCartApp.Models;
using System;
using Microsoft.AspNetCore.Authorization;

namespace ShoppingCartApp.Controllers;

[Authorize(Roles = "Admin")]
public class ProductController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProductController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Products
    public async Task<IActionResult> Index()
    {
        return View(await _context.Products.ToListAsync());
    }

    // GET: Products/Create
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Product product, IFormFile imageFile)
    {
        if (ModelState.IsValid)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var fileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
                var extension = Path.GetExtension(imageFile.FileName);
                var uniqueFileName = $"{fileName}_{Guid.NewGuid()}{extension}";

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                product.ProductImage = "/images/" + uniqueFileName;
            }
            else
            {
                ModelState.AddModelError("Product Image", "Please upload an image.");
                return View(product);
            }

            _context.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(product);
    }

    // GET: Products/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
        if (product == null)
        {
            return NotFound();
        }
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), product.ProductImage);
        ViewBag.ImageFile = CreateFormFileFromPath(filePath);
        ViewBag.viewImage = product.ProductImage;

        return View(product);
    }

    public IFormFile CreateFormFileFromPath(string filePath)
    {
        try
        {
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            var extension = Path.GetExtension(filePath);
            var uniqueFileName = $"{fileName}{extension}";

            var newFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", uniqueFileName);
            using (var fileStream = new FileStream(newFilePath, FileMode.Open, FileAccess.Read))
            {
                var formFile = new FormFile(fileStream, 0, fileStream.Length, Path.GetFileNameWithoutExtension(newFilePath), Path.GetFileName(newFilePath))
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "application/octet-stream"
                };

                return formFile;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error accessing file: {ex.Message}");
            throw;
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Product product, IFormFile imageFile)
    {
        if (id != product.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    var fileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
                    var extension = Path.GetExtension(imageFile.FileName);
                    var uniqueFileName = $"{fileName}_{Guid.NewGuid()}{extension}";

                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    product.ProductImage = "/images/" + uniqueFileName;
                }

                _context.Update(product);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Products.Any(e => e.Id == product.Id))
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
        return View(product);
    }

    // GET: Products/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    // POST: Products/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var product = await _context.Products.FindAsync(id);
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
