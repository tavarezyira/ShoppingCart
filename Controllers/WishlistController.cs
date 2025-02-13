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
    public class WishlistController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WishlistController(ApplicationDbContext context)
        {
            _context = context;
        }

        // --- Display Wishlist ---
        public async Task<IActionResult> Index()
        {
            var userEmail = User.Identity?.Name;

            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("CustomerLogin", "Account");
            }

            var wishlistItems = await _context.WishlistItems
                .AsNoTracking()
                .Include(w => w.Product)
                .Where(w => w.UserEmail == userEmail)
                .ToListAsync();

            return View(wishlistItems);
        }

        // --- Add Product to Wishlist ---
        public async Task<IActionResult> AddToWishlist(int productId)
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("CustomerLogin", "Account");
            }

            var userEmail = User.Identity?.Name;

            // Prevent duplicate entries
            if (!await _context.WishlistItems.AnyAsync(w => w.ProductId == productId && w.UserEmail == userEmail))
            {
                var wishlistItem = new WishlistItem
                {
                    ProductId = productId,
                    UserEmail = userEmail
                };

                _context.WishlistItems.Add(wishlistItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        // --- Remove Product from Wishlist ---
        public async Task<IActionResult> RemoveFromWishlist(int id)
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("CustomerLogin", "Account");
            }

            var userEmail = User.Identity?.Name;
            var wishlistItem = await _context.WishlistItems.FindAsync(id);

            if (wishlistItem == null || wishlistItem.UserEmail != userEmail)
            {
                return NotFound(); // Prevents deleting another user's wishlist item
            }

            _context.WishlistItems.Remove(wishlistItem);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
