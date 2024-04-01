using FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using ProElectionV2.Entities.Enums;
using ProElectionV2.Entities.Validations;
using ProElectionV2.Localization;

namespace ProElectionV2.Shared.Components;

public partial class ValidatedInput<TEntity> : IDisposable where TEntity : class
{
    /// <summary>
    /// Dependency Injects validator for given class
    /// </summary>
    [Inject] 
    public IValidator<TEntity> _validator { get; set; } = default!;

    [Inject] 
    private IStringLocalizer<Resources> _loc { get; set; } = default!;
    
    /// <summary>
    /// Name of the property of the class to validate
    /// </summary>
    [Parameter] 
    public string PropertyName { get; set; } = string.Empty;
    
    /// <summary>
    /// Allows rendering of input inside of the validation tag.
    /// </summary>
    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    /// <summary>
    /// Used to attach the validation to the context by assigning its events to this validation.
    /// </summary>
    [Parameter]
    public ValidationContext ValidationContext { get; set; } = default!;

    /// <summary>
    /// Specifies the class being validated. 
    /// </summary>
    [Parameter]
    public TEntity Entity { get; set; } = default!;

    /// <summary>
    /// Error message to display to user
    /// </summary>
    private string _error = string.Empty;

    protected override void OnInitialized()
    {
        base.OnInitialized();

        ValidationContext.ValidateEvent += Validate;
        ValidationContext.ResetEvent += Reset;
    }

    /// <summary>
    /// Validates the property of the class given
    /// </summary>
    private void Validate()
    {
        // Gets error from the first validation error for the property
        _error = _validator.Validate(Entity, options =>
            options.IncludeProperties([PropertyName]))
                .Errors.FirstOrDefault()?.ErrorMessage
                ?? string.Empty;
        
        if (string.IsNullOrWhiteSpace(_error) == false)
        {
            ValidationContext.State = ValidationState.Invalid;
        }
    }

    /// <summary>
    /// Sets error message to empty
    /// </summary>
    private void Reset() => _error = string.Empty;
    
    public void Dispose()
    {
        ValidationContext.ValidateEvent -= Validate;
        ValidationContext.ResetEvent -= Reset;
        GC.SuppressFinalize(this);
    }
}