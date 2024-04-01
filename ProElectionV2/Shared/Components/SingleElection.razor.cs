using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using ProElectionV2.Entities;
using ProElectionV2.Entities.Enums;
using ProElectionV2.Localization;
using ProElectionV2.Services.Interfaces;

namespace ProElectionV2.Shared.Components;

public partial class SingleElection : IDisposable
{
    [Parameter] 
    public Election Election { get; set; } = default!;

    [Parameter]
    public User ViewingUser { get; set; } = default!;
    
    [Parameter]
    public EventCallback<(Election, UserType)> OnAddUserToElection { get; set; } = default!;
    
    [Inject]
    private NavigationManager _navigationManager { get; set; } = default!;
    
    [Inject]
    private IElectionService _electionService { get; set; } = default!;
    
    [Inject]
    private IStringLocalizer<Resources> _loc { get; set; } = default!;
    
    private string _electionCode = string.Empty;
    
    private void NavigateToVotePage()
    {
        _navigationManager.NavigateTo($"/vote?Id={Election.Id}&IsInPerson=false");
    }
    
    private void AddUserToElection(UserType userType)
    {
        if (Election.End < DateTime.Now)
        {
            return;
        }
        
        OnAddUserToElection.InvokeAsync((Election, userType));
    }

    private async Task GetElectionCode()
    {
        if (Election.End < DateTime.Now)
        {
            return;
        }
        
        if ((ViewingUser.UserType == UserType.Voter && Election.End > DateTime.Now) == false)
        {
            return;
        }
        
        ElectionCode electionCodeResponse = 
            await _electionService.GetElectionCode(ViewingUser.Id, Election.Id);
        _electionCode = electionCodeResponse.Id.ToString();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}