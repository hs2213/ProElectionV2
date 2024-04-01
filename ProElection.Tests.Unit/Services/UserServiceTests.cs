namespace ProElection.Tests.Unit.Services;

public class UserServiceTests : DbFaker
{
    private readonly IUserService _sut;
    private readonly IUserRepository _substituteUserRepository;
    private readonly IElectionService _substituteElectionService;
    private readonly INotifyService _substituteNotifyService;

    public UserServiceTests()
    {
        _substituteUserRepository = Substitute.For<UserRepository>(InMemoryDb);
        _substituteElectionService = Substitute.For<IElectionService>();
        _substituteNotifyService = Substitute.For<INotifyService>();

        _sut = new UserService(
            _substituteUserRepository,
            _substituteElectionService,
            new UserValidator(),
            _substituteNotifyService);
    }
    
    [Fact]
    public async Task GetUserById_WhenUserExists_ReturnsUser()
    {
        // Arrange
        var user = Fakers.FakeUser(UserType.Voter);
        _substituteUserRepository.GetUserById(user.Id).Returns(user);
        
        // Act
        var result = await _sut.GetUserById(user.Id);

        // Assert
        Assert.Equivalent(user, result);
        await _substituteNotifyService.DidNotReceive().ShowNotification(Arg.Any<string>());
    }
    
    [Fact]
    public async Task GetUserById_WhenUserDoesNotExist_ReturnsNull()
    {
        // Arrange
        Guid userId = Guid.NewGuid();
        _substituteUserRepository.GetUserById(userId).Returns((User?)null);
        
        // Act
        var result = await _sut.GetUserById(userId);

        // Assert
        Assert.Null(result);
        await _substituteNotifyService.Received(1).ShowNotification("Failed to get user.");
    }
    
    [Fact]
    public async Task Authenticate_WhenUserExists_WithCorrectPassword_ReturnsUser()
    {
        // Arrange
        User user = Fakers.FakeUser(UserType.Voter);
        _substituteUserRepository.GetUserByEmail(user.Email).Returns(user);
        
        // Act
        User? result = await _sut.Authenticate(user.Email, "Test123");

        // Assert
        Assert.Equivalent(user, result);
        await _substituteNotifyService.Received(1).ShowNotification("Successfully Authenticated");
    }
    
    [Fact]
    public async Task Authenticate_WhenUserExists_WithInCorrectPassword_ReturnsUser()
    {
        // Arrange
        User user = Fakers.FakeUser(UserType.Voter);
        _substituteUserRepository.GetUserByEmail(user.Email).Returns(user);
        
        // Act
        User? result = await _sut.Authenticate(user.Email, "WrongPassword1");

        // Assert
        Assert.Null(result);
        await _substituteNotifyService.Received(1).ShowNotification("Password is incorrect.");
    }
    
    [Fact]
    public async Task Authenticate_WhenUserDoesNotExist_ReturnsNull()
    {
        // Arrange
        const string nonExistentEmail = "hello@hello.com";
        _substituteUserRepository.GetUserByEmail(nonExistentEmail).Returns((User?)null);
        
        // Act
        User? result = await _sut.Authenticate(nonExistentEmail, "teststs");

        // Assert
        Assert.Null(result);
        await _substituteNotifyService.Received(1).ShowNotification("No account associated with that email");
    }
    
    [Fact]
    public async Task CreateUser_WhenEmailExists_ReturnsNull()
    {
        // Arrange
        User user = Fakers.FakeUser(UserType.Voter);
        _substituteUserRepository.CheckEmailExists(user.Email).Returns(true);
        
        // Act
        User? result = await _sut.CreateUser(user);

        // Assert
        Assert.Null(result);
        await _substituteNotifyService.Received(1).ShowNotification("Email already exists");
    }
    
    [Fact]
    public async Task CreateUser_WhenEmailDoesNotExist_ReturnsUser()
    {
        // Arrange
        User user = Fakers.FakeUser(UserType.Voter);
        _substituteUserRepository.CheckEmailExists(user.Email).Returns(false);
        _substituteUserRepository.CreateUser(user).Returns(user);
        
        // Act
        User? result = await _sut.CreateUser(user);

        // Assert
        Assert.Equivalent(user, result);
        await _substituteNotifyService.Received(1).ShowNotification("Successfully Created User");
    }
    
    [Fact]
    public async Task GetUserElections_WhenUserExists_ReturnsElections()
    {
        // Arrange
        List<Election> elections =
        [
            Fakers.FakeElection(),
            Fakers.FakeElection(),
            Fakers.FakeElection()
        ];
        
        User user = Fakers.FakeUser(UserType.Voter);
        user.ParticipatingElections = [elections[0].Id, elections[1].Id, elections[2].Id];
        
        _substituteUserRepository.GetUserById(user.Id).Returns(user);
        _substituteElectionService.GetElectionsByMultipleIds(user.ParticipatingElections).Returns(elections);
        
        // Act
        var result = await _sut.GetUserElections(user.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equivalent(result, elections); 
    }
    
    [Fact]
    public async Task GetUserElections_WhenUserDoesNotExist_ReturnsNull()
    {
        // Arrange
        Guid userId = Guid.NewGuid();
        _substituteUserRepository.GetUserById(userId).Returns((User?)null);
        
        // Act
        var result = await _sut.GetUserElections(userId);

        // Assert
        Assert.Null(result);
    }
    
    [Fact]
    public async Task GetCandidatesForElection_WhenElectionExists_ReturnsCandidates()
    {
        // Arrange
        Election election = Fakers.FakeElection();
        List<User> candidates =
        [
            Fakers.FakeUser(UserType.Candidate),
            Fakers.FakeUser(UserType.Candidate),
            Fakers.FakeUser(UserType.Candidate)
        ];
        
        _substituteUserRepository.GetCandidatesOfAnElection(election.Id).Returns(candidates);
        
        // Act
        var result = await _sut.GetCandidatesForElection(election);

        // Assert
        Assert.NotNull(result);
        Assert.Equivalent(result, candidates);
    }
    
    [Fact]
    public async Task AddElectionToUser_WhenUserIsNotPartOfElection_AddsElection()
    {
        // Arrange
        User user = Fakers.FakeUser(UserType.Voter);
        Election election = Fakers.FakeElection();
        
        // Act
        await _sut.AddElectionToUser(user, election);

        // Assert
        await _substituteNotifyService.DidNotReceive().ShowNotification("User is already a part of the election");
        await _substituteUserRepository.Received(1).UpdateUser(user);
        Assert.Contains(election.Id, user.ParticipatingElections);
    }
    
    [Fact]
    public async Task AddElectionToUser_WhenUserIsPartOfElection_DoesNotAddElection()
    {
        // Arrange
        User user = Fakers.FakeUser(UserType.Voter);
        Election election = Fakers.FakeElection();
        user.ParticipatingElections.Add(election.Id);
        
        // Act
        await _sut.AddElectionToUser(user, election);

        // Assert
        await _substituteNotifyService.Received(1).ShowNotification("User is already a part of the election");
        await _substituteUserRepository.DidNotReceive().UpdateUser(user);
        Assert.Contains(election.Id, user.ParticipatingElections);
    }
    
    [Fact]
    public async Task GetUsersByEmailSearch_WhenUserExists_ReturnsUsers()
    {
        // Arrange
        string searchQuery = "hello";
        UserType userType = UserType.Voter;
        Guid electionId = Guid.NewGuid();
        
        List<User> users =
        [
            Fakers.FakeUser(UserType.Voter),
            Fakers.FakeUser(UserType.Voter),
            Fakers.FakeUser(UserType.Voter)
        ];

        foreach (User user in users)
        {
            user.ParticipatingElections.Add(electionId);
        }
        
        _substituteUserRepository.GetUserBySearchForElection(searchQuery, userType, electionId).Returns(users);
        
        // Act
        var result = await _sut.GetUsersByEmailSearch(searchQuery, userType, electionId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count());
    }
}