using FluentValidation;

namespace ProElection.Tests.Unit.Services;

public class ElectionServiceTests : DbFaker
{
    private readonly IElectionService _sut;
    private readonly IElectionRepository _substituteElectionRepository;
    private readonly IElectionCodeRepository _substituteElectionCodeRepository;
    private readonly IVoteRepository _substituteVoteRepository;
    private readonly IUserRepository _substituteUserRepository;
    private readonly INotifyService _substituteNotifyService;

    public ElectionServiceTests()
    {
        _substituteElectionRepository = Substitute.For<IElectionRepository>();
        _substituteElectionCodeRepository = Substitute.For<IElectionCodeRepository>();
        _substituteVoteRepository = Substitute.For<IVoteRepository>();
        _substituteUserRepository = Substitute.For<IUserRepository>();
        _substituteNotifyService = Substitute.For<INotifyService>();

        _sut = new ElectionService(
            _substituteElectionRepository,
            _substituteElectionCodeRepository,
            _substituteVoteRepository,
            _substituteUserRepository,
            new ElectionCodeValidator(),
            new ElectionValidator(),
            new VoteValidator(),
            _substituteNotifyService);
    }
    
    [Fact]
    public async Task CreateElection_WithValidElection_ShouldReturnElection()
    {
        // Arrange
        var election = Fakers.FakeElection();
        
        _substituteElectionRepository.Create(election).Returns(election);
        
        // Act
        Election result = await _sut.CreateElection(election);
        
        // Assert
        Assert.Equivalent(election, result);
    }
    
    [Fact]
    public async Task CreateElection_WithInvalidElection_ShouldThrowValidationException()
    {
        // Arrange
        var election = Fakers.FakeElection();
        
        election.Name = string.Empty;
        
        _substituteElectionRepository.Create(election).Returns(election);
        
        // Act
        Task CreateElection() => _sut.CreateElection(election);
        
        // Assert
        await Assert.ThrowsAsync<ValidationException>(CreateElection);
    }
    
    [Fact]
    public async Task GetElectionById_WithValidElectionId_ShouldReturnElection()
    {
        // Arrange
        var election = Fakers.FakeElection();
        
        _substituteElectionRepository.GetById(election.Id).Returns(election);
        
        // Act
        Election? result = await _sut.GetElectionById(election.Id);
        
        // Assert
        Assert.Equivalent(election, result);
    }
    
    [Fact]
    public async Task GetAllElections_WithElections_ShouldReturnElections()
    {
        // Arrange
        var elections = new List<Election>
        {
            Fakers.FakeElection(),
            Fakers.FakeElection(),
        };
        
        _substituteElectionRepository.GetElections().Returns(elections);
        
        // Act
        IEnumerable<Election> result = await _sut.GetAllElections();
        
        // Assert
        Assert.Equivalent(elections, result);
    }
    
    [Fact]
    public async Task GetElectionsByMultipleIds_WithElectionIds_ShouldReturnElections()
    {
        // Arrange
        List<Election> elections =
        [
            Fakers.FakeElection(),
            Fakers.FakeElection()
        ];

        List<Guid> electionIds = elections.Select(election => election.Id).ToList();
        
        _substituteElectionRepository.GetMultipleElectionsByIds(electionIds).Returns(elections);
        
        // Act
        IEnumerable<Election> result = await _sut.GetElectionsByMultipleIds(electionIds);
        
        // Assert
        Assert.Equivalent(elections, result);
    }
    
    [Fact]
    public async Task GetElectionCodeById_WithValidElectionCodeId_ShouldReturnElectionCode()
    {
        // Arrange
        ElectionCode electionCode = Fakers.FakeElectionCode();
        
        _substituteElectionCodeRepository.GetById(electionCode.Id).Returns(electionCode);
        _substituteElectionRepository.GetById(electionCode.ElectionId).Returns(Fakers.FakeElection());
        
        // Act
        ElectionCode? result = await _sut.GetElectionCodeById(electionCode.Id);
        
        // Assert
        Assert.Equivalent(electionCode, result);
    }
    
    [Fact]
    public async Task GetElectionCodeById_WithInvalidElectionCodeId_ShouldReturnNull()
    {
        // Arrange
        Guid electionCodeId = Guid.NewGuid();
        
        _substituteElectionCodeRepository.GetById(electionCodeId).Returns((ElectionCode?)null);
        
        // Act
        ElectionCode? result = await _sut.GetElectionCodeById(electionCodeId);
        
        // Assert
        Assert.Null(result);
        await _substituteNotifyService.Received(1)
            .ShowNotification("Failed to get election from code. Please try again");
    }
    
    [Fact]
    public async Task GetElectionCodeById_WithUsedElectionCode_ShouldReturnElectionCode()
    {
        // Arrange
        ElectionCode electionCode = Fakers.FakeElectionCode();
        electionCode.Status = CodeStatus.Used;
        
        _substituteElectionCodeRepository.GetById(electionCode.Id).Returns(electionCode);
        
        // Act
        ElectionCode? result = await _sut.GetElectionCodeById(electionCode.Id);
        
        // Assert
        Assert.Equivalent(electionCode, result);
        await _substituteNotifyService.Received(1).ShowNotification("Code has already been used.");
    }
    
    [Fact]
    public async Task GetElectionCodeById_WithElectionCodeAssociatedToNonExistingElection_ShouldReturnNull()
    {
        // Arrange
        ElectionCode electionCode = Fakers.FakeElectionCode();
        
        _substituteElectionCodeRepository.GetById(electionCode.Id).Returns(electionCode);
        _substituteElectionRepository.GetById(electionCode.ElectionId).Returns((Election?)null);
        
        // Act
        ElectionCode? result = await _sut.GetElectionCodeById(electionCode.Id);
        
        // Assert
        Assert.Null(result);
        await _substituteNotifyService.Received(1).ShowNotification("Election has ended or does not exist.");
    }
    
    [Fact]
    public async Task GetElectionCodeById_WithElectionCodeAssociatedToExpiredElection_ShouldReturnNull()
    {
        // Arrange
        ElectionCode electionCode = Fakers.FakeElectionCode();
        Election election = Fakers.FakeElection();
        election.End = DateTime.Now.AddDays(-1);
        
        _substituteElectionCodeRepository.GetById(electionCode.Id).Returns(electionCode);
        _substituteElectionRepository.GetById(electionCode.ElectionId).Returns(election);
        
        // Act
        ElectionCode? result = await _sut.GetElectionCodeById(electionCode.Id);
        
        // Assert
        Assert.Null(result);
        await _substituteNotifyService.Received(1).ShowNotification("Election has ended or does not exist.");
    }
    
    [Fact]
    public async Task GetElectionCodeByUserAndElection_WithValidElectionAndUser_ShouldReturnElectionCode()
    {
        // Arrange
        ElectionCode electionCode = Fakers.FakeElectionCode();
        
        _substituteElectionCodeRepository.GetByElectionAndUser(electionCode.ElectionId, electionCode.UserId)
            .Returns(electionCode);
        
        // Act
        ElectionCode result = await _sut.GetElectionCodeByUserAndElection(electionCode.ElectionId, electionCode.UserId);
        
        // Assert
        Assert.Equivalent(electionCode, result);
        await _substituteElectionCodeRepository.Received(1)
            .GetByElectionAndUser(electionCode.ElectionId, electionCode.UserId);
    }
    
    [Fact]
    public async Task GetElectionCodeByUserAndElection_WithNonExistingElectionAndUser_ShouldReturnNewElectionCode()
    {
        // Arrange
        ElectionCode electionCode = Fakers.FakeElectionCode();
        
        _substituteElectionCodeRepository
            .GetByElectionAndUser(electionCode.ElectionId, electionCode.UserId)
            .Returns((ElectionCode?)null);
        _substituteElectionCodeRepository.Create(Arg.Any<ElectionCode>()).Returns(electionCode);
        
        // Act
        ElectionCode result = await _sut.GetElectionCodeByUserAndElection(electionCode.ElectionId, electionCode.UserId);
        
        // Assert
       Assert.Equivalent(electionCode, result);
       await _substituteElectionCodeRepository.Received(1).Create(Arg.Any<ElectionCode>());
    }
    
    [Fact]
    public async Task Vote_WithValidVote_ShouldCreateVote()
    {
        // Arrange
        Vote vote = Fakers.FakeVote();
        
        // Act
        await _sut.Vote(vote);
        
        // Assert
        await _substituteVoteRepository.Received(1).Create(vote);
    }
    
    [Fact]
    public async Task Vote_WithInvalidVote_ShouldThrowValidationException()
    {
        // Arrange
        Vote vote = Fakers.FakeVote();
        
        vote.UserId = Guid.Empty;
        
        // Act
        Task Vote() => _sut.Vote(vote);
        
        // Assert
        await Assert.ThrowsAsync<ValidationException>(Vote);
    }
    
    [Fact]
    public async Task MarkElectionCodeAsUsed_WithElectionCode_ShouldUpdateElectionCode()
    {
        // Arrange
        ElectionCode electionCode = Fakers.FakeElectionCode();
        electionCode.Status = CodeStatus.Used;
        
        // Act
        await _sut.MarkElectionCodeAsUsed(electionCode);
        
        // Assert
        await _substituteElectionCodeRepository.Received(1).Update(electionCode);
        await _substituteNotifyService.Received(1).ShowNotification("Election Code Marked as Used");
    }
    
    [Fact]
    public async Task CheckIfUserHasVoted_WithUserAndElection_ShouldReturnTrue()
    {
        // Arrange
        User user = Fakers.FakeUser(UserType.Voter);
        Election election = Fakers.FakeElection();
        
        _substituteVoteRepository.CheckIfUserVotedInElection(user.Id, election.Id).Returns(true);
        
        // Act
        bool result = await _sut.CheckIfUserVoted(user.Id, election.Id);
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public async Task CheckIfUserHasVoted_WithUserAndElection_ShouldReturnFalse()
    {
        // Arrange
        User user = Fakers.FakeUser(UserType.Voter);
        Election election = Fakers.FakeElection();
        
        _substituteVoteRepository.CheckIfUserVotedInElection(user.Id, election.Id).Returns(false);
        
        // Act
        bool result = await _sut.CheckIfUserVoted(user.Id, election.Id);
        
        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public async Task CalculateResults_WithElection_ShouldReturnCorrectResults()
    {
        // Arrange
        Election election = Fakers.FakeElection();
        
        User candidate1 = Fakers.FakeUser(UserType.Candidate);
        User candidate2 = Fakers.FakeUser(UserType.Candidate);
        
        _substituteElectionRepository.GetById(election.Id).Returns(election);
        _substituteUserRepository.GetCandidatesOfAnElection(election.Id).Returns([candidate1, candidate2]);
        
        _substituteVoteRepository.GetCandidateVotesByElectionId(candidate1.Id, election.Id).Returns(3);
        _substituteVoteRepository.GetCandidateVotesByElectionId(candidate2.Id, election.Id).Returns(1);
        
        // Act
        Dictionary<User, int>? result = await _sut.CalculateResults(election.Id);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        
        Assert.Equivalent(candidate1, result.Keys.First());
        Assert.Equal(3, result.Values.First());
        
        Assert.Equivalent(candidate2, result.Keys.Last());
        Assert.Equal(1, result.Values.Last());
    }
}