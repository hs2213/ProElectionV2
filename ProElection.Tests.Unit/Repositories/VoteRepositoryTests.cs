using ProElection.Tests.Unit.Fakers;

namespace ProElection.Tests.Unit.Repositories;

public class VoteRepositoryTests : DbFaker
{
    private readonly IVoteRepository _sut;

    public VoteRepositoryTests()
    {
        _sut = new VoteRepository(InMemoryDb);
    }
    
    [Fact]
    public async Task Create_Vote_ShouldAddVoteToDb()
    {
        // Arrange
        Vote vote = Fakers.Fakers.FakeVote();
        
        // Act
        await _sut.Create(vote);
        Vote? voteInDb = await InMemoryDb.Votes.FirstOrDefaultAsync(v => v.Id == vote.Id);
        
        // Assert
        Assert.NotNull(voteInDb);
        Assert.Equivalent(vote, voteInDb);
    }

    [Fact]
    public async Task CheckIfUserVotedInElection_UserVoted_ShouldReturnTrue()
    {
        // Arrange.
        User voter = Fakers.Fakers.FakeUser(UserType.Voter);
        
        Vote vote = Fakers.Fakers.FakeVote();
        vote.UserId = voter.Id;
        await InMemoryDb.Votes.AddAsync(vote);
        await InMemoryDb.SaveChangesAsync();
        
        // Act
        bool result = await _sut.CheckIfUserVotedInElection(voter.Id, vote.ElectionId);
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public async Task CheckIfUserVotedInElection_UserNotVoted_ShouldReturnFalse()
    {
        // Arrange
        Vote vote = Fakers.Fakers.FakeVote();
        
        // Act
        bool result = await _sut.CheckIfUserVotedInElection(vote.UserId, vote.ElectionId);
        
        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public async Task GetCandidateVotesByElectionId_ShouldReturnVotesCount()
    {
        // Arrange
        Guid electionId = Guid.NewGuid();
        Guid candidateId = Guid.NewGuid();
        
        Vote vote1 = Fakers.Fakers.FakeVote();
        vote1.ElectionId = electionId;
        vote1.CandidateId = candidateId;
        
        Vote vote2 = Fakers.Fakers.FakeVote();
        vote2.ElectionId = electionId;
        vote2.CandidateId = candidateId;
        
        InMemoryDb.Votes.AddRange([vote1, vote2]);
        await InMemoryDb.SaveChangesAsync();
        
        // Act
        int result = await _sut.GetCandidateVotesByElectionId(candidateId, electionId);
        
        // Assert
        Assert.Equal(2, result);
    }
}