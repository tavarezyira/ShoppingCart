using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
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
}