using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ShoppingCartApp.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCartApp.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class ItemController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ItemController(ApplicationDbContext context)
        {
            _context = context;
        }

        // --- Display Cart ---
        public async Task<IActionResult> Index()
        {
            var userEmail = User.Identity?.Name;
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("CustomerLogin", "Account");
            }

            var cartItems = await _context.Items
                .Include(i => i.Product)
                .Where(i => i.UserEmail == userEmail)
                .ToListAsync();

            return View(cartItems);
        }

        // --- Select Quantity Before Adding to Cart ---
        public async Task<IActionResult> Create(int productId)
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("CustomerLogin", "Account");
            }

            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            var cartItem = new Item
            {
                ProductId = productId,
                Name = product.ProductName,
                Price = product.Price,
                Quantity = 1 // Default value
            };

            return View(cartItem); // Redirects to Create.cshtml where user can set quantity
        }

        // --- Add Item to Cart ---
        [HttpPost]
        public async Task<IActionResult> Create(Item item)
        {
            var userEmail = User.Identity?.Name;
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("CustomerLogin", "Account");
            }

            var product = await _context.Products.FindAsync(item.ProductId);
            if (product == null)
            {
                return NotFound();
            }

            var existingCartItem = await _context.Items
                .FirstOrDefaultAsync(i => i.ProductId == item.ProductId && i.UserEmail == userEmail);

            if (existingCartItem != null)
            {
                existingCartItem.Quantity += item.Quantity;
            }
            else
            {
                item.UserEmail = userEmail;
                _context.Items.Add(item);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // --- Edit Item in Cart ---
        public async Task<IActionResult> Edit(int id)
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("CustomerLogin", "Account");
            }

            var cartItem = await _context.Items.FindAsync(id);
            if (cartItem == null)
            {
                return NotFound();
            }
            return View(cartItem);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Item item)
        {
            if (id != item.Id)
            {
                return NotFound();
            }

            var cartItem = await _context.Items.FindAsync(id);
            if (cartItem == null)
            {
                return NotFound();
            }

            cartItem.Quantity = item.Quantity;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // --- Delete Item from Cart ---
        public async Task<IActionResult> Delete(int id)
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("CustomerLogin", "Account");
            }

            var cartItem = await _context.Items.FindAsync(id);
            if (cartItem == null)
            {
                return NotFound();
            }
            return View(cartItem);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cartItem = await _context.Items.FindAsync(id);
            if (cartItem != null)
            {
                _context.Items.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}
