namespace ProElection.Tests.Unit.Repositories;

public class ElectionRepositoryTests : DbFaker
{
    private readonly IElectionRepository _sut;

    public ElectionRepositoryTests()
    {
        _sut = new ElectionRepository(InMemoryDb);
    }
    
    [Fact]
    public async Task GetElections_ShouldReturnAllElections()
    {
        // Arrange
        IEnumerable<Election> fakeElections = new List<Election>
        {
            Fakers.FakeElection(),
            Fakers.FakeElection(),
            Fakers.FakeElection()
        };
        InMemoryDb.Elections.AddRange(fakeElections);
        await InMemoryDb.SaveChangesAsync();
        
        // Act
        IEnumerable<Election> result = await _sut.GetElections();
        
        // Assert
        Assert.NotEmpty(result);
        Assert.Equivalent(result, fakeElections);
    }
    
    [Fact]
    public async Task GetElectionById_ShouldReturnCorrectElection()
    {
        // Arrange
        Election fakeElection = Fakers.FakeElection();
        
        InMemoryDb.Elections.Add(fakeElection);
        await InMemoryDb.SaveChangesAsync();
        
        // Act
        Election? result = await _sut.GetById(fakeElection.Id);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equivalent(result, fakeElection);
    }
    
    [Fact]
    public async Task CreateElection_ShouldCreateElection()
    {
        // Arrange
        Election fakeElection = Fakers.FakeElection();
        
        // Act
        await _sut.Create(fakeElection);
        
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
        Election fakeElection = Fakers.FakeElection();
        
        InMemoryDb.Elections.Add(fakeElection);
        await InMemoryDb.SaveChangesAsync();
        
        Election updatedElection = fakeElection;
        updatedElection.Name = "Updated Election";
        
        // Act
        await _sut.Update(fakeElection);
        
        Election? electionInDb = 
            await InMemoryDb.Elections.SingleOrDefaultAsync(election => election.Id == fakeElection.Id);
        
        // Assert
        Assert.NotNull(electionInDb);
        Assert.Equivalent(electionInDb, updatedElection);
    }
}