namespace ProElection.Tests.Unit.Repositories;

public class ElectionCodeRepositoryTests : DbFaker
{
    private IElectionCodeRepository _substituteElectionCodeRepository;

    public ElectionCodeRepositoryTests()
    {
        _substituteElectionCodeRepository = Substitute.For<ElectionCodeRepository>(InMemoryDb);
    }
    
    [Fact]
    public async Task Create_ShouldAddElectionCodeToDatabase()
    {
        // Arrange
        ElectionCode electionCode = Fakers.FakeElectionCode();
        
        // Act
        await _substituteElectionCodeRepository.Create(electionCode);
        ElectionCode? storedElectionCode = await InMemoryDb.ElectionCodes
            .SingleOrDefaultAsync(code => code.Id == electionCode.Id);

        // Assert
        Assert.Equivalent(electionCode, storedElectionCode);
    }
    
    [Fact]
    public async Task GetById_ShouldReturnElectionCode()
    {
        // Arrange
        ElectionCode electionCode = Fakers.FakeElectionCode();
        await InMemoryDb.ElectionCodes.AddAsync(electionCode);
        await InMemoryDb.SaveChangesAsync();
        
        // Act
        ElectionCode? storedElectionCode = await _substituteElectionCodeRepository.GetById(electionCode.Id);

        // Assert
        Assert.NotNull(storedElectionCode);
        Assert.Equivalent(electionCode, storedElectionCode);
    }
    
    [Fact]
    public async Task GetByElectionAndUser_ShouldReturnCorrectElectionCode()
    {
        // Arrange
        ElectionCode electionCode = Fakers.FakeElectionCode();
        
        await InMemoryDb.ElectionCodes.AddAsync(electionCode);
        await InMemoryDb.SaveChangesAsync();
        
        // Act
        ElectionCode? storedElectionCode = await _substituteElectionCodeRepository
            .GetByElectionAndUser(electionCode.ElectionId, electionCode.UserId);

        // Assert
        Assert.NotNull(storedElectionCode);
        Assert.Equivalent(electionCode, storedElectionCode);
    }
    
    [Fact]
    public async Task Update_ShouldUpdateElectionCode()
    {
        // Arrange
        ElectionCode electionCode = Fakers.FakeElectionCode();
        
        await InMemoryDb.ElectionCodes.AddAsync(electionCode);
        await InMemoryDb.SaveChangesAsync();
        
        ElectionCode updatedElectionCode = electionCode;
        updatedElectionCode.Status = CodeStatus.Used;
        
        // Act
        await _substituteElectionCodeRepository.Update(updatedElectionCode);
        
        ElectionCode? storedElectionCode = await InMemoryDb.ElectionCodes
            .SingleOrDefaultAsync(code => code.Id == electionCode.Id);

        // Assert
        Assert.NotNull(storedElectionCode);
        Assert.Equivalent(updatedElectionCode, storedElectionCode);
    }
}