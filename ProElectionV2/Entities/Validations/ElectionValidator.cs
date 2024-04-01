using FluentValidation;

namespace ProElectionV2.Entities.Validations;

public class ElectionValidator : AbstractValidator<Election>
{
    public ElectionValidator()
    {
        RuleFor(election => election.Id)
            .NotEmpty().NotEqual(Guid.Empty).WithMessage("Id is required");
        
        RuleFor(election => election.Name)
            .NotEmpty().WithMessage("Please provide a Name");

        RuleFor(election => election.Start)
            .NotEmpty().WithMessage("Please provide a Start Date")
            .LessThan(election => election.End).WithMessage("Start Date must be before End Date");
        
        RuleFor(election => election.End)
            .NotEmpty().WithMessage("Please provide an End Date")
            .GreaterThan(election => election.Start).WithMessage("End Date must be after Start Date");
    }
}