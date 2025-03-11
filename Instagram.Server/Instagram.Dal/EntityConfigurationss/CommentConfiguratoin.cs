using Instagram.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Instagram.Dal.EntityConfigurationss;

public class CommentConfiguratoin : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("Comment");

        builder.HasKey(c => c.CommentId);

        builder.Property(c => c.CommentId)
            .IsRequired(true)
            .HasMaxLength(200);

        builder.Property(c => c.CreatedTime)
            .IsRequired(true);


        builder.HasOne(c => c.ParentComment)
            .WithMany(c => c.Replies)
            .HasForeignKey(c => c.CommentId);
    }
}
