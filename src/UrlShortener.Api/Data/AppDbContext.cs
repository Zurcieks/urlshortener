using Microsoft.EntityFrameworkCore;
using UrlShortener.Api.Entities;

namespace UrlShortener.Api.Data;

public class AppDbContext : DbContext
{
    // this constructor provides the options to the base DbContext class, which is necessary for configuring the database connection and other settings.
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<ShortUrl> ShortUrls { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShortUrl>(entity =>
        {
            entity.HasIndex(e => e.ShortCode).IsUnique();
            entity.HasIndex(e => e.OriginalUrl).IsUnique();

        });
    }

}
