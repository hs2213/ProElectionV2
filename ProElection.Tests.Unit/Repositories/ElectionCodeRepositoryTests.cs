using ProElection.Tests.Unit.Fakers;

namespace ProElection.Tests.Unit.Repositories;

public class ElectionCodeRepositoryTests : DbFaker
{
    private readonly IElectionCodeRepository _sut;

    public ElectionCodeRepositoryTests()
    {
        _sut = new ElectionCodeRepository(InMemoryDb);
    }
    
    [Fact]
    public async Task Create_ShouldAddElectionCodeToDatabase()
    {
        // Arrange
        ElectionCode electionCode = Fakers.Fakers.FakeElectionCode();
        
        // Act
        await _sut.Create(electionCode);
        ElectionCode? storedElectionCode = await InMemoryDb.ElectionCodes
            .SingleOrDefaultAsync(code => code.Id == electionCode.Id);

        // Assert
        Assert.Equivalent(electionCode, storedElectionCode);
    }
    
    [Fact]
    public async Task GetById_ShouldReturnElectionCode()
    {
        // Arrange
        ElectionCode electionCode = Fakers.Fakers.FakeElectionCode();
        await InMemoryDb.ElectionCodes.AddAsync(electionCode);
        await InMemoryDb.SaveChangesAsync();
        
        // Act
        ElectionCode? storedElectionCode = await _sut.GetById(electionCode.Id);

        // Assert
        Assert.NotNull(storedElectionCode);
        Assert.Equivalent(electionCode, storedElectionCode);
    }
    
    [Fact]
    public async Task GetByElectionAndUser_ShouldReturnCorrectElectionCode()
    {
        // Arrange
        ElectionCode electionCode = Fakers.Fakers.FakeElectionCode();
        
        await InMemoryDb.ElectionCodes.AddAsync(electionCode);
        await InMemoryDb.SaveChangesAsync();
        
        // Act
        ElectionCode? storedElectionCode = await _sut
            .GetByElectionAndUser(electionCode.ElectionId, electionCode.UserId);

        // Assert
        Assert.NotNull(storedElectionCode);
        Assert.Equivalent(electionCode, storedElectionCode);
    }
    
    [Fact]
    public async Task Update_ShouldUpdateElectionCode()
    {
        // Arrange
        ElectionCode electionCode = Fakers.Fakers.FakeElectionCode();
        
        await InMemoryDb.ElectionCodes.AddAsync(electionCode);
        await InMemoryDb.SaveChangesAsync();
        
        ElectionCode updatedElectionCode = electionCode;
        updatedElectionCode.Status = CodeStatus.Used;
        
        // Act
        await _sut.Update(updatedElectionCode);
        
        ElectionCode? storedElectionCode = await InMemoryDb.ElectionCodes
            .SingleOrDefaultAsync(code => code.Id == electionCode.Id);

        // Assert
        Assert.NotNull(storedElectionCode);
        Assert.Equivalent(updatedElectionCode, storedElectionCode);
    }
}