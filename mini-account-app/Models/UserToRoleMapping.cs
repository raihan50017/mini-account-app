using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mini_account_app.Models
{
    public class UserToRoleMapping
    {
        public int Id { get; set; }


        [ForeignKey(nameof(Users))]
        public required string UserId { get; set; }
        public virtual IdentityUser? Users { get; set; }

        public List<string>? Roles { get; set; }
    }

    public class UserToRoleMappingDto
    {
        public string? UserId { get; set; }
        public string? User { get; set; }
        public string? Roles { get; set; }
    }

    
    public class UserToRoleMappingDto_Create
    {
        [Required]
        public string? UserId { get; set; }
        public string? User { get; set; }
        public List<RoleSelection> Roles { get; set; } = new();
    }
    public class RoleSelection
    {
        public string? RoleName { get; set; }
        public bool IsSelected { get; set; }
    }
}
