using AutoMapper;
using CRUDNewsApi.Helpers;
using CRUDNewsApi.Helpers.Exceptions;
using KeyNotFoundException = CRUDNewsApi.Helpers.Exceptions.KeyNotFoundException;
using CRUDNewsApi.Models.Auth;
using CRUDNewsApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;
using CRUDNewsApi.Entities;
using CRUDNewsApi.Models.User;
using System.ComponentModel.DataAnnotations;

namespace CRUDNewsApi.Controllers
{

    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;
        private IUserService _userService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public AuthController(
            IAuthService authService,
            IUserService userService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _authService = authService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login(Login login)
        {
            var response = _authService.Login(login);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<IActionResult> Signup(Signup signup)
        {
            var response = await _authService.Signup(signup);

            if (response)
            {
                return Created("User Created", new {message = "User Created successfully" });
            }
                
            throw new Exception("Error sending activation email, contact support to activate account");
        }

        [AllowAnonymous]
        [HttpGet("activate")]
        public IActionResult ActivateAccount(string uuid)
        {
            Guid _;

            if (Guid.TryParse(uuid,out _))
            {
                _authService.ActivateAccount(uuid);
                return Ok(new { message = "User activated successfully" });
            }

            throw new BadRequestException("Guid should contain 32 digits with 4 dashes (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).");

        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ResetPasswordRequest model)
        {
            var response = await _authService.ForgotPassword(model);
            if (response is KeyNotFoundException) throw (KeyNotFoundException)response;
            else if (response is User) return Ok(new { message = "Email sent, follow the instructions to recover your password" });

            else throw new Exception((string?)response);

        }

        [AllowAnonymous]
        [HttpPost("resend-activation-email")]
        public async Task<IActionResult> ResendEmail(ResetPasswordRequest model)
        {
            var response = await _authService.ResendEmail(model);
            if (response is KeyNotFoundException) throw (KeyNotFoundException)response;
            else if (response is User) return Ok(new { message = "Email sent, follow the instructions to activate your account" });

            else throw new Exception((string?)response);

        }

        [AllowAnonymous]
        [HttpPut("reset-password")]
        public IActionResult ResetPassword(ResetPassword model)
        {
            _authService.ChangePassword(model);
            return Ok(new { message = "Password changed successfully" });

        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult UserLogged()
        {
            var identity = (User)HttpContext.Items["User"];
            var me = _authService.UserLogged(identity.Id);
            return Ok(me);

        }

        [Authorize]
        [HttpPut("me")]
        public IActionResult UpdateUserLogged(UpdateRequest model)
        {
            var identity = (User)HttpContext.Items["User"];
            var me = _authService.UpdateUserLogged(identity.Id, model);
            return Ok(new { message = "Info changed successfully", user = me });

        }

        [Authorize]
        [HttpPut("me/photo")]
        public IActionResult UpdatePhoto([Required] IFormFile photo)
        {
            var identity = (User)HttpContext.Items["User"];
            _userService.UpdatePhoto(identity.Id, photo);
            return Ok(new { message = "Photo Updated Successfully" });
        }
    }
}
