using System.ComponentModel.DataAnnotations;

namespace ShoppingCartApp.Models
{
    public class Customer
    {
        public int Id { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}
