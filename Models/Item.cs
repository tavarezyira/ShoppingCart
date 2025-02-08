using System.ComponentModel.DataAnnotations;
namespace ShoppingCartApp.Models;

public class Item 
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    [Range(1, 100)]
    public int Quantity { get; set; }

    [Required]
    [Range(0.01, 10000.00)]
    public decimal Price { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
}