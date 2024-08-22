using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

public class StoreContext : IdentityDbContext<User>
{
    public StoreContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Cart> Carts => Set<Cart>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<IdentityRole>()
            .HasData(
                new IdentityRole { Name = "Member", NormalizedName = "MEMBER" },
                new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" }
            );
    }
}