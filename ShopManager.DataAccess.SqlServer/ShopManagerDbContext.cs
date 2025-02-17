using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShopManager.DataAccess.SqlServer.Entities;

namespace ShopManager.DataAccess.SqlServer;

public class ShopManagerDbContext : IdentityDbContext<UserEntity, IdentityRole<Guid>, Guid>
{
    public ShopManagerDbContext(DbContextOptions<ShopManagerDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<CategoryEntity> Categories { get; set; }
    public DbSet<OrderItemEntity> OrderItems { get; set; }
    public DbSet<OrderEntity> Orders { get; set; }
    public DbSet<ProductEntity> Products { get; set; }
    public DbSet<SessionEntity> Sessions { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(ShopManagerDbContext).Assembly);
        base.OnModelCreating(builder);
    }
}