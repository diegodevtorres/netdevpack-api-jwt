using Login.Lib.Enumerators;
using Login.Models;
using Login.Setups.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Login.Setups
{
    public class RoleSetup : IRoleSetup
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IClaimSetup _claimSetup;

        public RoleSetup(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IClaimSetup claimSetup)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _claimSetup = claimSetup;
        }

        public async Task<IdentityResultModel> AddRole(string roleName, CustomClaimTypesEnum customClaimType, PermissionEnum claimValue, string username = null)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                role = new IdentityRole(roleName);

                var resultRole = await _roleManager.CreateAsync(role);
                if (!resultRole.Succeeded)
                    return new IdentityResultModel(resultRole.Succeeded, resultRole.Errors);

                var resultClaim = await _roleManager.AddClaimAsync(role, new Claim(customClaimType.GetValue(), claimValue.GetValue()));
                if (!resultClaim.Succeeded)
                    return new IdentityResultModel(resultClaim.Succeeded, resultClaim.Errors);

                return new IdentityResultModel(resultRole.Succeeded, resultRole.Errors);
            }

            return new IdentityResultModel(false, new IdentityError() { Code = "RoleDuplicated", Description = "'Role' já existente." });
        }

        public async Task<IdentityResultModel> AddRoleWithBasicPermissions(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                role = new IdentityRole(roleName);

                var resultRole = await _roleManager.CreateAsync(role);
                if (!resultRole.Succeeded)
                    return new IdentityResultModel(resultRole.Succeeded, resultRole.Errors);

                var resultClaimRole = await _roleManager.AddClaimAsync(role, _claimSetup.CreateClaim(CustomClaimTypesEnum.Account, PermissionEnum.View));
                if (!resultClaimRole.Succeeded)
                    return new IdentityResultModel(resultRole.Succeeded, resultRole.Errors);

                resultClaimRole = await _roleManager.AddClaimAsync(role, _claimSetup.CreateClaim(CustomClaimTypesEnum.Account, PermissionEnum.Create));
                if (!resultClaimRole.Succeeded)
                    return new IdentityResultModel(resultRole.Succeeded, resultRole.Errors);

                resultClaimRole = await _roleManager.AddClaimAsync(role, _claimSetup.CreateClaim(CustomClaimTypesEnum.Account, PermissionEnum.Update));
                if (!resultClaimRole.Succeeded)
                    return new IdentityResultModel(resultRole.Succeeded, resultRole.Errors);
            }

            return new IdentityResultModel(false, new IdentityError() { Code = "RoleDuplicated", Description = "'Role' já existente." });
        }

        public async Task<IdentityResultModel> AddClaimInRole(string roleName, CustomClaimTypesEnum customClaimType, PermissionEnum permission)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role != null)
            {
                var resultClaimRole = await _roleManager.AddClaimAsync(role, _claimSetup.CreateClaim(customClaimType, permission));
                return new IdentityResultModel(resultClaimRole.Succeeded, resultClaimRole.Errors);
            }

            return new IdentityResultModel(false, new IdentityError() { Code = "RoleDoesNotExist", Description = "'Role' não existente." });
        }

        public async Task<IdentityResultModel> AddUserRole(string username, string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role != null)
            {
                if (!string.IsNullOrWhiteSpace(username))
                {
                    var user = await _userManager.FindByNameAsync(username);
                    if (user == null)
                        return new IdentityResultModel(false, new IdentityError() { Code = "UserDoesNotExist", Description = "Usuário '" + username + "' não existente." });

                    if (await _userManager.IsInRoleAsync(user, role.Name))
                        return new IdentityResultModel(false, new IdentityError() { Code = "UserRoleDuplicated", Description = "'Role' já inserida para o usuário '" + username + "'." });

                    var resultUser = await _userManager.AddToRoleAsync(user, role.Name);
                    return new IdentityResultModel(resultUser.Succeeded, resultUser.Errors);
                }
            }

            return new IdentityResultModel(false, new IdentityError() { Code = "RoleDoesNotExist", Description = "'Role' não existente." });
        }

        public async Task<IdentityResultModel> AddUserRoleWithBasicPermissions(string userName, string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
                return new IdentityResultModel(false, new IdentityError() { Code = "RoleNull", Description = "Elemento 'Role' não existente." });

            if (!string.IsNullOrWhiteSpace(userName))
            {
                var user = await _userManager.FindByNameAsync(userName);
                if (user == null)
                    return new IdentityResultModel(false, new IdentityError() { Code = "UserDoesNotExist", Description = "Usuário '" + userName + "' não existente." });

                if (await _userManager.IsInRoleAsync(user, role.Name))
                    return new IdentityResultModel(false, new IdentityError() { Code = "UserIsInRole", Description = "O elemento role '" + role.Name + "' já existe para este usuário." });

                var resultUserRole = await _userManager.AddToRoleAsync(user, role.Name);
                if (!resultUserRole.Succeeded)
                    return new IdentityResultModel(resultUserRole.Succeeded, resultUserRole.Errors);

                var claimList = new List<Claim>() { _claimSetup.CreateClaim(CustomClaimTypesEnum.Account, PermissionEnum.View),
                                                        _claimSetup.CreateClaim(CustomClaimTypesEnum.Account, PermissionEnum.Create),
                                                        _claimSetup.CreateClaim(CustomClaimTypesEnum.Account, PermissionEnum.Update) };

                var resultClaimsUser = await _userManager.AddClaimsAsync(user, claimList);
                if (!resultClaimsUser.Succeeded)
                    return new IdentityResultModel(resultClaimsUser.Succeeded, resultClaimsUser.Errors);

                return new IdentityResultModel(true, null, "'Role' e permissões básicas criadas com sucesso!");
            }

            return new IdentityResultModel(false, new IdentityError() { Code = "UserNameNull", Description = "O elemento 'UserName' está nulo ou vazio." });
        }
    }
}
