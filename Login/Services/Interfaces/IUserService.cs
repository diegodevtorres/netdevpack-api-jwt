using Login.Models;
using System.Threading.Tasks;

namespace Login.Services.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResultModel> CreateUser(RegisterUserModel registerUser, bool isCallFromStartUp = false);
        Task<SignInResultModel> DoLogin(LoginUserModel loginUser, bool isPersist, bool lockouOnFailure);        
    }
}
