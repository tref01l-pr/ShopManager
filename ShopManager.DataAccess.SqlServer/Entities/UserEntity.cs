using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopManager.Domain.Interfaces.BaseInterfaces;
using ShopManager.Domain.Interfaces.Repositories;

namespace ShopManager.DataAccess.SqlServer.Entities;
 
public class UserEntity : IdentityUser<Guid>, IDbKey<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public DateTime RegistrationDate { get; set; }
    
    public virtual ICollection<OrderEntity> Orders { get; set; }
}

public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.Property(ue => ue.FirstName)
            .IsRequired(true);

        builder.Property(ue => ue.LastName)
            .IsRequired(true);
        
        builder.Property(ue => ue.MiddleName)
            .IsRequired(true);
        
        builder.Property(ue => ue.DateOfBirth)
            .IsRequired(true);
        
        builder.Property(ue => ue.RegistrationDate)
            .IsRequired(true);
        
        builder.HasIndex(msg => msg.DateOfBirth);
    }
}