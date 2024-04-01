using ProElectionV2.Services.Interfaces;
using ProElectionV2.Shared.Components;

namespace ProElectionV2.Services;

public class NotifyService : INotifyService, IDisposable
{
    public Notify? NotifyComponent { get; set; }

    public async Task ShowNotification(string message)
    {
        if (NotifyComponent == null)
        {
            return;
        }
        
        await NotifyComponent.NotifyMessage(message);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}