using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopManager.DataAccess.SqlServer.Entities.BaseEntities;

namespace ShopManager.DataAccess.SqlServer.Entities;

public class ProductEntity : BaseEntity<int>
{
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public string ArticleNumber { get; set; }
    public decimal Price { get; set; }

    public CategoryEntity Category { get; set; }
    public virtual ICollection<OrderItemEntity> OrderItems { get; set; }
}

public class ProductEntityConfiguration : IEntityTypeConfiguration<ProductEntity>
{
    public void Configure(EntityTypeBuilder<ProductEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(ue => ue.Name)
            .IsRequired(true);

        builder.Property(ue => ue.CategoryId)
            .IsRequired(true);

        builder.Property(ue => ue.ArticleNumber)
            .IsRequired(true);

        builder.Property(ue => ue.Price)
            .IsRequired(true)
            .HasPrecision(18, 2);

        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(o => o.CategoryId);
    }
}