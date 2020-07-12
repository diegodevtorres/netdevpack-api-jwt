using Login.Lib.Enumerators;
using Login.Models;
using Login.Services.Interfaces;
using Login.Setups.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using NetDevPack.Identity.Jwt;
using NetDevPack.Identity.Jwt.Model;
using System;
using System.Threading.Tasks;

namespace Login.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppJwtSettings _appJwtSettings;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IRoleSetup _roleSetup;

        public UserService(SignInManager<IdentityUser> signInManager,
                           UserManager<IdentityUser> userManager,
                           IOptions<AppJwtSettings> appJwtSettings,
                           IRoleSetup roleSetup)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appJwtSettings = appJwtSettings.Value;
            _roleSetup = roleSetup;
        }

        #region public methods
        public async Task<IdentityResultModel> CreateUser(RegisterUserModel registerUser, bool isCallFromStartUp = false)
        {
            try
            {
                var user = new IdentityUserModel(registerUser.UserName,
                                                 registerUser.Email,
                                                 registerUser.EmailConfirmed,
                                                 registerUser.FirstName,
                                                 registerUser.LastName,
                                                 registerUser.Age);

                if (registerUser.Role.GetValue().Equals("admin") && !isCallFromStartUp)
                    return new IdentityResultModel(false, new IdentityError() { Code = "RegisterError", Description = $"Não é possível cadastrar um usuário como administrador." });

                var result = await _userManager.CreateAsync(user, registerUser.Password);
                if (result.Succeeded)
                {
                    var resultUserRole = !isCallFromStartUp ? await _roleSetup.AddUserRoleWithBasicPermissions(user.UserName, registerUser.Role.GetValue()) : new IdentityResultModel(true, null, null);
                    if (resultUserRole.Succeeded)
                        return new IdentityResultModel(result.Succeeded, null, GetUserResponse(registerUser.Email));
                }

                return new IdentityResultModel(false, result.Errors);
            }
            catch (Exception ex)
            {
                return new IdentityResultModel(false, new IdentityError() { Code = "RegisterError", Description = $"Ocorreu um erro ao cadastrar o usuário: {ex.Message}" });
            }
        }

        public async Task<SignInResultModel> DoLogin(LoginUserModel loginUser, bool isPersist, bool lockoutOnFailure = false)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(loginUser.LoginWithUserName ? loginUser.Username : loginUser.Email, loginUser.Password, isPersist, lockoutOnFailure);
                if (result.Succeeded)
                {
                    var user = new IdentityUser();
                    if (loginUser.LoginWithUserName)
                        user = await _userManager.FindByNameAsync(loginUser.Username);
                    else
                        user = await _userManager.FindByEmailAsync(loginUser.Email);

                    var role = await _userManager.GetRolesAsync(user);
                    if (!role.Contains("admin"))
                    {
                        var isUserRole = role.Contains(loginUser.Role.GetValue());
                        if (!isUserRole)
                            new SignInResultModel(false, new IdentityError() { Code = "LoginError", Description = "O usuário não possui permissão para este sistema." }, result.IsLockedOut);
                    }

                    await _userManager.AddLoginAsync(user, new UserLoginInfo(loginUser.Role.GetValue(), user.Id, DateTime.Now.ToString()));
                    return new SignInResultModel(result.Succeeded, null, this.GetFullJwt(loginUser.Email));
                }

                return new SignInResultModel(false, new IdentityError() { Code = "LoginError", Description = "Usuário e/ou senha inválida." }, result.IsLockedOut);
            }
            catch (Exception ex)
            {
                return new SignInResultModel(false, new IdentityError() { Code = "LoginError", Description = $"Ocorreu um erro ao logar com o usuário: {ex.Message}" });
            }
        }
        #endregion

        #region private methods
        private UserResponse<string> GetUserResponse(string email)
        {
            var jwt = new JwtBuilder()
                      .WithUserManager(_userManager)
                      .WithJwtSettings(_appJwtSettings);

            return !string.IsNullOrEmpty(email) ? jwt.WithEmail(email).BuildUserResponse() : jwt.BuildUserResponse();
        }

        private string GetFullJwt(string email)
        {
            var jwt = new JwtBuilder()
                      .WithUserManager(_userManager)
                      .WithJwtSettings(_appJwtSettings);

            return !string.IsNullOrEmpty(email) ? jwt.WithEmail(email).BuildToken() : jwt.BuildToken();
        }
        #endregion
    }
}

