using Giftify.Models;
using Microsoft.AspNetCore.Identity;

namespace Giftify.Data.Seeder
{
    public class ApplicationDbSeeder
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        private async Task SeedRolesAsync()
        {
            string[] roles = { "Admin", "User" };

            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        private async Task SeedUsersAsync()
        {
            if (await _userManager.FindByEmailAsync("admin@giftify.com") == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = "admin@giftify.com",
                    Email = "admin@giftify.com",
                    FullName = "Admin User",
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(admin, "Admin@123456");

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(admin, "Admin");
                }
            }
        }

        private async Task SeedCategoriesAsync()
        {
            if (!_context.Categories.Any())
            {
                var categories = new List<Category>
        {
            new Category { Name = "Electronics", Description = "Electronic gifts" },
            new Category { Name = "Toys", Description = "Kids toys" },
            new Category { Name = "Fashion", Description = "Clothing & accessories" }
        };

                await _context.Categories.AddRangeAsync(categories);
                await _context.SaveChangesAsync();
            }
        }

        private async Task SeedOccasionsAsync()
        {
            if (!_context.Occasions.Any())
            {
                var occasions = new List<Occasion>
        {
            new Occasion { Name = "Birthday", Description = "Birthday gifts" },
            new Occasion { Name = "Wedding", Description = "Wedding gifts" },
            new Occasion { Name = "Christmas", Description = "Christmas gifts" }
        };

                await _context.Occasions.AddRangeAsync(occasions);
                await _context.SaveChangesAsync();
            }
        }

        /*    private async Task SeedProductsAsync()
            {
                if (!_context.Products.Any())
                {
                    var categoryId = _context.Categories.First().Id;

                    var products = new List<Product>
            {
                new Product
                {
                    Name = "Smart Watch",
                    Description = "Elegant smart watch",
                    Price = 299.99m,
                    Stock = 50,
                    CategoryId = categoryId
                },
                new Product
                {
                    Name = "Perfume Set",
                    Description = "Luxury perfume collection",
                    Price = 150.00m,
                    Stock = 30,
                    CategoryId = categoryId
                }
            };

                    await _context.Products.AddRangeAsync(products);
                    await _context.SaveChangesAsync();
                }
            }*/

        private async Task SeedProductsAsync()
        {
            if (!_context.Products.Any())
            {
                var electronics = _context.Categories.First(c => c.Name == "Electronics");
                var toys = _context.Categories.First(c => c.Name == "Toys");
                var fashion = _context.Categories.First(c => c.Name == "Fashion");

                var products = new List<Product>
        {
            // Electronics
            new Product { Name = "Smart Watch", Description = "Elegant smart watch with health tracking", Price = 299.99m, Stock = 50, CategoryId = electronics.Id },
            new Product { Name = "Wireless Earbuds", Description = "Premium noise cancelling earbuds", Price = 199.99m, Stock = 40, CategoryId = electronics.Id },
            new Product { Name = "Portable Speaker", Description = "Waterproof bluetooth speaker", Price = 149.99m, Stock = 35, CategoryId = electronics.Id },
            new Product { Name = "Smart Lamp", Description = "Color changing smart LED lamp", Price = 89.99m, Stock = 60, CategoryId = electronics.Id },
            new Product { Name = "Wireless Charger", Description = "Fast wireless charging pad", Price = 49.99m, Stock = 80, CategoryId = electronics.Id },

            // Toys
            new Product { Name = "LEGO Set", Description = "Creative building blocks set", Price = 79.99m, Stock = 45, CategoryId = toys.Id },
            new Product { Name = "Remote Control Car", Description = "High speed RC car", Price = 59.99m, Stock = 30, CategoryId = toys.Id },
            new Product { Name = "Puzzle 1000pcs", Description = "Challenging landscape puzzle", Price = 29.99m, Stock = 55, CategoryId = toys.Id },
            new Product { Name = "Board Game", Description = "Fun family board game", Price = 39.99m, Stock = 25, CategoryId = toys.Id },
            new Product { Name = "Stuffed Animal", Description = "Super soft teddy bear", Price = 24.99m, Stock = 70, CategoryId = toys.Id },

            // Fashion
            new Product { Name = "Perfume Set", Description = "Luxury perfume collection", Price = 150.00m, Stock = 30, CategoryId = fashion.Id },
            new Product { Name = "Silk Scarf", Description = "Premium silk scarf with floral print", Price = 89.99m, Stock = 40, CategoryId = fashion.Id },
            new Product { Name = "Leather Wallet", Description = "Genuine leather slim wallet", Price = 69.99m, Stock = 50, CategoryId = fashion.Id },
            new Product { Name = "Sunglasses", Description = "UV protection stylish sunglasses", Price = 119.99m, Stock = 35, CategoryId = fashion.Id },
            new Product { Name = "Watch Box", Description = "Elegant wooden watch display box", Price = 59.99m, Stock = 20, CategoryId = fashion.Id },
        };

                await _context.Products.AddRangeAsync(products);
                await _context.SaveChangesAsync();
            }
        }

   /*     private async Task SeedProductImagesAsync()
        {
            if (!_context.ProductImages.Any())
            {
                var products = _context.Products.ToList();

                if (!products.Any()) return;

                var images = new List<ProductImage>();

                foreach (var product in products)
                {
                    // الصورة الأساسية
                    images.Add(new ProductImage
                    {
                        ProductId = product.Id,
                        ImageUrl = $"https://placehold.co/600x400?text={product.Name.Replace(" ", "+")}",
                        IsPrimary = true,
                        DisplayOrder = 1
                    });

                    // صورة ثانية
                    images.Add(new ProductImage
                    {
                        ProductId = product.Id,
                        ImageUrl = $"https://placehold.co/600x400?text={product.Name.Replace(" ", "+")}+2",
                        IsPrimary = false,
                        DisplayOrder = 2
                    });

                    // صورة ثالثة
                    images.Add(new ProductImage
                    {
                        ProductId = product.Id,
                        ImageUrl = $"https://placehold.co/600x400?text={product.Name.Replace(" ", "+")}+3",
                        IsPrimary = false,
                        DisplayOrder = 3
                    });
                }

                await _context.ProductImages.AddRangeAsync(images);
                await _context.SaveChangesAsync();
            }
        }*/

        private async Task SeedProductImagesAsync()
{
    if (!_context.ProductImages.Any())
    {
        var products = _context.Products.ToList();
        if (!products.Any()) return;

        var imageUrls = new Dictionary<string, List<string>>
        {
            ["Smart Watch"] = new List<string>
            {
                "https://images.unsplash.com/photo-1523275335684-37898b6baf30?w=600",
                "https://images.unsplash.com/photo-1546868871-7041f2a55e12?w=600",
                "https://images.unsplash.com/photo-1585386959984-a4155224a1ad?w=600"
            },
            ["Wireless Earbuds"] = new List<string>
            {
                "https://images.unsplash.com/photo-1590658268037-6bf12165a8df?w=600",
                "https://images.unsplash.com/photo-1606220588913-b3aacb4d2f46?w=600",
                "https://images.unsplash.com/photo-1608156639585-b3a032ef9689?w=600"
            },
            ["Portable Speaker"] = new List<string>
            {
                "https://images.unsplash.com/photo-1608043152269-423dbba4e7e1?w=600",
                "https://images.unsplash.com/photo-1545454675-3531b543be5d?w=600",
                "https://images.unsplash.com/photo-1507646227500-4d389b0012be?w=600"
            },
            ["Smart Lamp"] = new List<string>
            {
                "https://images.unsplash.com/photo-1534189285-0e83d9d4bd7d?w=600",
                "https://images.unsplash.com/photo-1565814636199-ae8133055c1c?w=600",
                "https://images.unsplash.com/photo-1540932239986-30128078f3c5?w=600"
            },
            ["Wireless Charger"] = new List<string>
            {
                "https://images.unsplash.com/photo-1586953208448-b95a79798f07?w=600",
                "https://images.unsplash.com/photo-1622445275463-afa2ab738c34?w=600",
                "https://images.unsplash.com/photo-1609091839311-d5365f9ff1c5?w=600"
            },
            ["LEGO Set"] = new List<string>
            {
                "https://images.unsplash.com/photo-1587654780291-39c9404d746b?w=600",
                "https://images.unsplash.com/photo-1596461404969-9ae70f2830c1?w=600",
                "https://images.unsplash.com/photo-1560961911-ba7ef651a56c?w=600"
            },
            ["Remote Control Car"] = new List<string>
            {
                "https://images.unsplash.com/photo-1594787318286-3d835c1d207f?w=600",
                "https://images.unsplash.com/photo-1581235720704-06d3acfcb36f?w=600",
                "https://images.unsplash.com/photo-1568605117036-5fe5e7bab0b7?w=600"
            },
            ["Puzzle 1000pcs"] = new List<string>
            {
                "https://images.unsplash.com/photo-1590496793929-36417d3117de?w=600",
                "https://images.unsplash.com/photo-1611996575749-79a3a250f948?w=600",
                "https://images.unsplash.com/photo-1585366119957-e9730b6d0f60?w=600"
            },
            ["Board Game"] = new List<string>
            {
                "https://images.unsplash.com/photo-1610890716171-6b1bb98ffd09?w=600",
                "https://images.unsplash.com/photo-1606503153255-59d8b8b82176?w=600",
                "https://images.unsplash.com/photo-1632501641765-e568d28b0015?w=600"
            },
            ["Stuffed Animal"] = new List<string>
            {
                "https://images.unsplash.com/photo-1559715541-5daf8a0296d0?w=600",
                "https://images.unsplash.com/photo-1551356219-b819c98ac50f?w=600",
                "https://images.unsplash.com/photo-1562040506-a9b8e4debae1?w=600"
            },
            ["Perfume Set"] = new List<string>
            {
                "https://images.unsplash.com/photo-1541643600914-78b084683702?w=600",
                "https://images.unsplash.com/photo-1592945403244-b3fbafd7f539?w=600",
                "https://images.unsplash.com/photo-1588514912908-b6bb6e6b3c1c?w=600"
            },
            ["Silk Scarf"] = new List<string>
            {
                "https://images.unsplash.com/photo-1601924994987-69e26d50dc26?w=600",
                "https://images.unsplash.com/photo-1520903920243-00d872a2d1c9?w=600",
                "https://images.unsplash.com/photo-1606760227091-3dd870d97f1d?w=600"
            },
            ["Leather Wallet"] = new List<string>
            {
                "https://images.unsplash.com/photo-1627123424574-724758594913?w=600",
                "https://images.unsplash.com/photo-1517254456976-da9f220e6b7e?w=600",
                "https://images.unsplash.com/photo-1556742049-0cfed4f6a45d?w=600"
            },
            ["Sunglasses"] = new List<string>
            {
                "https://images.unsplash.com/photo-1572635196237-14b3f281503f?w=600",
                "https://images.unsplash.com/photo-1511499767150-a48a237f0083?w=600",
                "https://images.unsplash.com/photo-1508296695146-257a814070b4?w=600"
            },
            ["Watch Box"] = new List<string>
            {
                "https://images.unsplash.com/photo-1548169874-53e85f753f1e?w=600",
                "https://images.unsplash.com/photo-1594534475808-b18acf6f1b5d?w=600",
                "https://images.unsplash.com/photo-1607462109225-6b64ae2dd3cb?w=600"
            }
        };

        var images = new List<ProductImage>();

        foreach (var product in products)
        {
            if (!imageUrls.ContainsKey(product.Name)) continue;

            var urls = imageUrls[product.Name];
            for (int i = 0; i < urls.Count; i++)
            {
                images.Add(new ProductImage
                {
                    ProductId = product.Id,
                    ImageUrl = urls[i],
                    IsPrimary = i == 0,
                    DisplayOrder = i + 1
                });
            }
        }

        await _context.ProductImages.AddRangeAsync(images);
        await _context.SaveChangesAsync();
    }
}
        public ApplicationDbSeeder(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            await SeedRolesAsync();
            await SeedUsersAsync();
            await SeedCategoriesAsync();
            await SeedOccasionsAsync();
            await SeedProductsAsync();
            await SeedProductImagesAsync();
        }



    }
}