using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogApp.Data.Concrete.EntityFramework.Mappings
{
    public class RoleMap : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Id).ValueGeneratedOnAdd();

            builder.Property(r => r.Name).IsRequired();
            builder.Property(r => r.Name).HasMaxLength(100);

            builder.Property(r => r.Description).IsRequired();
            builder.Property(r => r.Description).HasMaxLength(500);

            builder.Property(r => r.CreatedDate).IsRequired();

            builder.Property(r => r.ModifiedDate).IsRequired();

            builder.Property(r => r.IsActive).IsRequired();
            
            builder.Property(r => r.IsDeleted).IsRequired();

            builder.Property(r => r.CreatedName).IsRequired();
            builder.Property(r => r.CreatedName).HasMaxLength(100);

            builder.Property(r => r.ModifiedName).IsRequired();
            builder.Property(r => r.ModifiedName).HasMaxLength(100);

            builder.Property(r => r.Note).HasMaxLength(500);

            builder.ToTable("Roles");

            builder.HasData(
                new Role(){
                    Id = 1,
                    Name = "Admin",
                    Description = "Access anywhere",
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedName = "InitialCreate",
                    ModifiedName = "InitialCreate",
                    Note = "Admin Role"
                }
            );
        }
    }
}