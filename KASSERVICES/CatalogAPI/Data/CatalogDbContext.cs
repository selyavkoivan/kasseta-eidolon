using CatalogAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Data;

public class CatalogDbContext: DbContext
{
    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Cassette> Cassettes => Set<Cassette>();

    public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options) 
    { 
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
