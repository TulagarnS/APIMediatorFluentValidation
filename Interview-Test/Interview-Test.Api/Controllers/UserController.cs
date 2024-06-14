using Interview_Test.Mediator.Queries;
using Interview_Test.Models;
using Interview_Test.Repositories.Dtos;
using Interview_Test.Repositories.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Utility.Package.UtilityServices;
using FluentValidation;
using Utility.Package.Extensions;

namespace Interview_Test.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerExtension
{
    private IUserRepository _userRepository;
    private readonly IMediator _mediator;

    public UserController(IMediator mediator, IUserRepository userRepository, ILogger<UserController> logger) : base(logger)
    {
        _mediator = mediator;
        _userRepository = userRepository; 
    }
    
    [HttpGet("GetUserById/{id}")]
    public async Task<ActionResult> GetUserById(string id)
    {
        try
        {
            var query = new GetUserByIdQuery { Id = id };
            UserReturnDto userReturn = await _mediator.Send(query);
            return Ok(userReturn);
        }
        catch (Exception e)
        {
            if(e is ValidationException)throw;
            Console.WriteLine(e);
            return InternalServerErrorCore(GetActualAsyncMethodName(),e);
        }
    }
    
    [HttpPost("CreateUser")]
    public ActionResult CreateUser(UserModelDto user)
    {
        var userModel = MappingService.Map<UserModelDto, UserModel>(user);
        MappingService.MapProp(userModel.UserProfile, user.UserProfile);
        
        userModel.UserRoleMappings = new List<UserRoleMappingModel>();
        foreach (var userRoleMapping in user.UserRoleMappings)
        {
            UserRoleMappingModel userRoleMappingModel = new UserRoleMappingModel();
            userRoleMappingModel.User = new();
            userRoleMappingModel.Role = new();
            userRoleMappingModel.Role.Permissions = new List<PermissionModel>();
            userRoleMappingModel.User = MappingService.Map<UserModelDto, UserModel>(userRoleMapping.User);
            userRoleMappingModel.Role = MappingService.Map<RoleModelDto, RoleModel>(userRoleMapping.Role);
            userModel.UserRoleMappings.Add(userRoleMappingModel);
        }
        // === MappingServices from Utility.Package
        // MappingService.MapProp(userModel.UserRoleMappings, user.UserRoleMappings);
        // MappingService.MapProp(user.UserRoleMappings, user.UserRoleMappings);
        
        _userRepository.CreateUser(userModel);
        return Ok();
    }
}