using System.ComponentModel.DataAnnotations;

namespace ShoppingCartApp.Models;

public class WishlistItem
{
    public int Id { get; set; }

    [Required]
    public int ProductId { get; set; }
    public Product Product { get; set; }

    [Required]
    public string UserEmail { get; set; }
}
