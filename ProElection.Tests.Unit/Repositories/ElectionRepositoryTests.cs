namespace ProElection.Tests.Unit.Repositories;

public class ElectionRepositoryTests : DbFaker
{
    private IElectionRepository _electionRepository;

    public ElectionRepositoryTests()
    {
        _electionRepository = Substitute.For<ElectionRepository>(InMemoryDb);
    }
    
    private static Election FakeElection() => new Election
    {
        Id = Guid.NewGuid(),
        Name = "Fake Election",
        Start = DateTime.Now,
        End = DateTime.Now.AddDays(1),
        ElectionType = ElectionType.FirstPastThePost
    };
    
    [Fact]
    public async Task GetElections_ShouldReturnAllElections()
    {
        // Arrange
        IEnumerable<Election> fakeElections = new List<Election>
        {
            FakeElection(),
            FakeElection(),
            FakeElection()
        };
        InMemoryDb.Elections.AddRange(fakeElections);
        await InMemoryDb.SaveChangesAsync();
        
        // Act
        IEnumerable<Election> result = await _electionRepository.GetElections();
        
        // Assert
        Assert.NotEmpty(result);
        Assert.Equivalent(result, fakeElections);
    }
    
    [Fact]
    public async Task GetElectionById_ShouldReturnCorrectElection()
    {
        // Arrange
        Election fakeElection = FakeElection();
        
        InMemoryDb.Elections.Add(fakeElection);
        await InMemoryDb.SaveChangesAsync();
        
        // Act
        Election? result = await _electionRepository.GetElectionById(fakeElection.Id);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equivalent(result, fakeElection);
    }
    
    [Fact]
    public async Task CreateElection_ShouldCreateElection()
    {
        // Arrange
        Election fakeElection = FakeElection();
        
        // Act
        await _electionRepository.CreateElection(fakeElection);
        
        Election? electionInDb = 
            await InMemoryDb.Elections.SingleOrDefaultAsync(election => election.Id == fakeElection.Id);
        
        // Assert
        Assert.NotNull(electionInDb);
        Assert.Equivalent(electionInDb, fakeElection);
    }
    
    [Fact]
    public async Task UpdateElection_ShouldUpdateElection()
    {
        // Arrange
        Election fakeElection = FakeElection();
        
        InMemoryDb.Elections.Add(fakeElection);
        await InMemoryDb.SaveChangesAsync();
        
        Election updatedElection = fakeElection;
        updatedElection.Name = "Updated Election";
        
        // Act
        await _electionRepository.UpdateElection(fakeElection);
        
        Election? electionInDb = 
            await InMemoryDb.Elections.SingleOrDefaultAsync(election => election.Id == fakeElection.Id);
        
        // Assert
        Assert.NotNull(electionInDb);
        Assert.Equivalent(electionInDb, updatedElection);
    }
}