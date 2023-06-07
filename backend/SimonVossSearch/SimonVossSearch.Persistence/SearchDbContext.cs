using Microsoft.EntityFrameworkCore;
using SimonVossSearch.Domain;

namespace Persistence;

public class SearchDbContext : DbContext
{
    public DbSet<Building> Buildings { get; set; }
    public DbSet<Lock> Locks { get; set; }
    public DbSet<Medium> Media { get; set; }
    public DbSet<Group> Groups { get; set; }

    public SearchDbContext() : base() { }
    public SearchDbContext(DbContextOptions<SearchDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("User ID=postgres;Password=postgrespw;Host=localhost;Port=5432;Database=mydb;");
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Building>().HasKey(x => x.Id);
        
        builder.Entity<Lock>().HasKey(x => x.Id);

        builder.Entity<Medium>().HasKey(x => x.Id);

        builder.Entity<Group>().HasKey(x => x.Id);
    }
}