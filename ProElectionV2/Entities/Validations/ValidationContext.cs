using ProElectionV2.Entities.Enums;

namespace ProElectionV2.Entities.Validations;

/// <summary>
/// Entity that groups multiple property validations together
/// This is used so that an object's properties can all be validated at the same time if attached to this context.
/// </summary>
public class ValidationContext
{
    /// <summary>
    /// Event that should be used to reset any error messages generated through validation
    /// </summary>
    public Action? ResetEvent { get; set; }

    /// <summary>
    /// Event that should be used to trigger the validation of a property
    /// </summary>
    public Action? ValidateEvent { get; set; }

    /// <summary>
    /// Property which shows if all validated objects in the context are valid or not
    /// </summary>
    public ValidationState State { get; set; } = ValidationState.Valid;

    /// <summary>
    /// Resets the validation state and invokes the reset event.
    /// </summary>
    public void Reset()
    {
        State = ValidationState.Valid;
        ResetEvent?.Invoke();
    }
}