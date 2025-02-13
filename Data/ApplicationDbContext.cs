using Microsoft.EntityFrameworkCore;
using ShoppingCartApp.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Item> Items { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<WishlistItem> WishlistItems { get; set; }
    public DbSet<PurchaseHistory> PurchaseHistories { get; set; }
    public DbSet<PurchaseItem> PurchaseItems { get; set; }
    

    public DbSet<Customer> Customers { get; set; }
}
