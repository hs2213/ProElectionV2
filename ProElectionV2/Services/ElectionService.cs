using FluentValidation;
using ProElectionV2.Entities;
using ProElectionV2.Entities.Enums;
using ProElectionV2.Repositories.Interfaces;
using ProElectionV2.Services.Interfaces;

namespace ProElectionV2.Services;

public sealed class ElectionService : IElectionService, IDisposable
{
    private readonly IElectionRepository _electionRepository;
    private readonly IElectionCodeRepository _electionCodeRepository;
    private readonly IVoteRepository _voteRepository;
    private readonly IUserRepository _userRepository;
    
    private readonly INotifyService _notifyService;
    
    private readonly IValidator<ElectionCode> _electionCodeValidator;
    private readonly IValidator<Election> _electionValidator;
    private readonly IValidator<Vote> _voteValidator;

    public ElectionService(
        IElectionRepository electionRepository, 
        IElectionCodeRepository electionCodeRepository, 
        IVoteRepository voteRepository,
        IUserRepository userRepository, 
        IValidator<ElectionCode> electionCodeValidator, 
        IValidator<Election> electionValidator,
        IValidator<Vote> voteValidator,
        INotifyService notifyService)
    {
        _electionRepository = electionRepository;
        _electionCodeRepository = electionCodeRepository;
        _voteRepository = voteRepository;
        _userRepository = userRepository;
        _electionCodeValidator = electionCodeValidator;
        _electionValidator = electionValidator;
        _voteValidator = voteValidator;
        _notifyService = notifyService;
    }
    
    /// <inheritdoc/>
    public async Task<Election> CreateElection(Election election)
    {
        await _electionValidator.ValidateAndThrowAsync(election);
        Election electionCreated = await _electionRepository.Create(election);
        await _notifyService.ShowNotification("Election Created");
        return electionCreated;
    }
    
    /// <inheritdoc/>
    public async Task<Election?> GetElectionById(Guid electionId)
    {
        return await _electionRepository.GetById(electionId);
    }
    
    /// <inheritdoc/>
    public async Task<IEnumerable<Election>> GetAllElections()
    {
        return await _electionRepository.GetElections();
    }
    
    /// <inheritdoc/>
    public async Task<IEnumerable<Election>> GetElectionsByMultipleIds(IEnumerable<Guid> electionIds)
    {
        return await _electionRepository.GetMultipleElectionsByIds(electionIds);
    }

    /// <inheritdoc/>
    public async Task<ElectionCode?> GetElectionCodeById(Guid electionCodeId)
    {
        ElectionCode? electionCode = await _electionCodeRepository.GetById(electionCodeId);

        if (electionCode == null)
        {
            await _notifyService.ShowNotification("Failed to get election from code. Please try again");
            return null;
        }

        if (electionCode!.Status == CodeStatus.Used)
        {
            await _notifyService.ShowNotification("Code has already been used.");
            return electionCode;
        }

        Election? electionAssociated = await GetElectionById(electionCode.ElectionId);

        if (electionAssociated ==  null || electionAssociated.End < DateTime.Now)
        {
            await _notifyService.ShowNotification("Election has ended or does not exist.");
            return null;
        }

        return electionCode;
    }
    
    /// <inheritdoc/>
    public async Task<ElectionCode> GetElectionCodeByUserAndElection(Guid electionId, Guid userId)
    {
        ElectionCode? electionCode = await _electionCodeRepository.GetByElectionAndUser(electionId, userId);

        if (electionCode != null)
        {
            return electionCode;
        }
        
        electionCode = new ElectionCode
        {
            ElectionId = electionId,
            UserId = userId,
            Id = Guid.NewGuid(),
            Status = CodeStatus.New
        };

        await _electionCodeValidator.ValidateAndThrowAsync(electionCode);
        
        return await _electionCodeRepository.Create(electionCode);
    }

    /// <inheritdoc/>
    public async Task Vote(Vote vote)
    {
        await _voteValidator.ValidateAndThrowAsync(vote);
        
        await _voteRepository.Create(vote);

        await _notifyService.ShowNotification("Vote Sent");
    } 
    
    /// <inheritdoc/>
    public async Task MarkElectionCodeAsUsed(ElectionCode electionCode)
    {
        electionCode.Status = CodeStatus.Used;
        
        await _electionCodeRepository.Update(electionCode);

        await _notifyService.ShowNotification("Election Code Marked as Used");
    }
    
    /// <inheritdoc/>
    public async Task<bool> CheckIfUserVoted(Guid electionId, Guid userId) =>
        await _voteRepository.CheckIfUserVotedInElection(electionId, userId);

    /// <inheritdoc/>
    public async Task<Dictionary<User, int>?> CalculateResults(Guid electionId)
    {
        Election? election = await GetElectionById(electionId);

        if (election == null)
        {
            return null;
        }

        Dictionary<User, int> candidatesWithVoteCounts = await GetCandidatesWithVoteCounts(election);

        // Orders the candidates by the number of votes they got in ascending order
        return candidatesWithVoteCounts
            .OrderByDescending(candidateWithVoteCount => candidateWithVoteCount.Value)
            .ToDictionary(
                candidateWithVoteCount => candidateWithVoteCount.Key,
                candidateWithVoteCount => candidateWithVoteCount.Value);
    }

    
    /// <summary>
    /// Creates a dictionary where a key is a candidate and its value is the number of votes they got.
    /// </summary>
    /// <param name="election">Election to get vote count from</param>
    /// <returns><see cref="Dictionary{TKey,TValue}"/> where the key is a candidate
    /// and the value is the number of votes they got</returns>
    private async Task<Dictionary<User, int>> GetCandidatesWithVoteCounts(Election election)
    {
        Dictionary<User, int> candidatesWithVoteCount = new Dictionary<User, int>();
        
        IEnumerable<User> candidates = await _userRepository.GetCandidatesOfAnElection(election.Id);
        
        foreach (User candidate in candidates)
        {
            int noOfVotes = await _voteRepository.GetCandidateVotesByElectionId(candidate.Id, election.Id);
            
            candidatesWithVoteCount.Add(candidate, noOfVotes);
        }

        return candidatesWithVoteCount;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}