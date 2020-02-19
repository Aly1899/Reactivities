using FluentValidation;

namespace Application.Validators
{
    public static class ValidatorExtentions
    {
        public static IRuleBuilder<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            var option = ruleBuilder
                .NotEmpty()
                .MinimumLength(6).WithMessage("Password must be at least 6 characters")
                .Matches("[A-Z]").WithMessage("Password must contain at least 1 uppercase letter")
                .Matches("[a-z]").WithMessage("Password must contain at least 1 lowercase letter")
                .Matches("[0-9]").WithMessage("Password must contain at least 1 number")
                .Matches("[^a-zA-Z1-9]").WithMessage("Password must contain at least 1 non alphanumeric");
        
            return option;
        }
    }
}