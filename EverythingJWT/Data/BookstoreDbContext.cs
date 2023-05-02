using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EverythingJWT.Data;

public partial class BookstoreDbContext : IdentityDbContext<ApiUser>
{
    public BookstoreDbContext()
    {
    }

    public BookstoreDbContext(DbContextOptions<BookstoreDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Require for Identity Tables
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Authors__3214EC078AA3E904");

            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Books__3214EC07B8BBEE95");

            entity.HasIndex(e => e.Isbn, "UQ__Books__447D36EADD6D4A9C").IsUnique();

            entity.Property(e => e.Image)
                .HasMaxLength(250)
                .IsFixedLength();
            entity.Property(e => e.Isbn)
                .HasMaxLength(50)
                .HasColumnName("ISBN");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Summary).HasMaxLength(250);
            entity.Property(e => e.Title).HasMaxLength(50);

            entity.HasOne(d => d.Author).WithMany(p => p.Books)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Books_ToTable");
        });


        // Identity Roles
        modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole
            {
                Id = "f1a9b119-0f0f-4acb-b7f0-7e0d47373f78",
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
            new IdentityRole
            {
                Id = "d28cf6e4-5ebc-4937-b593-7b5f6d57a4a8",
                Name = "User",
                NormalizedName = "USER"
            });

        // Password hasher for Identity
        var hasher = new PasswordHasher<ApiUser>();

        // Identity Users
        modelBuilder.Entity<ApiUser>().HasData(
            new ApiUser
            {
                Id = "e384573d-d2f7-4547-b9df-036360273883",
                Email = "admin@penpab.com",
                NormalizedEmail = "ADMIN@PENPAB.COM",
                UserName = "Admin",
                NormalizedUserName = "ADMIN",
                FirstName = "System",
                LastName = "Admin",
                PasswordHash = hasher.HashPassword(null,"Penpab123")
            },
            new ApiUser
            {
                Id = "e09f995c-334f-45dc-9c54-d0249bd8ba5f",
                Email = "user@penpab.com",
                NormalizedEmail = "USER@PENPAB.COM",
                UserName = "User",
                NormalizedUserName = "USER",
                FirstName = "System",
                LastName = "User",
                PasswordHash = hasher.HashPassword(null,"Penpab123")
            });

        // Identity User Roles
        modelBuilder.Entity<IdentityUserRole<string>>().HasData(
           new IdentityUserRole<string>
           {
               RoleId = "f1a9b119-0f0f-4acb-b7f0-7e0d47373f78",
               UserId = "e384573d-d2f7-4547-b9df-036360273883",
           },
           new IdentityUserRole<string>
           {
               RoleId = "d28cf6e4-5ebc-4937-b593-7b5f6d57a4a8",
               UserId = "e09f995c-334f-45dc-9c54-d0249bd8ba5f",
           });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
