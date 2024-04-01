using FluentValidation;

namespace ProElectionV2.Entities.Validations;

public class UserElectionAssociationValidator<T> : AbstractValidator<T> where T : UserElectionAssociation
{
    protected UserElectionAssociationValidator()
    {
        RuleFor(association => association.Id)
            .NotEqual(Guid.Empty).NotEmpty().WithMessage("Id is required");
        
        RuleFor(association => association.ElectionId)
            .NotEqual(Guid.Empty).NotEmpty().WithMessage("Election Id is required");
        
        RuleFor(association => association.UserId)
            .NotEqual(Guid.Empty).NotEmpty().WithMessage("User Id is required");
    }
}