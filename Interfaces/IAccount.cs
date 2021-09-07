using System.Threading.Tasks;

namespace CreditVillageBackend
{
    public interface IAccount
    {
        Task<UserRegisterResponse> UserRegisterAsync(UserRegisterRequest userRequest);

        Task<AdminRegisterResponse> AdminRegisterAsync(AdminRegisterRequest userRequest);

        Task<UserLoginResponse> LoginAsync(UserLoginRequest userRequest);
    }
}