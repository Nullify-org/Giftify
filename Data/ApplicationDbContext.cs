using Giftify.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Giftify.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{


    // public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){ }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
      : base(options)
    {
    }
    public DbSet<Category> Categories { get; set; }
    public DbSet<OccasionCategory> OccasionCategories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<Occasion> Occasions { get; set; }
    public DbSet<OccasionProduct> OccasionProducts { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderITems { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }



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
        
        // Occasion Categories

        builder.Entity<OccasionCategory>()
              .HasOne(oc => oc.Occasion)
              .WithMany(o => o.OccasionCategories)
              .HasForeignKey(oc => oc.OccasionId);
        builder.Entity<OccasionCategory>()
              .HasOne(oc => oc.Category)
              .WithMany(c => c.OccasionCategories)
              .HasForeignKey(oc => oc.CategoryId);
        // 
        // One User → One Cart
        builder.Entity<Cart>()
            .HasOne(c => c.User)
            .WithOne(u => u.Cart)
            .HasForeignKey<Cart>(c => c.ApplicationUserId);

        // One Order → One Payment
        builder.Entity<Order>()
            .HasOne(o => o.Payment)
            .WithOne(p => p.Order)
            .HasForeignKey<Payment>(p => p.OrderId);


        // Data seeding
    }
}
