using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CreditVillageBackend.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {      
        private readonly IAccount _account;

        private readonly IEmailSender _emailSender;

        private readonly IMapper _mapper;

        public AccountController(IAccount account,
            IEmailSender emailSender,
            IMapper mapper)
        {
            _account = account;
            this._emailSender = emailSender;
            this._mapper = mapper;
        }


        [HttpPost("v1/register")]
        public async Task<IActionResult> UserRegister([FromBody] UserRegisterRequest userRequest)
        {
            var authResponse = await _account.UserRegisterAsync(userRequest);

            if (!authResponse.Check)
            {
                return Ok(_mapper.Map<UserRegisterMapping>(authResponse));
            }

            var callbackUrl = Url.Action("ConfirmEmail", "Manage", new { UserId = authResponse.UserId, Code = authResponse.Code }, HttpContext.Request.Scheme);
             
            await _emailSender.SendEmailAsync(authResponse.Email, "CREDIT VILLAGE - Confirm Your Email", "Please Confirm Your E-Mail by clicking this link: <a href=\"" + callbackUrl + "\">Click here </a>");

            return Ok( _mapper.Map<UserRegisterMapping>(authResponse));
        }


        [HttpPost("v1/admin/register")]
        public async Task<IActionResult> AdminRegister([FromBody] AdminRegisterRequest userRequest)
        {
            var authResponse = await _account.AdminRegisterAsync(userRequest);

            if (!authResponse.Check)
            {
                return Ok(_mapper.Map<AdminRegisterMapping>(authResponse));
            }

            var callbackUrl = Url.Action("ConfirmEmail", "Manage", new { UserId = authResponse.UserId, Code = authResponse.Code }, HttpContext.Request.Scheme);
             
            await _emailSender.SendEmailAsync(authResponse.Email, "CREDIT VILLAGE - Confirm Your Email", "Please Confirm Your E-Mail by clicking this link: <a href=\"" + callbackUrl + "\">Click here </a>");

            return Ok( _mapper.Map<AdminRegisterMapping>(authResponse));
        }


        [HttpPost("v1/login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest userRequest)
        {
            var authUserResponse = await _account.LoginAsync(userRequest);

            if (!authUserResponse.Check)
            {
                return Ok(_mapper.Map<UserLoginMapping>(authUserResponse));
            }

            return Ok(_mapper.Map<UserLoginMapping>(authUserResponse));
        }


        [HttpPost("v1/user/changepassword")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest changePasswordRequest)
        {
            var passwordResponse = await _account.ChangePasswordAsync(GetUserId(), changePasswordRequest);

            if (!passwordResponse.Check)
            {
                return Ok(_mapper.Map<ChangePasswordMapping>(passwordResponse));
            }

            return Ok(_mapper.Map<ChangePasswordMapping>(passwordResponse));
        }


        [HttpGet("v1/user")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetUser()
        {
            var user = await _account.GetUserAsync(GetUserId());

            return Ok(_mapper.Map<GetUserMapping>(user));
        }


        [HttpPut("v1/user")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateRequest updateRequest)
        {
            var userUpdate = await _account.UpdateUserAsync(GetUserId(), updateRequest);

            if (!userUpdate.Check)
            {
                return Ok(_mapper.Map<UserUpdateMapping>(userUpdate));
            }

            return Ok(_mapper.Map<UserUpdateMapping>(userUpdate));
        }


        private string GetUserId()
        {
            return HttpContext.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;
        }       
    }
}