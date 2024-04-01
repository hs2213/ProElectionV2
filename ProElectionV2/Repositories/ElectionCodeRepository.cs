using Microsoft.EntityFrameworkCore;
using ProElectionV2.Entities;
using ProElectionV2.Persistence;
using ProElectionV2.Repositories.Interfaces;

namespace ProElectionV2.Repositories;

/// <inheritdoc/>
public class ElectionCodeRepository : IElectionCodeRepository, IAsyncDisposable
{
    private readonly ProElectionV2DbContext _dbContext;

    public ElectionCodeRepository(ProElectionV2DbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    /// <inheritdoc/>
    public virtual async Task<ElectionCode> Create(ElectionCode electionCode)
    {
        await _dbContext.ElectionCodes.AddAsync(electionCode);
        await _dbContext.SaveChangesAsync();
        return electionCode;
    }
    
    /// <inheritdoc/>
    public virtual async Task<ElectionCode?> GetById(Guid id)
    {
        return await _dbContext.ElectionCodes.SingleOrDefaultAsync(storedCode => storedCode.Id == id);
    }
    
    public virtual async Task<ElectionCode?> GetByElectionAndUser(Guid electionId, Guid userId)
    {
        return await _dbContext.ElectionCodes
            .SingleOrDefaultAsync(code => code.ElectionId == electionId && code.UserId == userId);
    }
    
    /// <inheritdoc/>
    public virtual async Task Update(ElectionCode electionCode)
    {
        _dbContext.ElectionCodes.Update(electionCode);
        await _dbContext.SaveChangesAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await _dbContext.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}