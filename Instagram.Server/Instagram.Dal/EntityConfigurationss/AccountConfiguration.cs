using Instagram.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace Instagram.Dal.EntityConfigurationss
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("Account");

            builder.HasKey(a => a.AccountId);

            builder.Property(a => a.UserName).IsRequired(true);

            builder.HasIndex(a => a.UserName).IsUnique(true);

            builder.Property(a => a.Bio)
                .IsRequired(false)
                .HasMaxLength(200);

            builder.HasMany(a => a.Posts)
                .WithOne(p => p.Account)
                .HasForeignKey(p => p.AccountId);

            builder.HasMany(a => a.Comments)
                .WithOne(c => c.Account)
                .HasForeignKey(c => c.AccountId);

            builder.HasMany(a => a.Followers)
                .WithMany(a => a.Following);
        }
    }
}
