using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagement.Infrastructure.Data.Identity;

namespace TaskManagement.Infrastructure.Data.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasMany(u => u.Tasks)
                .WithOne()
                .HasForeignKey(t => t.IdentityUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
