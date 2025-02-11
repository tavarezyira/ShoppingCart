using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShoppingCartApp.Models;

public class PurchaseHistory
{
    public int Id { get; set; }
    
    [Required]
    public string UserId { get; set; }
    
    public DateTime PurchaseDate { get; set; }
    
    public List<PurchaseItem> PurchaseItems { get; set; }
}
