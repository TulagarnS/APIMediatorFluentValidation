using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Interview_Test.Repositories.Dtos;

public class UserReturnDto
{
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public string Username { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Age { get; set; }
    public List<RoleModelReturnDto> Roles { get; set; }
    public List<string> Permissions { get; set; }



    public class RoleModelReturnDto
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}