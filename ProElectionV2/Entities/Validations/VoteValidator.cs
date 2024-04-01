using FluentValidation;

namespace ProElectionV2.Entities.Validations;

public class VoteValidator : UserElectionAssociationValidator<Vote>
{
    public VoteValidator()
    {
        RuleFor(vote => vote.CandidateId)
            .NotEmpty().NotEqual(Guid.Empty).WithMessage("Id is required");

        RuleFor(vote => vote.Time)
            .NotEmpty().WithMessage("Time of Vote is required")
            .GreaterThanOrEqualTo(DateTimeOffset.Now).WithMessage("Vote Cannot be cast in the past");
    }
}