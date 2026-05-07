using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SirTebz_Tech.Models.Entities;

namespace SirTebz_Tech.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var config = serviceProvider.GetRequiredService<IConfiguration>();

        await context.Database.MigrateAsync();

        // Seed Roles
        if (!await roleManager.RoleExistsAsync("Admin"))
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        if (!await roleManager.RoleExistsAsync("User"))
            await roleManager.CreateAsync(new IdentityRole("User"));

        // Seed Admin User
        var adminEmail = config["AdminSettings:DefaultAdminEmail"] ?? "admin@sirtebz.tech";
        var adminPassword = config["AdminSettings:DefaultAdminPassword"] ?? "Admin@123456";

        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var admin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FirstName = "Sir",
                LastName = "Tebz",
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(admin, adminPassword);
            if (result.Succeeded)
                await userManager.AddToRoleAsync(admin, "Admin");
        }

        // Seed Categories
        if (!await context.Categories.AnyAsync())
        {
            context.Categories.AddRange(new List<Category>
            {
                new() { Name = "Laptops", Description = "Portable computing powerhouses", IconClass = "bi bi-laptop" },
                new() { Name = "Desktop PCs", Description = "Full-power desktop computers", IconClass = "bi bi-pc-display" },
                new() { Name = "Graphics Cards", Description = "GPU for gaming and creativity", IconClass = "bi bi-gpu-card" },
                new() { Name = "Monitors", Description = "High resolution displays", IconClass = "bi bi-display" },
                new() { Name = "Accessories", Description = "Keyboards, mice, and more", IconClass = "bi bi-mouse" },
                new() { Name = "Storage", Description = "SSDs, HDDs and storage solutions", IconClass = "bi bi-device-hdd" },
                new() { Name = "Networking", Description = "Routers, switches, and cables", IconClass = "bi bi-router" },
                new() { Name = "Headphones", Description = "Audio gear for professionals", IconClass = "bi bi-headphones" }
            });
            await context.SaveChangesAsync();
        }

        // Seed Brands
        if (!await context.Brands.AnyAsync())
        {
            context.Brands.AddRange(new List<Brand>
            {
                new() { Name = "Dell" },
                new() { Name = "HP" },
                new() { Name = "ASUS" },
                new() { Name = "Lenovo" },
                new() { Name = "Apple" },
                new() { Name = "MSI" },
                new() { Name = "Samsung" },
                new() { Name = "Logitech" }
            });
            await context.SaveChangesAsync();
        }

        // Seed Products
        if (!await context.Products.AnyAsync())
        {
            var laptopCat = await context.Categories.FirstAsync(c => c.Name == "Laptops");
            var gpuCat = await context.Categories.FirstAsync(c => c.Name == "Graphics Cards");
            var monitorCat = await context.Categories.FirstAsync(c => c.Name == "Monitors");
            var accessoryCat = await context.Categories.FirstAsync(c => c.Name == "Accessories");
            var storageCat = await context.Categories.FirstAsync(c => c.Name == "Storage");

            var dell = await context.Brands.FirstAsync(b => b.Name == "Dell");
            var hp = await context.Brands.FirstAsync(b => b.Name == "HP");
            var asus = await context.Brands.FirstAsync(b => b.Name == "ASUS");
            var lenovo = await context.Brands.FirstAsync(b => b.Name == "Lenovo");
            var msi = await context.Brands.FirstAsync(b => b.Name == "MSI");
            var samsung = await context.Brands.FirstAsync(b => b.Name == "Samsung");
            var logitech = await context.Brands.FirstAsync(b => b.Name == "Logitech");

            var products = new List<Product>
            {
                new()
                {
                    Name = "Dell XPS 15 (2024)",
                    Description = "The Dell XPS 15 is a premium laptop designed for creative professionals and power users. With its stunning OLED display, powerful Intel Core i9 processor, and NVIDIA RTX graphics, it handles everything from 4K video editing to complex 3D rendering with ease.",
                    Price = 32999.00m,
                    OriginalPrice = 36999.00m,
                    CategoryId = laptopCat.Id,
                    BrandId = dell.Id,
                    ImageUrl = "/images/products/dell-xps15.jpg",
                    Stock = 12,
                    IsFeatured = true
                },
                new()
                {
                    Name = "HP Spectre x360 16",
                    Description = "The HP Spectre x360 is the ultimate convertible laptop, combining stunning design with powerful performance. Its 2-in-1 form factor lets you switch between laptop and tablet modes seamlessly.",
                    Price = 27499.00m,
                    OriginalPrice = 29999.00m,
                    CategoryId = laptopCat.Id,
                    BrandId = hp.Id,
                    ImageUrl = "/images/products/hp-spectre.jpg",
                    Stock = 8,
                    IsFeatured = true
                },
                new()
                {
                    Name = "ASUS ROG Strix G16",
                    Description = "Built for elite gaming, the ASUS ROG Strix G16 features a blazing-fast 240Hz display, RGB keyboard, and top-tier AMD Ryzen 9 processor with NVIDIA GeForce RTX 4080.",
                    Price = 39999.00m,
                    CategoryId = laptopCat.Id,
                    BrandId = asus.Id,
                    ImageUrl = "/images/products/asus-rog.jpg",
                    Stock = 5,
                    IsFeatured = true
                },
                new()
                {
                    Name = "Lenovo ThinkPad X1 Carbon",
                    Description = "The world's lightest 14-inch business laptop. The ThinkPad X1 Carbon Gen 12 is engineered for professionals who demand the best in performance, security, and reliability.",
                    Price = 24999.00m,
                    OriginalPrice = 27999.00m,
                    CategoryId = laptopCat.Id,
                    BrandId = lenovo.Id,
                    ImageUrl = "/images/products/lenovo-x1.jpg",
                    Stock = 15,
                    IsFeatured = false
                },
                new()
                {
                    Name = "MSI GeForce RTX 4080 Super",
                    Description = "Experience next-level gaming with the MSI GeForce RTX 4080 Super. Ada Lovelace architecture, 16GB GDDR6X, and DLSS 3 deliver outstanding 4K gaming performance.",
                    Price = 21999.00m,
                    OriginalPrice = 24999.00m,
                    CategoryId = gpuCat.Id,
                    BrandId = msi.Id,
                    ImageUrl = "/images/products/rtx4080.jpg",
                    Stock = 7,
                    IsFeatured = true
                },
                new()
                {
                    Name = "Samsung Odyssey G9 49\"",
                    Description = "The Samsung Odyssey G9 is a super ultrawide curved gaming monitor with a 240Hz refresh rate, 1ms response time, and QLED technology for stunning colors.",
                    Price = 18999.00m,
                    CategoryId = monitorCat.Id,
                    BrandId = samsung.Id,
                    ImageUrl = "/images/products/samsung-g9.jpg",
                    Stock = 4,
                    IsFeatured = true
                },
                new()
                {
                    Name = "Logitech MX Master 3S",
                    Description = "The Logitech MX Master 3S is the ultimate productivity mouse. 8K DPI sensor, MagSpeed scroll wheel, and ergonomic design for all-day comfort.",
                    Price = 1899.00m,
                    CategoryId = accessoryCat.Id,
                    BrandId = logitech.Id,
                    ImageUrl = "/images/products/mx-master.jpg",
                    Stock = 30,
                    IsFeatured = false
                },
                new()
                {
                    Name = "Samsung 990 Pro 2TB NVMe",
                    Description = "Blazing-fast PCIe 4.0 NVMe SSD with sequential read speeds up to 7,450 MB/s. Ideal for gaming, creative work, and professional applications.",
                    Price = 2499.00m,
                    OriginalPrice = 2999.00m,
                    CategoryId = storageCat.Id,
                    BrandId = samsung.Id,
                    ImageUrl = "/images/products/samsung-990pro.jpg",
                    Stock = 25,
                    IsFeatured = false
                }
            };

            context.Products.AddRange(products);
            await context.SaveChangesAsync();

            // Seed Specifications
            var savedProducts = await context.Products.ToListAsync();

            var specs = new List<ProductSpecification>
            {
                // Dell XPS 15
                new() { ProductId = savedProducts[0].Id, SpecKey = "CPU", SpecValue = "Intel Core i9-14900HX", DisplayOrder = 1 },
                new() { ProductId = savedProducts[0].Id, SpecKey = "RAM", SpecValue = "32GB DDR5", DisplayOrder = 2 },
                new() { ProductId = savedProducts[0].Id, SpecKey = "Storage", SpecValue = "1TB NVMe SSD", DisplayOrder = 3 },
                new() { ProductId = savedProducts[0].Id, SpecKey = "GPU", SpecValue = "NVIDIA GeForce RTX 4070", DisplayOrder = 4 },
                new() { ProductId = savedProducts[0].Id, SpecKey = "Display", SpecValue = "15.6\" OLED 3.5K 120Hz", DisplayOrder = 5 },
                new() { ProductId = savedProducts[0].Id, SpecKey = "Battery", SpecValue = "86Wh", DisplayOrder = 6 },
                new() { ProductId = savedProducts[0].Id, SpecKey = "Weight", SpecValue = "1.86 kg", DisplayOrder = 7 },
                new() { ProductId = savedProducts[0].Id, SpecKey = "OS", SpecValue = "Windows 11 Pro", DisplayOrder = 8 },

                // HP Spectre x360
                new() { ProductId = savedProducts[1].Id, SpecKey = "CPU", SpecValue = "Intel Core Ultra 7 165H", DisplayOrder = 1 },
                new() { ProductId = savedProducts[1].Id, SpecKey = "RAM", SpecValue = "16GB LPDDR5", DisplayOrder = 2 },
                new() { ProductId = savedProducts[1].Id, SpecKey = "Storage", SpecValue = "1TB NVMe SSD", DisplayOrder = 3 },
                new() { ProductId = savedProducts[1].Id, SpecKey = "GPU", SpecValue = "Intel Arc Graphics", DisplayOrder = 4 },
                new() { ProductId = savedProducts[1].Id, SpecKey = "Display", SpecValue = "16\" OLED 2.8K Touch 120Hz", DisplayOrder = 5 },
                new() { ProductId = savedProducts[1].Id, SpecKey = "Battery", SpecValue = "83Wh", DisplayOrder = 6 },
                new() { ProductId = savedProducts[1].Id, SpecKey = "Weight", SpecValue = "2.07 kg", DisplayOrder = 7 },
                new() { ProductId = savedProducts[1].Id, SpecKey = "OS", SpecValue = "Windows 11 Home", DisplayOrder = 8 },

                // ASUS ROG Strix
                new() { ProductId = savedProducts[2].Id, SpecKey = "CPU", SpecValue = "AMD Ryzen 9 7945HX", DisplayOrder = 1 },
                new() { ProductId = savedProducts[2].Id, SpecKey = "RAM", SpecValue = "32GB DDR5 4800MHz", DisplayOrder = 2 },
                new() { ProductId = savedProducts[2].Id, SpecKey = "Storage", SpecValue = "2TB NVMe SSD", DisplayOrder = 3 },
                new() { ProductId = savedProducts[2].Id, SpecKey = "GPU", SpecValue = "NVIDIA GeForce RTX 4080", DisplayOrder = 4 },
                new() { ProductId = savedProducts[2].Id, SpecKey = "Display", SpecValue = "16\" QHD+ 240Hz", DisplayOrder = 5 },
                new() { ProductId = savedProducts[2].Id, SpecKey = "Battery", SpecValue = "90Wh", DisplayOrder = 6 },
                new() { ProductId = savedProducts[2].Id, SpecKey = "Weight", SpecValue = "2.5 kg", DisplayOrder = 7 },
                new() { ProductId = savedProducts[2].Id, SpecKey = "OS", SpecValue = "Windows 11 Home", DisplayOrder = 8 },

                // Lenovo ThinkPad
                new() { ProductId = savedProducts[3].Id, SpecKey = "CPU", SpecValue = "Intel Core Ultra 5 125U", DisplayOrder = 1 },
                new() { ProductId = savedProducts[3].Id, SpecKey = "RAM", SpecValue = "16GB LPDDR5", DisplayOrder = 2 },
                new() { ProductId = savedProducts[3].Id, SpecKey = "Storage", SpecValue = "512GB NVMe SSD", DisplayOrder = 3 },
                new() { ProductId = savedProducts[3].Id, SpecKey = "GPU", SpecValue = "Intel Integrated Graphics", DisplayOrder = 4 },
                new() { ProductId = savedProducts[3].Id, SpecKey = "Display", SpecValue = "14\" IPS 2.8K 60Hz", DisplayOrder = 5 },
                new() { ProductId = savedProducts[3].Id, SpecKey = "Battery", SpecValue = "57Wh", DisplayOrder = 6 },
                new() { ProductId = savedProducts[3].Id, SpecKey = "Weight", SpecValue = "1.12 kg", DisplayOrder = 7 },
                new() { ProductId = savedProducts[3].Id, SpecKey = "OS", SpecValue = "Windows 11 Pro", DisplayOrder = 8 },

                // RTX 4080
                new() { ProductId = savedProducts[4].Id, SpecKey = "Architecture", SpecValue = "Ada Lovelace", DisplayOrder = 1 },
                new() { ProductId = savedProducts[4].Id, SpecKey = "VRAM", SpecValue = "16GB GDDR6X", DisplayOrder = 2 },
                new() { ProductId = savedProducts[4].Id, SpecKey = "CUDA Cores", SpecValue = "10240", DisplayOrder = 3 },
                new() { ProductId = savedProducts[4].Id, SpecKey = "Boost Clock", SpecValue = "2610 MHz", DisplayOrder = 4 },
                new() { ProductId = savedProducts[4].Id, SpecKey = "Memory Bus", SpecValue = "256-bit", DisplayOrder = 5 },
                new() { ProductId = savedProducts[4].Id, SpecKey = "TDP", SpecValue = "320W", DisplayOrder = 6 },
                new() { ProductId = savedProducts[4].Id, SpecKey = "Interface", SpecValue = "PCIe 4.0 x16", DisplayOrder = 7 },

                // Samsung G9
                new() { ProductId = savedProducts[5].Id, SpecKey = "Panel Type", SpecValue = "VA QLED", DisplayOrder = 1 },
                new() { ProductId = savedProducts[5].Id, SpecKey = "Resolution", SpecValue = "5120x1440 (DQHD)", DisplayOrder = 2 },
                new() { ProductId = savedProducts[5].Id, SpecKey = "Refresh Rate", SpecValue = "240Hz", DisplayOrder = 3 },
                new() { ProductId = savedProducts[5].Id, SpecKey = "Response Time", SpecValue = "1ms (GtG)", DisplayOrder = 4 },
                new() { ProductId = savedProducts[5].Id, SpecKey = "Curvature", SpecValue = "1000R", DisplayOrder = 5 },
                new() { ProductId = savedProducts[5].Id, SpecKey = "HDR", SpecValue = "DisplayHDR 1000", DisplayOrder = 6 },
                new() { ProductId = savedProducts[5].Id, SpecKey = "Ports", SpecValue = "2x HDMI 2.1, 1x DP 1.4", DisplayOrder = 7 },
            };

            context.ProductSpecifications.AddRange(specs);
            await context.SaveChangesAsync();
        }
    }
}