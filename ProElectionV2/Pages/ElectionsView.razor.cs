using Microsoft.AspNetCore.Components;
using ProElectionV2.Entities;
using ProElectionV2.Entities.Enums;
using ProElectionV2.Services.Interfaces;
using ProElectionV2.Shared.ComponentBases;

namespace ProElectionV2.Pages;

public partial class ElectionsView : LoggedInBase, IDisposable
{
    private List<Election> _elections = [];
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            _elections = await UserService.GetUserElections(UserId) as List<Election> ?? [];
            
            StateHasChanged();
        }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}