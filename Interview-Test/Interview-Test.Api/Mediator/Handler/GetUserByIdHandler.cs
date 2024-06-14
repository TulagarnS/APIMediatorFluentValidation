using Interview_Test.Validations;
using FluentValidation.Results;
using Interview_Test.Infrastructure;
using Interview_Test.Mediator.Queries;
using Interview_Test.Models;
using Interview_Test.Repositories.Dtos;
using Interview_Test.Repositories.Interfaces;
using MediatR;

namespace Interview_Test.Mediator.Handler;

public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserReturnDto>
{
    
    private readonly InterviewTestDbContext _context;
    private readonly IUserRepository _userRepository;
    
    public GetUserByIdHandler(InterviewTestDbContext context, IUserRepository userRepository)
    {
        _context = context;
        _userRepository = userRepository;
    }
    public async Task<UserReturnDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            
            IdValidator validator = new IdValidator(_userRepository);
            ValidationResult results = validator.Validate(request);
            
            UserModel user = _userRepository.GetUserById(request.Id);
            UserReturnDto userReturn = new();
            userReturn.Id = user.Id;
            userReturn.UserId = user.UserId;
            userReturn.Username = user.Username;
            userReturn.Firstname = user.UserProfile.FirstName;
            userReturn.Lastname = user.UserProfile.LastName;
            userReturn.Age = user.UserProfile.Age.ToString();
            userReturn.Roles = user.UserRoleMappings
                .Select(selector => new UserReturnDto.RoleModelReturnDto()
                {
                    RoleId = selector.Role.RoleId,
                    RoleName = selector.Role.RoleName
                })
                .OrderBy(order=> order.RoleId)
                .ToList();
            userReturn.Permissions = user.UserRoleMappings.SelectMany(
            selectorP => selectorP.Role.Permissions.Select
                (
                    selectorPP => selectorPP.Permission)
                )
                .ToList();
            return await Task.FromResult(userReturn);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}