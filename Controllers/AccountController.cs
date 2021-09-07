using System.Threading.Tasks;
using AutoMapper;
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


        [HttpPost("v1/register/admin")]
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
    }
}