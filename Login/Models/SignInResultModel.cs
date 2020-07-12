using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Login.Models
{
    public class SignInResultModel : IdentityResultModel
    {
        public SignInResultModel(bool succeeded, IEnumerable<IdentityError> errors)
            : base(succeeded, errors)
        {
        }

        public SignInResultModel(bool succeeded, IdentityError error)
            : base(succeeded, error)
        {
        }

        public SignInResultModel(bool succeeded, IdentityError error, object data)
            : base(succeeded, error, data)
        {
        }

        public SignInResultModel(bool succeeded, IdentityError error, object data, bool isLockedOut)
            :base(succeeded, error, data)
        {
            IsLockedOut = isLockedOut;
        }


        public bool IsLockedOut { get; private set; }
    }
}
