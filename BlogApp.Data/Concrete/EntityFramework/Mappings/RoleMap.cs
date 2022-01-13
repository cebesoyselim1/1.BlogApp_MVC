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
            builder.Property(r => r.Name).HasMaxLength(50);

            builder.Property(r => r.Description).IsRequired();
            builder.Property(r => r.Description).HasMaxLength(200);

            builder.Property(r => r.CreatedDate).IsRequired();

            builder.Property(r => r.ModifiedDate).IsRequired();

            builder.Property(r => r.IsActive).IsRequired();
            
            builder.Property(r => r.IsDeleted).IsRequired();

            builder.Property(r => r.CreatedName).IsRequired();
            builder.Property(r => r.CreatedName).HasMaxLength(100);

            builder.Property(r => r.ModifiedName).IsRequired();
            builder.Property(r => r.ModifiedName).HasMaxLength(100);

            builder.Property(r => r.Note).HasMaxLength(100);

            builder.ToTable("Roles");
        }
    }
}