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
            builder.Property(c => c.Text).HasMaxLength(1000);

            builder.Property(c => c.CreatedDate).IsRequired();

            builder.Property(c => c.ModifiedDate).IsRequired();

            builder.Property(c => c.IsActive).IsRequired();
            
            builder.Property(c => c.IsDeleted).IsRequired();

            builder.Property(c => c.CreatedByName).IsRequired();
            builder.Property(c => c.CreatedByName).HasMaxLength(100);

            builder.Property(c => c.ModifiedByName).IsRequired();
            builder.Property(c => c.ModifiedByName).HasMaxLength(100);

            builder.Property(c => c.Note).HasMaxLength(500);

            builder.HasOne<Article>(c => c.Article).WithMany(a => a.Comments).HasForeignKey(c => c.ArticleId);

            builder.ToTable("Comments");

            // builder.HasData(
            //     new Comment(){
            //         Id = 1,
            //         ArticleId = 1,
            //         Text = "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum is that it has a more-or-less normal distribution of letters, as opposed to using 'Content here, content here', making it look like readable English. Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like)",
            //         CreatedDate = DateTime.Now,
            //         ModifiedDate = DateTime.Now,
            //         IsActive = true,
            //         IsDeleted = false,
            //         CreatedName = "InitialCreate",
            //         ModifiedName = "InitialCreate",
            //         Note = "C# Comment"
            //     },
            //     new Comment(){
            //         Id = 2,
            //         ArticleId = 1,
            //         Text = "There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable. If you are going to use a passage of Lorem Ipsum, you need to be sure there isn't anything embarrassing hidden in the middle of text. All the Lorem Ipsum generators on the Internet tend to repeat predefined chunks as necessary, making this the first true generator on the Internet. It uses a dictionary of over 200 Latin words, combined with a handful of model sentence structures, to generate Lorem Ipsum which looks reasonable. The generated Lorem Ipsum is therefore always free from repetition, injected humour, or non-characteristic words etc.",
            //         CreatedDate = DateTime.Now,
            //         ModifiedDate = DateTime.Now,
            //         IsActive = true,
            //         IsDeleted = false,
            //         CreatedName = "InitialCreate",
            //         ModifiedName = "InitialCreate",
            //         Note = "C# Comment"
            //     },
            //     new Comment(){
            //         Id = 3,
            //         ArticleId = 2,
            //         Text = "Phasellus eu vehicula massa. Sed viverra ut dui ac lacinia. Aenean vestibulum eget ex vel finibus. Morbi non porttitor metus. Nam rhoncus quam vitae quam elementum tincidunt. Fusce vel eros tempus, ullamcorper massa vitae, varius nunc. In ut bibendum diam. Duis sit amet vestibulum lectus. Morbi lacinia quam vitae viverra condimentum. In porta, augue id pharetra cursus, odio felis molestie turpis, sit amet scelerisque urna ex sed neque.",
            //         CreatedDate = DateTime.Now,
            //         ModifiedDate = DateTime.Now,
            //         IsActive = true,
            //         IsDeleted = false,
            //         CreatedName = "InitialCreate",
            //         ModifiedName = "InitialCreate",
            //         Note = "C++ Comment"
            //     },
            //     new Comment(){
            //         Id = 4,
            //         ArticleId = 3,
            //         Text = "Aenean finibus nibh at purus dictum, quis condimentum eros viverra. Proin sed imperdiet dolor. Maecenas at est risus. Quisque commodo tortor eu ligula porttitor feugiat ut id nulla. Fusce et volutpat nibh, id ultrices diam. Sed est sem, consequat in arcu et, consequat eleifend orci. Pellentesque dignissim nec ex bibendum tincidunt.",
            //         CreatedDate = DateTime.Now,
            //         ModifiedDate = DateTime.Now,
            //         IsActive = true,
            //         IsDeleted = false,
            //         CreatedName = "InitialCreate",
            //         ModifiedName = "InitialCreate",
            //         Note = "Javascript Comment"
            //     }
            // );
        }
    }
}