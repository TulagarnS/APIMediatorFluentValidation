using FluentValidation;
using Interview_Test.Mediator.Queries;
using Interview_Test.Repositories.Interfaces;

namespace Interview_Test.Validations;

public class IdValidator : AbstractValidator<GetUserByIdQuery>
{
    private IUserRepository _userRepository;
    public IdValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;
        CascadeMode = CascadeMode.Stop;
        RuleFor(x => x.Id.Replace("-", ""))
            .NotEmpty().WithMessage("User ID is required")
            .Length(32, 36).WithMessage("User ID must be the length of 32 characters (excluding dashes)");
        
        RuleFor(x => x).Custom((property, action) =>
        {
            if (_userRepository.CheckUserIfExistsByUserId(property.Id)==false)
            {
                action.AddFailure(new FluentValidation.Results.ValidationFailure()
                {
                    ErrorCode = "404",
                    ErrorMessage = "User Not Found",
                    AttemptedValue = new Dictionary<string, string>()
                    {
                        { "{0}", $"{property.Id}" }
                    }
                });
            }
        });
    }
    
}