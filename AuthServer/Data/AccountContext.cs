using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace AuthServer.Data
{
    public partial class AccountContext : IdentityDbContext<AccountUser>
    {
        public AccountContext()
        {
        }
        public AccountContext(DbContextOptions<AccountContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Identity Roles
            builder.Entity<IdentityRole>().HasData(
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

            base.OnModelCreating(builder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
