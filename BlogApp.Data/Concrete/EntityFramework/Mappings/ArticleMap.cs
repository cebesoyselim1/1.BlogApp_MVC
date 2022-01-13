using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogApp.Data.Concrete.EntityFramework.Mappings
{
    public class ArticleMap : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id).ValueGeneratedOnAdd();

            builder.Property(a => a.Title).IsRequired();
            builder.Property(a => a.Title).HasMaxLength(100);

            builder.Property(a => a.Content).IsRequired();
            builder.Property(a => a.Content).HasColumnType("NVARCHAR(MAX)");

            builder.Property(a => a.Date).IsRequired();

            builder.Property(a => a.SeoAuthor).IsRequired();
            builder.Property(a => a.SeoAuthor).HasMaxLength(50);

            builder.Property(a => a.SeoDescription).IsRequired();
            builder.Property(a => a.SeoDescription).HasMaxLength(150);

            builder.Property(a => a.SeoTags).IsRequired();
            builder.Property(a => a.SeoTags).HasMaxLength(75);

            builder.Property(a => a.ViewCount).IsRequired();

            builder.Property(a => a.CommentCont).IsRequired();

            builder.Property(a => a.Thumbnail).IsRequired();
            builder.Property(a => a.Thumbnail).HasMaxLength(250);

            builder.Property(a => a.CreatedDate).IsRequired();

            builder.Property(a => a.ModifiedDate).IsRequired();

            builder.Property(a => a.IsActive).IsRequired();
            
            builder.Property(a => a.IsDeleted).IsRequired();

            builder.Property(a => a.CreatedName).IsRequired();
            builder.Property(a => a.CreatedName).HasMaxLength(100);

            builder.Property(a => a.ModifiedName).IsRequired();
            builder.Property(a => a.ModifiedName).HasMaxLength(100);

            builder.Property(a => a.Note).HasMaxLength(100);

            builder.HasOne<Category>(a => a.Category).WithMany(c => c.Articles).HasForeignKey(a => a.CategoryId);
            builder.HasOne<User>(a => a.User).WithMany(u => u.Articles).HasForeignKey(a => a.UserId);

            builder.ToTable("Articles");
        }
    }
}