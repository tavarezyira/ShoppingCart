using System.ComponentModel.DataAnnotations;
namespace ShoppingCartApp.Models;

public class Product
{
    public int Id { get; set; }

    [Required]
    public string ProductName { get; set; }

    [Required]
    [Range(0.01, 10000.00)]
    public decimal Price { get; set; }
    
    public string ProductImage { get; set; }

    // Nueva propiedad para la descripción del chocolate
    [Required]
    public string Description { get; set; }
}
