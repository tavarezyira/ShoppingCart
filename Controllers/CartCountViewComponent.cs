using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCartApp.Models;
using System.Linq;
using System.Threading.Tasks;

public class CartCountViewComponent : ViewComponent
{
    private readonly ApplicationDbContext _context;

    public CartCountViewComponent(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var userEmail = User.Identity?.Name;
        int count = await _context.Items
            .Where(i => i.UserEmail == userEmail)
            .SumAsync(i => i.Quantity);

        return View("Default", count.ToString());
    }
}
