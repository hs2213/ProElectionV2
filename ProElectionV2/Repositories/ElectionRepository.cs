using Microsoft.EntityFrameworkCore;
using ProElectionV2.Entities;
using ProElectionV2.Persistence;
using ProElectionV2.Repositories.Interfaces;

namespace ProElectionV2.Repositories;

public class ElectionRepository : IElectionRepository, IAsyncDisposable
{
    private readonly ProElectionV2DbContext _dbContext;

    public ElectionRepository(ProElectionV2DbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    /// <inheritdoc/>
    public async Task<IEnumerable<Election>> GetElections()
    {
        return await _dbContext.Elections.ToListAsync();
    }

    public async Task<IEnumerable<Election>> GetMultipleElectionsByIds(IEnumerable<Guid> ids)
    {
        return await _dbContext.Elections.Where(election => ids.Contains(election.Id)).ToListAsync();
    }
    
    /// <inheritdoc/>
    public async Task<Election?> GetElectionById(Guid id)
    {
        return await _dbContext.Elections.SingleOrDefaultAsync(election => election.Id == id);
    }
    
    /// <inheritdoc/>
    public async Task<Election> CreateElection(Election election)
    {
        await _dbContext.Elections.AddAsync(election);
        await _dbContext.SaveChangesAsync();
        return election;
    }
    
    /// <inheritdoc/>
    public async Task UpdateElection(Election election)
    {
        _dbContext.Elections.Update(election);
        await _dbContext.SaveChangesAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await _dbContext.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}