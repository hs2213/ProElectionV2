using ProElectionV2.Persistence;

namespace ProElection.Tests.Unit.Fakers;

public abstract class DbFaker
{
    protected ProElectionV2DbContext InMemoryDb { get; private set; }

    protected DbFaker()
    {
        DbContextOptionsBuilder<ProElectionV2DbContext> builder = 
            new DbContextOptionsBuilder<ProElectionV2DbContext>();

        builder.EnableDetailedErrors();
        builder.EnableSensitiveDataLogging();
        
        builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        
        InMemoryDb = new ProElectionV2DbContext(builder.Options);
    }
}