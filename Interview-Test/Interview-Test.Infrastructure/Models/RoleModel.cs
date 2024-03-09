﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Interview_Test.Models;

[Table("RoleTb")]
public class RoleModel
{
    [Required]
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int RoleId { get; set; }
    [Required]
    [Column(TypeName = "varchar(100)")]
    public string RoleName { get; set; }
    public ICollection<PermissionModel> Permissions { get; set; }
    public ICollection<UserRoleMappingModel> UserRoleMappings { get; set; }
}