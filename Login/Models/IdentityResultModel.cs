using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Login.Models
{
    public class IdentityResultModel
    {
        public IdentityResultModel()
        {

        }

        public IdentityResultModel(bool succeeded, IEnumerable<IdentityError> errors)
        {
            Succeeded = succeeded;
            Errors = errors;
        }

        public IdentityResultModel(bool succeeded, IdentityError error)
        {
            Succeeded = succeeded;
            Errors = error == null ? new List<IdentityError>() : new List<IdentityError>(){ error };
        }

        public IdentityResultModel(bool succeeded, IdentityError error, object data)
        {
            Succeeded = succeeded;
            Errors = error == null ? new List<IdentityError>() : new List<IdentityError>() { error };
            Data = data;
        }

        public bool Succeeded { get; private set; }
        public IEnumerable<IdentityError> Errors { get; private set; }
        public object Data { get; private set; }
    }
}
