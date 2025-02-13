using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCartApp.Models;
using System.Threading.Tasks;
using System.Linq;

namespace ShoppingCartApp.Controllers
{
    public class PurchaseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PurchaseController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Show customer's history
        public async Task<IActionResult> History()
        {
            var userId = User.Identity?.Name;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var history = await _context.PurchaseHistories
                .Include(ph => ph.PurchaseItems)
                    .ThenInclude(pi => pi.Product)
                .Where(ph => ph.UserId == userId)
                .OrderByDescending(ph => ph.PurchaseDate)
                .ToListAsync();

            return View(history);
        }
    }
}
