using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCartApp.Models;
using System.Threading.Tasks;

namespace ShoppingCartApp.Controllers;

public class WishlistController : Controller
{
    private readonly ApplicationDbContext _context;

    public WishlistController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> AddToWishlist(int productId)
    {
        var userId = User.Identity?.Name;
        if (string.IsNullOrEmpty(userId))
            return RedirectToAction("Login", "Account");

        var exists = await _context.WishlistItems.AnyAsync(w => w.ProductId == productId && w.UserId == userId);
        if (!exists)
        {
            var wishlistItem = new WishlistItem
            {
                ProductId = productId,
                UserId = userId
            };
            _context.WishlistItems.Add(wishlistItem);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Index", "Home");
    }
}
