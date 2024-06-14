using Interview_Test.Repositories.Dtos;
using MediatR;

namespace Interview_Test.Mediator.Queries;

public class GetUserByIdQuery: IRequest<UserReturnDto>
{
    public string Id { get; set; }
}