using Login.Lib.Enumerators;
using System.ComponentModel.DataAnnotations;

namespace Login.Models
{
    public class LoginUserModel
    {
        [EmailAddress]
        public string Email { get; set; }
        [StringLength(100, MinimumLength = 3)]
        public string Username { get; set; }
        [StringLength(100, MinimumLength = 8)]
        [Required]
        public string Password { get; set; }
        [Required]
        public bool LoginWithUserName { get; set; }
        [Required]
        public RoleEnum Role { get; set; }
    }
}
