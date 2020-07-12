using Login.Lib.Enumerators;
using NetDevPack.Identity.Model;
using System.ComponentModel.DataAnnotations;

namespace Login.Models
{
    public class RegisterUserModel : RegisterUser
    {
        public string UserName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "O campo 'Nome' é obrigatório")]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Age { get; set; }
        public bool EmailConfirmed { get; set; }
        public RoleEnum Role { get; set; }
    }
}
