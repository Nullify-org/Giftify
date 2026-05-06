using Giftify.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Giftify.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<Occasion> Occasions { get; set; }
    public DbSet<OccasionProduct> OccasionProducts { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderITems { get; set; }
    public DbSet<Payment> Payments { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Relations 

        // Occasion Products
        builder.Entity<OccasionProduct>()
            .HasOne(op => op.Product)
            .WithMany(p => p.OccasionProducts)
            .HasForeignKey(op => op.ProductId);

        builder.Entity<OccasionProduct>()
            .HasOne(op => op.Occasion)
            .WithMany(p => p.OccasionProducts)
            .HasForeignKey(op => op.OccasionId);



        // Data seeding
    }
}
