using Login.Lib.Enumerators;
using Login.Setups.Interfaces;
using System.Security.Claims;

namespace Login.Setups
{
    public class ClaimSetup : IClaimSetup
    {
        public ClaimSetup()
        {

        }
        public Claim CreateClaim(CustomClaimTypesEnum customClaimType, PermissionEnum claimValue)
        {
            return new Claim(customClaimType.GetValue(), claimValue.GetValue());
        }
    }
}
