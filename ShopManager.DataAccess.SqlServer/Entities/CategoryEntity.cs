using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopManager.DataAccess.SqlServer.Entities.BaseEntities;

namespace ShopManager.DataAccess.SqlServer.Entities;

public class CategoryEntity : BaseEntity<int>
{
    public string Name { get; set; }

    public virtual ICollection<ProductEntity> Products { get; set; }
}

public class CategoryEntityConfiguration : IEntityTypeConfiguration<CategoryEntity>
{
    public void Configure(EntityTypeBuilder<CategoryEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(ue => ue.Name)
            .IsRequired(true);
    }
}