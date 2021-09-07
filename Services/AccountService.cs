using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CreditVillageBackend
{
    public class AccountService : IAccount
    {
        private readonly UserManager<AppUser> _userManager;

        private readonly AppSettings _appSettings;

        public AccountService(UserManager<AppUser> userManager,
                                IOptions<AppSettings> appSettings)
        {
            _userManager = userManager;
            _appSettings = appSettings.Value;
        }

        public async Task<UserRegisterResponse> UserRegisterAsync(UserRegisterRequest userRequest)
        {
            var existingEmail = await _userManager.FindByEmailAsync(userRequest.Email);

            if (existingEmail != null)
            {
                return new UserRegisterResponse()
                {
                    Check = false,
                    Status = "success",
                    Message = "This Email Address is Already Used"
                };
            }

            var newUser = new AppUser
            {
                Full_Name = userRequest.Full_Name,
                Email = userRequest.Email,
                UserName = userRequest.Email,
                Joined_Date = DateTime.Now,
                StateId = userRequest.State,
                NationalityId = userRequest.Nationality
            };

            var result = await _userManager.CreateAsync(newUser, userRequest.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, "User");

                string code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);

                return new UserRegisterResponse()
                {
                    Check = true,
                    Status = "success",
                    Message = "Account Created Successfully. Please Check Your Email to Confirm Account",
                    UserId = newUser.Id,
                    Email = newUser.Email,
                    Code = code
                };
            }

            return new UserRegisterResponse()
            {
                Check = false,
                Status = "error",
                Message = result.Errors.LastOrDefault().Description
            };
        }

        
        public async Task<AdminRegisterResponse> AdminRegisterAsync(AdminRegisterRequest userRequest)
        {
            var existingEmail = await _userManager.FindByEmailAsync(userRequest.Email);

            if (existingEmail != null)
            {
                return new AdminRegisterResponse()
                {
                    Check = false,
                    Status = "success",
                    Message = "This Email Address is Already Used"
                };
            }

            var newUser = new AppUser
            {
                Full_Name = userRequest.Full_Name,
                Email = userRequest.Email,
                UserName = userRequest.Email,
                Joined_Date = DateTime.Now,
                StateId = userRequest.State,
                NationalityId = userRequest.Nationality
            };

            var result = await _userManager.CreateAsync(newUser, userRequest.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, "Admin");

                string code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);

                return new AdminRegisterResponse()
                {
                    Check = true,
                    Status = "success",
                    Message = "Account Created Successfully. Please Check Your Email to Confirm Account",
                    UserId = newUser.Id,
                    Email = newUser.Email,
                    Code = code
                };
            }

            return new AdminRegisterResponse()
            {
                Check = false,
                Message = result.Errors.LastOrDefault().Description
            };
        }


        public async Task<UserLoginResponse> LoginAsync(UserLoginRequest userRequest)
        {
            var existUser = await _userManager.FindByNameAsync(userRequest.Email);

            if (existUser == null)
            {
                return new UserLoginResponse()
                {
                    Check = false,
                    Status = "success",
                    Message = "Email doesn't Exist"
                };
            }

            var checkPassword = await _userManager.CheckPasswordAsync(existUser, userRequest.Password);

            if (!checkPassword)
            {
                return new UserLoginResponse()
                {
                    Check = false,
                    Status = "success",
                    Message = "InCorrect Password"
                };
            }

            var emailConfirmed = await _userManager.IsEmailConfirmedAsync(existUser);

            if (!emailConfirmed)
            {
                return new UserLoginResponse()
                {
                    Check = false,
                    Status = "success",
                    Message = "Your Email Address has not been confirm. Kindly Check Your Email Address"
                };
            }

            return await GenerateUserToken(existUser);
        }

        private async Task<UserLoginResponse> GenerateUserToken(AppUser user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret));

            double tokenExpiryTime = Convert.ToDouble(_appSettings.ExpireTime);

            var Expires = DateTime.UtcNow.AddDays(tokenExpiryTime);

            var tokenHandler = new JwtSecurityTokenHandler();

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("CreditVillage", "Know Your Credit Score"),
                new Claim(ClaimTypes.Role, roles.FirstOrDefault()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _appSettings.Site,
                audience: _appSettings.Audience,
                claims: claims,
                expires: Expires,
                signingCredentials: creds);

            return new UserLoginResponse()
            {
                Token = tokenHandler.WriteToken(token),
                Expiration = Expires,
                Check = true,
                Status = "success",
                Message = "successfully login"
            };
        }
    }
}