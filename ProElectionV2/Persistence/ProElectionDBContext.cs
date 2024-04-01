using Microsoft.EntityFrameworkCore;
using ProElectionV2.Entities;

namespace ProElectionV2.Persistence;

public class ProElectionV2DbContext : DbContext
{
    public ProElectionV2DbContext(DbContextOptions<ProElectionV2DbContext> options) : base(options)
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("DataSource=ProElectionV2.db");
        base.OnConfiguring(optionsBuilder);
    }

    public DbSet<Election> Elections { get; set; }
    public DbSet<ElectionCode> ElectionCodes { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Vote> Votes { get; set; }
}