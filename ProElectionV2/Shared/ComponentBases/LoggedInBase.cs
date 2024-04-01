using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.Extensions.Localization;
using ProElectionV2.Entities;
using ProElectionV2.Localization;
using ProElectionV2.Services.Interfaces;

namespace ProElectionV2.Shared.ComponentBases;

public class LoggedInBase : ComponentBase
{
    [Inject]
    protected ProtectedSessionStorage ProtectedSessionStorage { get; set; } = null!;

    [Inject]
    protected NavigationManager _navigationManager { get; set; } = null!;

    [Inject]
    protected IUserService UserService { get; set; } = null!;

    [Inject]
    protected IStringLocalizer<Resources> _loc { get; set; } = null!;
    
    protected Guid UserId { get; set; }
    
    protected User? ViewingUser { get; set; }
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            ProtectedBrowserStorageResult<Guid> userIdResult = 
                await ProtectedSessionStorage.GetAsync<Guid>("userId");

            if (userIdResult.Success == false)
            {
                _navigationManager.NavigateTo("/");
                return;
            }
            
            UserId = userIdResult.Value;
            await GetUser();
            
            StateHasChanged();
        }
        
        await base.OnAfterRenderAsync(firstRender);
    }

    private async Task GetUser()
    {
        if (UserId == null)
        {
            return;
        }
            
        ViewingUser = await UserService.GetUserById(UserId);
    }
}