﻿using Microsoft.EntityFrameworkCore;
using ProElectionV2.Entities;
using ProElectionV2.Entities.Enums;
using ProElectionV2.Persistence;
using ProElectionV2.Repositories.Interfaces;

namespace ProElectionV2.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ProElectionV2DbContext _dbContext;

    public UserRepository(ProElectionV2DbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    /// <inheritdoc/>
    public async Task<User?> GetById(Guid id)
    {
        return await _dbContext.Users.SingleOrDefaultAsync(user => user.Id == id);
    }
    
    /// <inheritdoc/>
    public async Task<User?> GetUserByEmail(string email)
    {
        return await _dbContext.Users
            .SingleOrDefaultAsync(user => user.Email == email);
    }
    
    /// <inheritdoc/>
    public async Task<User> Create(User user)
    {
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        return user;
    }
    
    /// <inheritdoc/>
    public async Task<IEnumerable<User>> GetCandidates()
    {
        return await _dbContext.Users.Where(user => user.UserType == UserType.Candidate).ToListAsync();
    }
    
    /// <inheritdoc/>
    public async Task<bool> CheckEmailExists(string email)
    {
        return await _dbContext.Users.AnyAsync(user => user.Email == email);
    }
    
    /// <inheritdoc/>
    public async Task Update(User user)
    {
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
    }
    
    /// <inheritdoc/>
    public async Task<IEnumerable<User>> GetCandidatesOfAnElection(Guid electionId)
    {
        return await _dbContext.Users
            .Where(user => user.UserType == UserType.Candidate)
            .Where(candidate => candidate.ParticipatingElections.Contains(electionId))
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<User>> GetUserBySearchForElection(string searchQuery, UserType userType, Guid electionId)
    {
        return await _dbContext.Users
            .Where(user => user.UserType == userType)
            .Where(user => user.ParticipatingElections.Contains(electionId) == false)
            .Where(user => user.Email.Contains(searchQuery))
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        await _dbContext.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}