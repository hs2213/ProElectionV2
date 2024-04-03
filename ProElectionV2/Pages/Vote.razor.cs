using Microsoft.AspNetCore.Components;
using ProElectionV2.Entities;
using ProElectionV2.Entities.Enums;
using ProElectionV2.Services.Interfaces;
using ProElectionV2.Shared.ComponentBases;

namespace ProElectionV2.Pages;

public partial class Vote : LoggedInBase, IDisposable
{
    [SupplyParameterFromQuery(Name = "Id")]
    public Guid Id { get; set; }
    
    [SupplyParameterFromQuery(Name = "IsInPerson")]
    public bool IsInPerson { get; set; }
    
    [Inject]
    private IElectionService _electionService { get; set; } = default!;
    
    private Election? _election = default!;

    private ElectionCode? _electionCode;

    private Dictionary<User, int>? _candidatesResults;

    private List<User> _candidates = [];
    
    private bool _votingDisabled = false;
    
    private bool _resultsAvailable = false;
    
    private bool _alreadyVoted = false;
    
    private bool _failedToGetElection = false;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (IsInPerson)
        {
            await ProcessElectionCode();
        }
        
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            if (IsInPerson == false)
            {
                await ProcessElectionId();
            }
            
            if (_failedToGetElection)
            {
                _navigationManager.NavigateTo("/", true);
                return;
            }
            
            if (ViewingUser?.UserType is UserType.Candidate or UserType.Admin)
            {
                _votingDisabled = true;
            }
            
            _candidates = await UserService.GetCandidatesForElection(_election!) as List<User> ?? [];
            
            if (_election!.End < DateTime.Now)
            {
                await GetResults();
                _resultsAvailable = true;
            }

            _alreadyVoted = await _electionService.CheckIfUserVoted(UserId, _election.Id);

            if (_alreadyVoted)
            {
                _votingDisabled = true;
            }
            
            StateHasChanged();
        }
    }

    private async Task ProcessElectionCode()
    {
        _electionCode = await _electionService.GetElectionCodeById(Id);

        if (_electionCode is { Status: CodeStatus.New })
        {
            UserId = _electionCode.UserId;
            ViewingUser = await UserService.GetUserById(UserId);
            _election = await _electionService.GetElectionById(_electionCode.ElectionId);

            await ProtectedSessionStorage.SetAsync("userId", UserId);
            return;
        }

        _failedToGetElection = true;
    }

    private async Task ProcessElectionId()
    {
        _election = await _electionService.GetElectionById(Id);

        if (_election == null)
        {
            _navigationManager.NavigateTo("/elections");
        }
    }
    
    private async Task GetResults()
    {
        _candidatesResults = await _electionService.CalculateResults(_election!.Id);
    }
    
    private async Task PlaceVote(User candidate)
    {
        if (_votingDisabled || _resultsAvailable)
        {
            _navigationManager.NavigateTo("/elections");
            return;
        }
        
        Entities.Vote vote = new Entities.Vote
        {
            CandidateId = candidate.Id,
            ElectionId = _election!.Id,
            UserId = UserId,
            Id = Guid.NewGuid(),
            Time = DateTimeOffset.Now
        };
        
        await _electionService.Vote(vote);
        
        _votingDisabled = true;

        if (IsInPerson)
        {
            await _electionService.MarkElectionCodeAsUsed(_electionCode!);
        }
        
        _navigationManager.NavigateTo("/elections");
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}