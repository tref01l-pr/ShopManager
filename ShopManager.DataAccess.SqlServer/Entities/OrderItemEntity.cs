using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopManager.DataAccess.SqlServer.Entities.BaseEntities;

namespace ShopManager.DataAccess.SqlServer.Entities;

public class OrderItemEntity : BaseEntity<int>
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    public OrderEntity Order { get; set; }
    public ProductEntity Product { get; set; }
}

public class OrderItemEntityConfiguration : IEntityTypeConfiguration<OrderItemEntity>
{
    public void Configure(EntityTypeBuilder<OrderItemEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(oi => oi.OrderId)
            .IsRequired(true);

        builder.Property(oi => oi.ProductId)
            .IsRequired(true);

        builder.Property(oi => oi.Quantity)
            .IsRequired(true);

        builder.Property(oi => oi.UnitPrice)
            .IsRequired(true)
            .HasPrecision(18, 2);

        builder.HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(o => o.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(oi => oi.Product)
            .WithMany(p => p.OrderItems)
            .HasForeignKey(o => o.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(o => o.OrderId);
    }
}