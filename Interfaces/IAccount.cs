using System.Threading.Tasks;

namespace CreditVillageBackend
{
    public interface IAccount
    {
        Task<UserRegisterResponse> UserRegisterAsync(UserRegisterRequest userRequest);

        Task<AdminRegisterResponse> AdminRegisterAsync(AdminRegisterRequest userRequest);

        Task<UserResendCodeResponse> UserResendCodeAsync(UserResendCodeRequest userRequest);

        Task<UserVerifyResponse> UserVerifyAsync(UserVerifyRequest userRequest);

        Task<UserLoginResponse> LoginAsync(UserLoginRequest userRequest);

        Task<UserForgetPasswordResponse> UserForgetPasswordAsync(UserForgetPasswordRequest userRequest);

        Task<ChangePasswordResponse> ChangePasswordAsync(string GetUserId, ChangePasswordRequest changePasswordRequest);

        Task<AppUser> GetUserAsync(string UserId);

        Task<UserUpdateResponse> UpdateUserAsync(string GetUserId, UserUpdateRequest updateRequest);
    }
}