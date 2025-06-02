using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mini_account_app.Models
{
    public class UserToModulePermission
    {
        public int Id { get; set; }


        [ForeignKey(nameof(Users))]
        public required string UserId { get; set; }
        public virtual IdentityUser? Users { get; set; }

        public string? Module { get; set; }
    }

}
