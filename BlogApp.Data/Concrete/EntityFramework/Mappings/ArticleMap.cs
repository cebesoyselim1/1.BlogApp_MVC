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

            builder.Property(a => a.Thumbnail).IsRequired();

            builder.Property(a => a.Date).IsRequired();

            builder.Property(a => a.SeoAuthor).IsRequired();
            builder.Property(a => a.SeoAuthor).HasMaxLength(100);

            builder.Property(a => a.SeoDescription).IsRequired();
            builder.Property(a => a.SeoDescription).HasMaxLength(500);

            builder.Property(a => a.SeoTags).IsRequired();
            builder.Property(a => a.SeoTags).HasMaxLength(500);

            builder.Property(a => a.ViewCount).IsRequired();

            builder.Property(a => a.CommentCount).IsRequired();

            builder.Property(a => a.Thumbnail).IsRequired();
            builder.Property(a => a.Thumbnail).HasMaxLength(250);

            builder.Property(a => a.CreatedDate).IsRequired();

            builder.Property(a => a.ModifiedDate).IsRequired();

            builder.Property(a => a.IsActive).IsRequired();
            
            builder.Property(a => a.IsDeleted).IsRequired();

            builder.Property(a => a.CreatedByName).IsRequired();
            builder.Property(a => a.CreatedByName).HasMaxLength(100);

            builder.Property(a => a.ModifiedByName).IsRequired();
            builder.Property(a => a.ModifiedByName).HasMaxLength(100);

            builder.Property(a => a.Note).HasMaxLength(500);

            builder.HasOne<Category>(a => a.Category).WithMany(c => c.Articles).HasForeignKey(a => a.CategoryId);
            builder.HasOne<User>(a => a.User).WithMany(u => u.Articles).HasForeignKey(a => a.UserId);

            builder.ToTable("Articles");

            // builder.HasData(
            //     new Article(){
            //         Id = 1,
            //         CategoryId = 1,
            //         UserId = 1,
            //         Title = "What are new on C# 9.0",
            //         Content = "C# 9.0 introduces record types. You use the record keyword to define a reference type that provides built-in functionality for encapsulating data. You can create record types with immutable properties by using positional parameters or standard property syntax:",
            //         Date = DateTime.Now,
            //         SeoAuthor = "Ali Duman",
            //         SeoDescription = "What are new on C# 9.0",
            //         SeoTags = "C#, C# 9.0, C# news",
            //         ViewCount = 4332,
            //         CommentCont = 57,
            //         Thumbnail = "Default.jpg",
            //         CreatedDate = DateTime.Now,
            //         ModifiedDate = DateTime.Now,
            //         IsActive = true,
            //         IsDeleted = false,
            //         CreatedName = "InitialCreate",
            //         ModifiedName = "InitialCreate",
            //         Note = "C# 9.0 news"
            //     },
            //     new Article(){
            //         Id = 2,
            //         CategoryId = 2,
            //         UserId = 1,
            //         Title = "Briliant changes on C++ 11.0",
            //         Content = "C++11 is a version of the ISO/IEC 14882 standard for the C++ programming language. C++11 replaced the prior version of the C++ standard, called C++03,[1] and was later replaced by C++14. The name follows the tradition of naming language versions by the publication year of the specification, though it was formerly named C++0x because it was expected to be published before 2010.",
            //         Date = DateTime.Now,
            //         SeoAuthor = "Ali Duman",
            //         SeoDescription = "What are news on C++ 11",
            //         SeoTags = "C++, C++ 11.0, C++ news",
            //         ViewCount = 2701,
            //         CommentCont = 32,
            //         Thumbnail = "Default.jpg",
            //         CreatedDate = DateTime.Now,
            //         ModifiedDate = DateTime.Now,
            //         IsActive = true,
            //         IsDeleted = false,
            //         CreatedName = "InitialCreate",
            //         ModifiedName = "InitialCreate",
            //         Note = "C++ 11.0 news"
            //     },
            //     new Article(){
            //         Id = 3,
            //         CategoryId = 3,
            //         UserId = 1,
            //         Title = "What are news on ES6",
            //         Content = "ECMAScript 2015 was the second major revision to JavaScript.ECMAScript 2015 is also known as ES6 and ECMAScript 6.This chapter describes the most important features of ES6.",
            //         Date = DateTime.Now,
            //         SeoAuthor = "Ali Duman",
            //         SeoDescription = "What are news on ES6",
            //         SeoTags = "Javascript, ES6, Javascript ES6",
            //         ViewCount = 1003,
            //         CommentCont = 13,
            //         Thumbnail = "Default.jpg",
            //         CreatedDate = DateTime.Now,
            //         ModifiedDate = DateTime.Now,
            //         IsActive = true,
            //         IsDeleted = false,
            //         CreatedName = "InitialCreate",
            //         ModifiedName = "InitialCreate",
            //         Note = "ES6 news"
            //     }
            // );
            
        }
    }
}