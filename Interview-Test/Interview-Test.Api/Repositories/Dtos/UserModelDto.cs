using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Interview_Test.Repositories.Dtos;

public class UserModelDto
{
    public Guid Id { get; set; }
    [Required]
    [Column(TypeName = "varchar(20)")]
    public string UserId { get; set; }
    [Required]
    [Column(TypeName = "varchar(100)")]
    public string Username { get; set; }
    public UserProfileModelDto UserProfile { get; set; }
    public List<UserRoleMappingDto> UserRoleMappings { get; set; }
}

public class UserProfileModelDto
{
    public Guid ProfileId { get; set; }
    [Required]
    [Column(TypeName = "varchar(100)")]
    public string FirstName { get; set; }
    [Required]
    [Column(TypeName = "varchar(100)")]
    public string LastName { get; set; }
    public int? Age { get; set; }
}

public class UserRoleMappingDto
{
    [ForeignKey("Id")]
    public UserModelDto User { get; set; }
    [ForeignKey("RoleId")]
    public RoleModelDto Role { get; set; }
}

public class RoleModelDto
{
    [Required]
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int RoleId { get; set; }
    [Required]
    [Column(TypeName = "varchar(100)")]
    public string RoleName { get; set; }
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public ICollection<PermissionModelDto> Permissions { get; set; }
    // public ICollection<UserRoleMappingModel> UserRoleMappings { get; set; }
}

public class PermissionModelDto
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long PermissionId { get; set; }
    [Required]
    [Column(TypeName = "text")]
    public string Permission { get; set; }
    // [ForeignKey("RoleId")]
    // public RoleModel Role { get; set; }
}