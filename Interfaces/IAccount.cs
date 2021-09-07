using System.Threading.Tasks;

namespace CreditVillageBackend
{
    public interface IAccount
    {
        Task<UserRegisterResponse> UserRegisterAsync(UserRegisterRequest userRequest);

        Task<AdminRegisterResponse> AdminRegisterAsync(AdminRegisterRequest userRequest);

        Task<UserLoginResponse> LoginAsync(UserLoginRequest userRequest);

        Task<ChangePasswordResponse> ChangePasswordAsync(string GetUserId, ChangePasswordRequest changePasswordRequest);

        Task<AppUser> GetUserAsync(string UserId);

        Task<UserUpdateResponse> UpdateUserAsync(string GetUserId, UserUpdateRequest updateRequest);
    }
}