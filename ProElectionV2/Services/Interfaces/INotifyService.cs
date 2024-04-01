using ProElectionV2.Shared.Components;

namespace ProElectionV2.Services.Interfaces;

/// <summary>
/// Used to display real time messages to users.
/// </summary>
public interface INotifyService
{
    /// <summary>
    /// Component to show message on
    /// </summary>
    public Notify? NotifyComponent { get; set; }

    /// <summary>
    /// Causes the <see cref="NotifyComponent"/> to render with the message given.
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public Task ShowNotification(string message);
}