using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogApp.Data.Concrete.EntityFramework.Mappings
{
    public class CategoryMap : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();
            
            builder.Property(c => c.Name).IsRequired();
            builder.Property(c => c.Name).HasMaxLength(70);

            builder.Property(c => c.Description).HasMaxLength(500);

            builder.Property(c => c.CreatedDate).IsRequired();

            builder.Property(c => c.ModifiedDate).IsRequired();

            builder.Property(c => c.IsActive).IsRequired();
            
            builder.Property(c => c.IsDeleted).IsRequired();

            builder.Property(c => c.CreatedByName).IsRequired();
            builder.Property(c => c.CreatedByName).HasMaxLength(100);

            builder.Property(c => c.ModifiedByName).IsRequired();
            builder.Property(c => c.ModifiedByName).HasMaxLength(100);

            builder.Property(c => c.Note).HasMaxLength(500);

            builder.ToTable("Categories");

            builder.HasData(
                new Category(){
                    Id = 1,
                    Name = "C#",
                    Description = "Everthing about C# programming language",
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedByName = "InitialCreate",
                    ModifiedByName = "InitialCreate",
                    Note = "C# Programming Language"
                },
                new Category(){
                    Id = 2,
                    Name = "C++",
                    Description = "Everthing about C++ programming language",
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedByName = "InitialCreate",
                    ModifiedByName = "InitialCreate",
                    Note = "C++ Programming Language"
                },
                new Category(){
                    Id = 3,
                    Name = "Javascript",
                    Description = "Everthing about Javascript programming language",
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedByName = "InitialCreate",
                    ModifiedByName = "InitialCreate",
                    Note = "Javascript Programming Language"
                }
            );
        }
    }
}