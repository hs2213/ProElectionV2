using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using ProElectionV2.Entities;
using ProElectionV2.Entities.Enums;
using ProElectionV2.Entities.Validations;
using ProElectionV2.Localization;

namespace ProElectionV2.Shared.Components;

public partial class AddUser : IDisposable
{
    [Parameter]
    public EventCallback<User> OnUserCreated { get; set; }

    [Parameter] 
    public string Label { get; set; } = string.Empty;

    [Parameter] 
    public string Class { get; set; } = string.Empty;
    
    [Parameter]
    public UserType UserType { get; set; }

    [Inject] 
    private IStringLocalizer<Resources> _loc { get; set; } = default!;

    private ValidationContext _userContext = new ValidationContext();
    
    private User _user = GetEntity.User(UserType.Voter);

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        _user.UserType = UserType;
    }

    private async Task CreateUser()
    {
        _userContext.Reset();
        _userContext.ValidateEvent?.Invoke();

        if (_userContext.State == ValidationState.Invalid)
        {
            return;
        }

        await OnUserCreated.InvokeAsync(_user);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}