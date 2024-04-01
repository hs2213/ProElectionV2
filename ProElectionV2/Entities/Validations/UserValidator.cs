using FluentValidation;

namespace ProElectionV2.Entities.Validations;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(user => user.Id)
            .NotEmpty().NotEqual(Guid.Empty).WithMessage("Id is required");
        
        RuleFor(user => user.Name)
            .NotEmpty().WithMessage("Please provide a Name");

        RuleFor(user => user.PhoneNumber)
            .NotEmpty().WithMessage("Please provide a Phone Number")
            .Custom((number, context) =>
            {
                // Removes the '+' sign from the phone number.
                number = number.Replace("+", "");
                
                // If the phone number is not a number or
                // it has less than 3 digits (the smallest global phone number length) or
                // it has more than 14 digits (1 more than the largest global number) it's invalid.
                if ((int.TryParse(number, out int numberAsInt)) == false 
                    || number.Length < 3
                    || number.Length > 15)
                {
                    context.AddFailure("Phone number is invalid");
                }
            });
        
        RuleFor(user => user.Email)
            .NotEmpty().WithMessage("Please provide an Email")
            .EmailAddress().WithMessage("Email is invalid");
        
        RuleFor(user => user.Address)
            .NotEmpty().WithMessage("Please provide an Address");

        RuleFor(user => user.Postcode)
            .NotEmpty().WithMessage("Please provide a Postcode")
            // The Longest Global Postcode is 10 characters long.
            .MaximumLength(10).WithMessage("Postcode is Invalid");
        
        RuleFor(user => user.Country)
            .NotEmpty().WithMessage("Please provide a Country");

        RuleFor(user => user.HashedPassword)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long")
            .MaximumLength(20).WithMessage("Password must be at most 20 characters long")
            .Matches(@"[A-Z]+").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]+").WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"[0-9]+").WithMessage("Password must contain at least one number.");
    }
}