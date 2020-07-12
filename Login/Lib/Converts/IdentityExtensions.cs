using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace Login.Lib.Converts
{
    public static class IdentityExtensions
    {
        public static IEnumerable<IdentityError> AllErrors(this ModelStateDictionary modelState)
        {
            var result = from ms in modelState
                         where ms.Value.Errors.Any()
                         let fieldKey = ms.Key
                         let errors = ms.Value.Errors
                         from error in errors
                         select new IdentityError() { Code = fieldKey, Description = error.ErrorMessage };

            return result;
        }
    }
}
