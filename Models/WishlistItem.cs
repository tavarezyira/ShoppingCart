using System.ComponentModel.DataAnnotations;
namespace ShoppingCartApp.Models;

public class WishlistItem
{
    public int Id { get; set; }
    public string UserId { get; set; }

    [Required]
    public int ProductId { get; set; }
    public Product Product { get; set; }
}
