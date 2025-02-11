using System.ComponentModel.DataAnnotations;

namespace ShoppingCartApp.Models;

public class PurchaseItem
{
    public int Id { get; set; }
    
    [Required]
    public int ProductId { get; set; }
    public Product Product { get; set; }
    
    [Required]
    public int Quantity { get; set; }
    
    [Required]
    public decimal PriceAtPurchase { get; set; }
    
    public int PurchaseHistoryId { get; set; }
    public PurchaseHistory PurchaseHistory { get; set; }
}
