using AutoMapper;
using Microsoft.AspNetCore.Identity;
using ShopManager.DataAccess.SqlServer;
using ShopManager.DataAccess.SqlServer.Entities;
using ShopManager.Domain.Dtos.CategoryDtos;
using ShopManager.Domain.Dtos.ProductDtos;
using ShopManager.Domain.Interfaces;
using ShopManager.Domain.Interfaces.Repositories;
using ShopManager.Domain.Models;

namespace ShopManager.API;

public class Seed
{
    private readonly ShopManagerDbContext _context;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly UserManager<UserEntity> _userManager;
    private readonly ITransactionsRepository _transactionsRepository;
    private readonly IMapper _mapper;
    private readonly ICategoriesService _categoriesService;
    private readonly IProductsService _productsService;

    public Seed(ShopManagerDbContext context,
        RoleManager<IdentityRole<Guid>> roleManager,
        UserManager<UserEntity> userManager,
        ITransactionsRepository transactionsRepository,
        IMapper mapper,
        ICategoriesService categoriesService,
        IProductsService productsService)
    {
        _context = context;
        _roleManager = roleManager;
        _userManager = userManager;
        _transactionsRepository = transactionsRepository;
        _mapper = mapper;
        _categoriesService = categoriesService;
        _productsService = productsService;
    }

    public async Task SeedDataContextAsync()
    {
        try
        {
            if (_context.Users.FirstOrDefault(u => u.UserName == "systemadmin") is null)
            {
                var user = User.Create(
                    "systemadmin@systemadmin.systemadmin",
                    "systemadmin",
                    "string",
                    "string",
                    "string",
                    DateOnly.FromDateTime(DateTime.Now.AddYears(-30)));

                if (user.IsFailure)
                {
                    throw new Exception();
                }

                var systemAdmin = _mapper.Map<User, UserEntity>(user.Value);


                var result = await _userManager.CreateAsync(systemAdmin, "string");

                if (!result.Succeeded)
                {
                    return;
                }

                var roleExists = await _roleManager.RoleExistsAsync(nameof(Roles.SystemAdmin));
                if (!roleExists)
                {
                    var role = new IdentityRole<Guid>()
                    {
                        Name = nameof(Roles.SystemAdmin)
                    };

                    await _roleManager.CreateAsync(role);
                }

                await _userManager.AddToRoleAsync(systemAdmin, nameof(Roles.SystemAdmin));
            }

            if (_context.Users.FirstOrDefault(u => u.UserName == "user") is null)
            {
                var user = User.Create(
                    "user@user.user",
                    "user",
                    "string",
                    "string",
                    "string",
                    DateOnly.FromDateTime(DateTime.Now.AddYears(-20)));

                if (user.IsFailure)
                {
                    throw new Exception();
                }

                var mailSystemAdmin = _mapper.Map<User, UserEntity>(user.Value);

                var result = await _userManager.CreateAsync(mailSystemAdmin, "string");

                if (!result.Succeeded)
                {
                    return;
                }

                var roleExists = await _roleManager.RoleExistsAsync(nameof(Roles.User));
                if (!roleExists)
                {
                    var role = new IdentityRole<Guid>()
                    {
                        Name = nameof(Roles.User)
                    };

                    await _roleManager.CreateAsync(role);
                }

                await _userManager.AddToRoleAsync(mailSystemAdmin, nameof(Roles.User));
            }

            await _context.SaveChangesAsync();

            if (!(await _categoriesService.GetAllAsync<CategoryDto>()).Value.Any())
            {
                var categories = new List<Category>
                {
                    Category.Create("Smartphones").Value,
                    Category.Create("Laptops").Value,
                    Category.Create("Tablets").Value,
                    Category.Create("Audio Equipment").Value,
                    Category.Create("Smart Watches").Value,
                    Category.Create("Gaming Consoles").Value,
                    Category.Create("Cameras").Value,
                    Category.Create("Accessories").Value,
                    Category.Create("TVs").Value,
                    Category.Create("Computer Components").Value,
                    Category.Create("Networking Equipment").Value,
                    Category.Create("Smart Home").Value,
                    Category.Create("Office Equipment").Value,
                    Category.Create("Home Appliances").Value,
                    Category.Create("Kitchen Appliances").Value,
                    Category.Create("Climate Control").Value,
                    Category.Create("Video Surveillance").Value,
                    Category.Create("Power Tools").Value,
                    Category.Create("Garden Equipment").Value,
                    Category.Create("Gaming Gear").Value,
                };

                foreach (var category in categories)
                {
                    var result = await _categoriesService.CreateAsync<CategoryDto>(category);
                    if (result.IsFailure)
                    {
                        throw new Exception(result.Error);
                    }
                }
            }

            if (!(await _productsService.GetAllAsync<ProductDto>()).Value.Any())
            {
                var categories = (await _categoriesService.GetAllAsync<CategoryDto>()).Value.Take(20).ToList();

                if (categories.Count < 20)
                {
                    throw new InvalidOperationException("Not enough categories. Expected 20 categories, but got: " +
                                                        categories.Count);
                }

                var products = new List<Product>();

                // Smartphones
                products.Add(Product.Create(categories[0].Id, "Samsung A23", "A23-128", 299.99m).Value);
                products.Add(Product.Create(categories[0].Id, "iPhone 13", "IP13-128", 699.99m).Value);

                // Laptops
                products.Add(Product.Create(categories[1].Id, "Acer Aspire 3", "ASP3-156", 599.99m).Value);
                products.Add(Product.Create(categories[1].Id, "HP Pavilion", "PAV-173", 749.99m).Value);

                // Tablets
                products.Add(Product.Create(categories[2].Id, "Lenovo Tab M10", "TAB-M10", 199.99m).Value);
                products.Add(Product.Create(categories[2].Id, "iPad 9", "IPAD-9", 329.99m).Value);

                // Audio
                products.Add(Product.Create(categories[3].Id, "Sony WH-CH510", "SNY-510", 49.99m).Value);
                products.Add(Product.Create(categories[3].Id, "JBL Flip 5", "JBL-FLP5", 89.99m).Value);

                // Smart Watches
                products.Add(Product.Create(categories[4].Id, "Mi Band 7", "MI-BND7", 49.99m).Value);
                products.Add(Product.Create(categories[4].Id, "Apple Watch SE", "AWS-SE", 249.99m).Value);

                // Gaming Consoles
                products.Add(Product.Create(categories[5].Id, "PS4 Slim", "PS4-SLIM", 299.99m).Value);
                products.Add(Product.Create(categories[5].Id, "Nintendo Switch", "NSW-001", 299.99m).Value);

                // Cameras
                products.Add(Product.Create(categories[6].Id, "Canon 2000D", "CAN-2000D", 449.99m).Value);
                products.Add(Product.Create(categories[6].Id, "GoPro Hero 9", "GPH-9", 349.99m).Value);

                // Accessories
                products.Add(Product.Create(categories[7].Id, "Spigen Case", "SPG-IP13", 19.99m).Value);
                products.Add(Product.Create(categories[7].Id, "Anker PowerCore", "ANK-20000", 39.99m).Value);

                // TVs
                products.Add(Product.Create(categories[8].Id, "LG 43UP7500", "LG-43UP", 399.99m).Value);
                products.Add(Product.Create(categories[8].Id, "Samsung 50TU7000", "SAM-50TU", 449.99m).Value);

                // PC Parts
                products.Add(Product.Create(categories[9].Id, "Kingston Fury 8GB", "RAM-8GB", 49.99m).Value);
                products.Add(Product.Create(categories[9].Id, "Samsung 870 EVO", "SSD-256", 59.99m).Value);

                // Network
                products.Add(Product.Create(categories[10].Id, "TP-Link AC1200", "TPL-1200", 39.99m).Value);
                products.Add(Product.Create(categories[10].Id, "D-Link Switch", "DLK-8P", 24.99m).Value);

                // Smart Home
                products.Add(Product.Create(categories[11].Id, "Yeelight LED", "YL-RGB", 19.99m).Value);
                products.Add(Product.Create(categories[11].Id, "TP-Link Plug", "TPL-P100", 15.99m).Value);

                // Office
                products.Add(Product.Create(categories[12].Id, "HP 107w", "HP-107W", 129.99m).Value);
                products.Add(Product.Create(categories[12].Id, "Canon Scan", "CAN-LiDE", 89.99m).Value);

                // Home
                products.Add(Product.Create(categories[13].Id, "Xiaomi Vacuum", "XM-VAC", 159.99m).Value);
                products.Add(Product.Create(categories[13].Id, "Xiaomi Fan 2", "XM-FAN2", 59.99m).Value);

                // Kitchen
                products.Add(Product.Create(categories[14].Id, "Bosch Microwave", "BSH-MW", 119.99m).Value);
                products.Add(Product.Create(categories[14].Id, "Philips Blender", "PH-BLD", 49.99m).Value);

                // Climate
                products.Add(Product.Create(categories[15].Id, "LG Air Cond", "LG-AC", 449.99m).Value);
                products.Add(Product.Create(categories[15].Id, "Xiaomi Air 3", "XM-AIR3", 129.99m).Value);

                // Security
                products.Add(Product.Create(categories[16].Id, "Hikvision Cam", "HIK-2MP", 89.99m).Value);
                products.Add(Product.Create(categories[16].Id, "Ring Door", "RNG-V3", 79.99m).Value);

                // Tools
                products.Add(Product.Create(categories[17].Id, "Bosch Drill", "BSH-DRL", 89.99m).Value);
                products.Add(Product.Create(categories[17].Id, "Hammer Set", "HMR-110", 34.99m).Value);

                // Garden
                products.Add(Product.Create(categories[18].Id, "AL-KO Mower", "ALK-MWR", 249.99m).Value);
                products.Add(Product.Create(categories[18].Id, "Garden Kit", "GRD-SET", 45.99m).Value);

                // Gaming Gear
                products.Add(Product.Create(categories[19].Id, "Logitech G102", "LOG-G102", 29.99m).Value);
                products.Add(Product.Create(categories[19].Id, "RedDragon K552", "RD-K552", 44.99m).Value);

                foreach (var product in products)
                {
                    await _productsService.CreateAsync<ProductDto>(product);
                }
            }
            
            
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}