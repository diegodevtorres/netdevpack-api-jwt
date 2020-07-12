using Login.Lib.Enumerators;
using System.Security.Claims;

namespace Login.Setups.Interfaces
{
    public interface IClaimSetup
    {
        Claim CreateClaim(CustomClaimTypesEnum customClaimType, PermissionEnum claimValue);
    }
}
