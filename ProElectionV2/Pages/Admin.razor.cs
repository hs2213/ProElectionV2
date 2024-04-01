using FluentValidation;
using Microsoft.AspNetCore.Components;
using ProElectionV2.Entities;
using ProElectionV2.Entities.Enums;
using ProElectionV2.Entities.Validations;
using ProElectionV2.Services.Interfaces;
using ProElectionV2.Shared.ComponentBases;

namespace ProElectionV2.Pages;

public partial class Admin : LoggedInBase, IDisposable
{
    [Inject]
    private IElectionService _electionService { get; set; } = default!;
    
    private ElectionModificationType _electionModificationType = ElectionModificationType.Default;

    private UserType _userTypeToSearchFor;

    private readonly ValidationContext _electionValidationContext = new ValidationContext();
    
    private readonly ValidationContext _candidateValidationContext = new ValidationContext();
    
    private User _selectedUser = default!;
    
    private Election _selectedElection = default!;
    
    private Election _electionToAdd = GetEmptyEntity.Election();
    
    private List<Election> _elections = [];
    
    private List<User> _foundUsers = [];
    
    private string _searchQuery = string.Empty;
    
    private bool _renderSingleElection = false;

    protected override async Task OnInitializedAsync()
    {
        _elections = await _electionService.GetAllElections() as List<Election> ?? [];
        
        await base.OnInitializedAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            if (ViewingUser?.UserType != UserType.Admin)
            {
                _navigationManager.NavigateTo("/");
            }

            _renderSingleElection = true;
            StateHasChanged();
        }
    }

    private async Task CreateElection()
    {
        _electionValidationContext.Reset();
        _electionValidationContext.ValidateEvent?.Invoke();
        
        if (_electionValidationContext.State == ValidationState.Invalid)
        {
            return;
        }
        
        _electionToAdd.Id = Guid.NewGuid();
        
        Election newElection = await _electionService.CreateElection(_electionToAdd);
        
        _elections.Add(newElection);
        
        StateHasChanged();
    }

    private async Task CreateCandidate(User candidateToAdd)
    {
        _candidateValidationContext.Reset();
        _candidateValidationContext.ValidateEvent?.Invoke();
        
        if (_candidateValidationContext.State == ValidationState.Invalid)
        {
            return;
        }
        
        candidateToAdd.Id = Guid.NewGuid();
        candidateToAdd.UserType = UserType.Candidate;
        
        await UserService.CreateUser(candidateToAdd);
    }
    
    private async Task SearchUsers()
    {
        _foundUsers =
            await UserService.GetUsersByEmailSearch(_searchQuery, _userTypeToSearchFor, _selectedElection.Id) 
                as List<User> ?? [];
        
        StateHasChanged();
    }
    
    private async Task AddUserToElection()
    {
        await UserService.AddElectionToUser(_selectedUser, _selectedElection);
        _foundUsers.Remove(_selectedUser);
        StateHasChanged();
    }
    
    private void OnAddUserToElectionClicked((Election election, UserType userType) args)
    {
        _userTypeToSearchFor = args.userType;
        _selectedElection = args.election;
        _electionModificationType = ElectionModificationType.AddUserToElection;
        StateHasChanged();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}