using CatalogAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Data;

public class CatalogDbContext: DbContext
{
    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Cassette> Cassettes => Set<Cassette>();
    public DbSet<Genre> Genres => Set<Genre>();

    public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options) 
    { 
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
