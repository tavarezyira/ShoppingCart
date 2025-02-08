using System.Linq;
using Microsoft.AspNetCore.Mvc;

public class CartCountViewComponent : ViewComponent
{
    private readonly CartService _cartService;

    public CartCountViewComponent(CartService cartService)
    {
        _cartService = cartService;
    }

    public IViewComponentResult Invoke()
    {
        var userId = User.Identity.Name; // Or however you identify the user
        string count = _cartService.GetCartItemCount();
       // string viewCount = count as String;
        return View("Default", count);
    }
}

public class CartService
{
    private readonly ApplicationDbContext _context;

    public CartService(ApplicationDbContext context)
    {
        _context = context;
    }

    public string GetCartItemCount()
    {
        int count = _context.Items.Count();
        string countString = count.ToString();
        return countString;
    }
}