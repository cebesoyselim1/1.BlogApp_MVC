using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogApp.Data.Concrete.EntityFramework.Mappings
{
    public class CommentMap : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();

            builder.Property(c => c.Text).IsRequired();
            builder.Property(c => c.Text).HasMaxLength(250);

            builder.Property(c => c.CreatedDate).IsRequired();

            builder.Property(c => c.ModifiedDate).IsRequired();

            builder.Property(c => c.IsActive).IsRequired();
            
            builder.Property(c => c.IsDeleted).IsRequired();

            builder.Property(c => c.CreatedName).IsRequired();
            builder.Property(c => c.CreatedName).HasMaxLength(100);

            builder.Property(c => c.ModifiedName).IsRequired();
            builder.Property(c => c.ModifiedName).HasMaxLength(100);

            builder.Property(c => c.Note).HasMaxLength(100);

            builder.HasOne<Article>(c => c.Article).WithMany(a => a.Comments).HasForeignKey(c => c.ArticleId);

            builder.ToTable("Comments");
        }
    }
}