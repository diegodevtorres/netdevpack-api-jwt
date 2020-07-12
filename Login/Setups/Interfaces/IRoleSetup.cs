using Login.Lib.Enumerators;
using Login.Models;
using System.Threading.Tasks;

namespace Login.Setups.Interfaces
{
    public interface IRoleSetup
    {
        Task<IdentityResultModel> AddRole(string roleName, CustomClaimTypesEnum customClaimType, PermissionEnum claimValue, string username = null);
        Task<IdentityResultModel> AddRoleWithBasicPermissions(string roleName);
        Task<IdentityResultModel> AddUserRole(string username, string roleName);
        Task<IdentityResultModel> AddUserRoleWithBasicPermissions(string username, string roleName);
        Task<IdentityResultModel> AddClaimInRole(string roleName, CustomClaimTypesEnum customClaimType, PermissionEnum permission);
    }
}
