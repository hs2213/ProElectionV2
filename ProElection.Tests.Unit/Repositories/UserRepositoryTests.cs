﻿namespace ProElection.Tests.Unit.Repositories;

public class UserRepositoryTests : DbFaker
{
    private readonly IUserRepository _substituteUserRepository;
    
    public UserRepositoryTests()
    {
        _substituteUserRepository = Substitute.For<UserRepository>(InMemoryDb);
    }
    
    [Fact]
    public async Task GetUserById_WhenUserExists_ReturnsUser()
    {
        // Arrange
        User user = Fakers.FakeUser(UserType.Voter);
        InMemoryDb.Users.Add(user);
        await InMemoryDb.SaveChangesAsync();
        
        // Act
        User? response = await _substituteUserRepository.GetUserById(user.Id);
        
        // Assert
        Assert.NotNull(response);
        Assert.Equivalent(user, response);
    }
    
    [Fact] 
    public async Task GetUserById_WhenUserDoesNotExist_ReturnsNull()
    {
        // Arrange
        User user = Fakers.FakeUser(UserType.Voter);
        
        // Act
        User? response = await _substituteUserRepository.GetUserById(user.Id);
        
        // Assert
        Assert.Null(response);
    }
    
    [Fact]
    public async Task GetUserByEmail_WhenUserExists_ReturnsUser()
    {
        // Arrange
        User user = Fakers.FakeUser(UserType.Voter);
        InMemoryDb.Users.Add(user);
        await InMemoryDb.SaveChangesAsync();
        
        // Act
        User? response = await _substituteUserRepository.GetUserByEmail(user.Email);
        
        // Assert
        Assert.NotNull(response);
        Assert.Equivalent(user, response);
    }
    
    [Fact]
    public async Task GetUserByEmail_WhenUserDoesNotExist_ReturnsNull()
    {
        // Arrange
        User fakeUser = Fakers.FakeUser(UserType.Voter);
        
        // Act
        User? response = await _substituteUserRepository.GetUserByEmail(fakeUser.Email);
        User? find = await InMemoryDb.Users.SingleOrDefaultAsync(user => user.Id == fakeUser.Id);
        
        // Assert
        Assert.Null(find);
        Assert.Null(response);
    }
    
    [Fact]
    public async Task CreateUser_SuccessfullyCreatesUser()
    {
        // Arrange
        User fakeUser = Fakers.FakeUser(UserType.Voter);
        
        // Act
        User response = await _substituteUserRepository.CreateUser(fakeUser);
        
        User? createdUser = await InMemoryDb.Users.SingleOrDefaultAsync(user => user.Id == response.Id);
        
        // Assert
        Assert.NotNull(createdUser);
        Assert.Equivalent(createdUser, response);
    }
    
    [Fact]
    public async Task GetCandidates_ReturnsAllCandidates()
    {
        // Arrange
        User candidate1 = Fakers.FakeUser(UserType.Candidate);
        User candidate2 = Fakers.FakeUser(UserType.Candidate);
        User voter = Fakers.FakeUser(UserType.Voter);
        
        InMemoryDb.Users.Add(candidate1);
        InMemoryDb.Users.Add(candidate2);
        InMemoryDb.Users.Add(voter);
        await InMemoryDb.SaveChangesAsync();
        
        // Act
        List<User> response = await _substituteUserRepository.GetCandidates() as List<User> ?? [];
        
        // Assert
        Assert.Equal(2, response.Count);
        Assert.Contains(candidate1, response);
        Assert.Contains(candidate2, response);
        Assert.DoesNotContain(voter, response);
    }
    
    [Fact]
    public async Task CheckEmailExists_WhenEmailExists_ReturnsTrue()
    {
        // Arrange
        User fakeUser = Fakers.FakeUser(UserType.Voter);
        InMemoryDb.Users.Add(fakeUser);
        await InMemoryDb.SaveChangesAsync();
        
        // Act
        bool response = await _substituteUserRepository.CheckEmailExists(fakeUser.Email);
        
        // Assert
        Assert.True(response);
    }
    
    [Fact]
    public async Task CheckEmailExists_WhenEmailDoesNotExist_ReturnsFalse()
    {
        // Arrange
        User fakeUser = Fakers.FakeUser(UserType.Voter);
        
        // Act
        bool response = await _substituteUserRepository.CheckEmailExists(fakeUser.Email);
        
        // Assert
        Assert.False(response);
    }
    
    [Fact]
    public async Task UpdateUser_SuccessfullyUpdatesUser()
    {
        // Arrange
        User fakeUser = Fakers.FakeUser(UserType.Voter);
        InMemoryDb.Users.Add(fakeUser);
        await InMemoryDb.SaveChangesAsync();
        
        User updatedUser = fakeUser;
        updatedUser.Name = "Updated Name";
        
        // Act
        await _substituteUserRepository.UpdateUser(updatedUser);
        
        User? response = await InMemoryDb.Users.SingleOrDefaultAsync(user => user.Id == updatedUser.Id);
        
        // Assert
        Assert.NotNull(response);
        Assert.Equivalent(updatedUser, response);
    }
    
    [Fact]
    public async Task GetCandidatesOfAnElection_ReturnsOnlyCandidatesOfAnElection()
    {
        // Arrange
        Guid fakeElectionId = Guid.NewGuid();
        
        User candidate1 = Fakers.FakeUser(UserType.Candidate);
        User candidate2 = Fakers.FakeUser(UserType.Candidate);
        User voter = Fakers.FakeUser(UserType.Voter);
        
        candidate1.ParticipatingElections.Add(fakeElectionId);
        candidate2.ParticipatingElections.Add(fakeElectionId);
        voter.ParticipatingElections.Add(fakeElectionId);
        
        InMemoryDb.Users.Add(candidate1);
        InMemoryDb.Users.Add(candidate2);
        InMemoryDb.Users.Add(voter);
        await InMemoryDb.SaveChangesAsync();
        
        // Act
        List<User> response = await _substituteUserRepository
            .GetCandidatesOfAnElection(fakeElectionId) as List<User> ?? [];
        
        // Assert
        Assert.Equal(2, response.Count);
        Assert.Contains(candidate1, response);
        Assert.Contains(candidate2, response);
        Assert.DoesNotContain(voter, response);
    }
    
    [Fact]
    public async Task GetUserBySearchForElection_ReturnsAllUsersNotInElectionBySearchForElection()
    {
        // Arrange
        Guid fakeElectionId = Guid.NewGuid();
        
        User candidate1 = Fakers.FakeUser(UserType.Candidate);
        User candidate2 = Fakers.FakeUser(UserType.Candidate);
        User voter = Fakers.FakeUser(UserType.Voter);

        candidate2.Email = "test2";
        
        candidate1.ParticipatingElections.Add(fakeElectionId);
        voter.ParticipatingElections.Add(fakeElectionId);
        
        InMemoryDb.Users.Add(candidate1);
        InMemoryDb.Users.Add(candidate2);
        InMemoryDb.Users.Add(voter);
        await InMemoryDb.SaveChangesAsync();
        
        // Act
        List<User> response = await _substituteUserRepository
            .GetUserBySearchForElection("test", UserType.Candidate, fakeElectionId) as List<User> ?? [];
        
        // Assert
        Assert.Single(response);
        Assert.Contains(candidate2, response);
        Assert.DoesNotContain(voter, response);
    }
}