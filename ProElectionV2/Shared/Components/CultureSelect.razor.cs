using System.Globalization;
using Microsoft.AspNetCore.Components;

namespace ProElectionV2.Shared.Components;

public partial class CultureSelect : IDisposable
{
    [Inject]
    private NavigationManager NavigationManager { get; set; } = null!;

    private string Culture
    {
        get => CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }
            
            NavigationManager
                .NavigateTo($"culture/change?culture={value}&returnUrl={NavigationManager.Uri}", true);
        }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}