using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopManager.DataAccess.SqlServer.Entities.BaseEntities;

namespace ShopManager.DataAccess.SqlServer.Entities;

public class OrderEntity : BaseEntity<int>
{ 
    public Guid UserId { get; set; }
    public string Number { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }

    public UserEntity User { get; set; }
    public ICollection<OrderItemEntity> OrderItems { get; set; }
}

public class OrderEntityConfiguration : IEntityTypeConfiguration<OrderEntity>
{
    public void Configure(EntityTypeBuilder<OrderEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(o => o.UserId)
            .IsRequired(true);

        builder.Property(o => o.Number)
            .IsRequired(true);

        builder.Property(o => o.OrderDate)
            .IsRequired(true);

        builder.Property(o => o.TotalAmount)
            .IsRequired(true)
            .HasPrecision(18, 2);

        builder.HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(o => o.OrderDate);
        builder.HasIndex(o => o.UserId);
    }
}