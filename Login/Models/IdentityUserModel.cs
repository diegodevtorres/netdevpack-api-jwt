using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Login.Models
{
    public class IdentityUserModel : IdentityUser
    {
        public IdentityUserModel()
        {

        }

        public IdentityUserModel(string userName, string email, bool emailConfirmed, string firstName, string lastName, int? age)
            :base(userName)
        {
            Email = email;
            EmailConfirmed = emailConfirmed;
            FirstName = firstName;
            LastName = lastName;
            Age = age;
        }

        [Column(TypeName = "nvarchar(150)")]
        public string FirstName { get; private set; }
        [Column(TypeName = "nvarchar(150)")]
        public string LastName { get; private set; }
        [Column(TypeName = "smallint")]
        public int? Age { get; private set; }
    }
}
