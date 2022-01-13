using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogApp.Data.Concrete.EntityFramework.Mappings
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).ValueGeneratedOnAdd();

            builder.Property(u => u.FirstName).IsRequired();
            builder.Property(u => u.FirstName).HasMaxLength(50);
            
            builder.Property(u => u.LastName).IsRequired();
            builder.Property(u => u.LastName).HasMaxLength(50);

            builder.Property(u => u.Email).IsRequired();
            builder.Property(u => u.Email).HasMaxLength(50);
            builder.HasIndex(u => u.Email).IsUnique();

            builder.Property(u => u.UserName).IsRequired();
            builder.Property(u => u.UserName).HasMaxLength(50);
            builder.HasIndex(u => u.UserName).IsUnique();

            builder.Property(u => u.PasswordHash).IsRequired();
            builder.Property(u => u.PasswordHash).HasColumnType("VARBINARY(500)");

            builder.Property(u => u.Image).IsRequired();
            builder.Property(u => u.Image).HasMaxLength(250);

            builder.Property(u => u.Description).HasMaxLength(500);


            builder.Property(u => u.CreatedDate).IsRequired();

            builder.Property(u => u.ModifiedDate).IsRequired();

            builder.Property(u => u.IsActive).IsRequired();
            
            builder.Property(u => u.IsDeleted).IsRequired();

            builder.Property(u => u.CreatedName).IsRequired();
            builder.Property(u => u.CreatedName).HasMaxLength(100);

            builder.Property(u => u.ModifiedName).IsRequired();
            builder.Property(u => u.ModifiedName).HasMaxLength(100);

            builder.Property(u => u.Note).HasMaxLength(100);

            builder.HasOne<Role>(u => u.Role).WithMany(r => r.Users).HasForeignKey(u => u.RoleId);

            builder.ToTable("Users");
        }
    }
}