using Microsoft.EntityFrameworkCore;
using ProElectionV2.Entities;
using ProElectionV2.Persistence;
using ProElectionV2.Repositories.Interfaces;

namespace ProElectionV2.Repositories;

public class VoteRepository : IVoteRepository, IAsyncDisposable
{
    private readonly ProElectionV2DbContext _dbContext;

    public VoteRepository(ProElectionV2DbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    /// <inheritdoc/>
    public virtual async Task Create(Vote vote)
    {
        await _dbContext.Votes.AddAsync(vote);
        await _dbContext.SaveChangesAsync();
    }
    
    /// <inheritdoc/>
    public virtual async Task<bool> CheckIfUserVotedInElection(Guid userId, Guid electionId)
    {
        return await _dbContext.Votes.AnyAsync(vote => vote.UserId == userId && vote.ElectionId == electionId);
    }
    
    /// <inheritdoc/>
    public virtual async Task<int> GetCandidateVotesByElectionId(Guid candidateId, Guid electionId)
    {
        return await _dbContext.Votes
            .CountAsync(vote => vote.CandidateId == candidateId && vote.ElectionId == electionId);
    }

    public async ValueTask DisposeAsync()
    {
        await _dbContext.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}