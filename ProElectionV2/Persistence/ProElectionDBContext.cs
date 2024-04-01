using Microsoft.EntityFrameworkCore;
using ProElectionV2.Entities;

namespace ProElectionV2.Persistence;

public class ProElectionV2DbContext : DbContext
{
    public ProElectionV2DbContext(DbContextOptions<ProElectionV2DbContext> options) : base(options) { }

    public DbSet<Election> Elections { get; set; }
    public DbSet<ElectionCode> ElectionCodes { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Vote> Votes { get; set; }
}