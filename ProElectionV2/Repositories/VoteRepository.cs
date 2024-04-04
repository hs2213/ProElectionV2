using Microsoft.EntityFrameworkCore;
using ProElectionV2.Entities;
using ProElectionV2.Persistence;
using ProElectionV2.Repositories.Interfaces;

namespace ProElectionV2.Repositories;

public class VoteRepository : IVoteRepository
{
    private readonly ProElectionV2DbContext _dbContext;

    public VoteRepository(ProElectionV2DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc/>
    public async Task<Vote> Create(Vote vote)
    {
        await _dbContext.Votes.AddAsync(vote);
        await _dbContext.SaveChangesAsync();

        return vote;
    }
    
    /// <inheritdoc/>
    public async Task<bool> CheckIfUserVotedInElection(Guid userId, Guid electionId)
    {
        List<Vote> matches = await _dbContext.Votes
            .Where(vote => vote.UserId == userId && vote.ElectionId == electionId).ToListAsync();
        return matches.Count > 0;
    }
    
    /// <inheritdoc/>
    public async Task<int> GetCandidateVotesByElectionId(Guid candidateId, Guid electionId)
    {
        return await _dbContext.Votes
            .CountAsync(vote => vote.CandidateId == candidateId && vote.ElectionId == electionId);
    }
    
    public Task<Vote?> GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task Update(Vote obj)
    {
        throw new NotImplementedException();
    }

    public async ValueTask DisposeAsync()
    {
        await _dbContext.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}