﻿namespace ProElection.Tests.Unit.Repositories;

public class VoteRepositoryTests : DbFaker
{
    private readonly IVoteRepository _substituteVoteRepository;

    public VoteRepositoryTests()
    {
        _substituteVoteRepository = Substitute.For<VoteRepository>(InMemoryDb);
    }
    
    [Fact]
    public async Task Create_Vote_ShouldAddVoteToDb()
    {
        // Arrange
        Vote vote = Fakers.FakeVote();
        
        // Act
        await _substituteVoteRepository.Create(vote);
        Vote? voteInDb = await InMemoryDb.Votes.FirstOrDefaultAsync(v => v.Id == vote.Id);
        
        // Assert
        Assert.NotNull(voteInDb);
        Assert.Equivalent(vote, voteInDb);
    }

    [Fact]
    public async Task CheckIfUserVotedInElection_UserVoted_ShouldReturnTrue()
    {
        // Arrange
        Vote vote = Fakers.FakeVote();
        await InMemoryDb.Votes.AddAsync(vote);
        await InMemoryDb.SaveChangesAsync();
        
        // Act
        bool result = await _substituteVoteRepository.CheckIfUserVotedInElection(vote.UserId, vote.ElectionId);
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public async Task CheckIfUserVotedInElection_UserNotVoted_ShouldReturnFalse()
    {
        // Arrange
        Vote vote = Fakers.FakeVote();
        
        // Act
        bool result = await _substituteVoteRepository.CheckIfUserVotedInElection(vote.UserId, vote.ElectionId);
        
        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public async Task GetCandidateVotesByElectionId_ShouldReturnVotesCount()
    {
        // Arrange
        Guid electionId = Guid.NewGuid();
        Guid candidateId = Guid.NewGuid();
        
        Vote vote1 = Fakers.FakeVote();
        vote1.ElectionId = electionId;
        vote1.CandidateId = candidateId;
        
        Vote vote2 = Fakers.FakeVote();
        vote2.ElectionId = electionId;
        vote2.CandidateId = candidateId;
        
        InMemoryDb.Votes.AddRange([vote1, vote2]);
        await InMemoryDb.SaveChangesAsync();
        
        // Act
        int result = await _substituteVoteRepository.GetCandidateVotesByElectionId(candidateId, electionId);
        
        // Assert
        Assert.Equal(2, result);
    }
}