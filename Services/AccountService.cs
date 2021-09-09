using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

                string code = await _userManager.GenerateTwoFactorTokenAsync(newUser, "Email");

                return new UserRegisterResponse()
                {
                    Check = true,
                    Status = "success",
                    Message = $"Account Created Successfully. Please Check Your Email to Confirm Account { code }",
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

                string code = await _userManager.GenerateTwoFactorTokenAsync(newUser, "Email" );

                return new AdminRegisterResponse()
                {
                    Check = true,
                    Status = "success",
                    Message = $"Account Created Successfully. Please Check Your Email to Confirm Account { code }",
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


        public async Task<UserVerifyResponse> UserVerifyAsync(UserVerifyRequest userRequest)
        {
            var existUser = await _userManager.FindByEmailAsync(userRequest.Email);

            if (existUser == null)
            {
                return new UserVerifyResponse()
                {
                    Check = false,
                    Status = "success",
                    Message = "This Email Address doesn't exist"
                };
            }

            bool result = await _userManager.VerifyTwoFactorTokenAsync(existUser, "Email", userRequest.Token);

            if (!result)
            {
                return new UserVerifyResponse()
                {
                    Check = false,
                    Status = "success",
                    Message = "Invalid token"
                };
            }
            else
            {
                existUser.EmailConfirmed = true;

                await _userManager.UpdateAsync(existUser);

                return new UserVerifyResponse()
                {
                    Check = true,
                    Status = "success",
                    Message = "Email Verify Successfully"
                };
            }
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


        public async Task<ChangePasswordResponse> ChangePasswordAsync(string GetUserId, ChangePasswordRequest changePasswordRequest)
        {
            var user = await _userManager.FindByIdAsync(GetUserId);

            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                return new ChangePasswordResponse()
                {
                    Check = false,
                    Status = "success",
                    Message = "Your Email Address has not been confirm. Kindly Check Your Email Address"
                };
            }

            var result = await _userManager.ChangePasswordAsync(user, changePasswordRequest.Old_Password, changePasswordRequest.Password);

            if(!result.Succeeded)
            {
                return new ChangePasswordResponse()
                {
                    Check = false,
                    Status = "success",
                    Message = "Your Old Password is Incorrect"
                };
            }

            return new ChangePasswordResponse()
            {
                Check = true,
                Status = "success",
                Message = "Password Changed"
            };
        }

        public async Task<AppUser> GetUserAsync(string UserId)
        {
            return await _userManager.Users
                                    .Include(s => s.Nationality)
                                    .Include(x => x.State)
                                    .SingleOrDefaultAsync(s => s.Id == UserId);
        }


        public async Task<UserUpdateResponse> UpdateUserAsync(string GetUserId, UserUpdateRequest updateRequest)
        {
            var user = await _userManager.FindByIdAsync(GetUserId);

            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                return new UserUpdateResponse()
                {
                    Check = false,
                    Status = "success",
                    Message = "Username Doesn't Exist"
                };
            }

            user.Full_Name = updateRequest.Full_Name;
            user.Address = updateRequest.Address;
            user.Bio = updateRequest.Bio;
            user.NationalityId = updateRequest.NationalityId;
            user.StateId = updateRequest.StateId;
            user.PhoneNumber = updateRequest.Phone_Number;

            await _userManager.UpdateAsync(user);

            return new UserUpdateResponse()
            {
                Check = true,
                Status = "success",
                Message = "Updated Successfully"
            };
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