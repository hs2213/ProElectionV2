using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using ProElectionV2.Localization;
using ProElectionV2.Shared.ComponentBases;

namespace ProElectionV2.Shared.Components;

public partial class Logout : LoggedInBase, IDisposable
{
    private async Task LogOut()
    {
        await ProtectedSessionStorage.DeleteAsync("userId");
        _navigationManager.NavigateTo("/");
        StateHasChanged();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}