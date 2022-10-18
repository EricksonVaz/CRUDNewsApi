using AutoMapper;
using CRUDNewsApi.Helpers;
using CRUDNewsApi.Models.Auth;
using CRUDNewsApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CRUDNewsApi.Controllers
{
    [AllowAnonymous]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public AuthController(
            IAuthService authService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _authService = authService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [HttpPost("login")]
        public IActionResult Login(Login login)
        {
            var response = _authService.Login(login);
            return Ok(response);
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup(Signup signup)
        {
            var response = await _authService.Signup(signup);

            if (response)
            {
                StatusCode(201);
                return new ObjectResult(new { message = "Account registered successfully check your email to activate it" });
            }
                

            StatusCode(500);
            return new ObjectResult( new {message = "Error sending activation email, contact support to activate account" });
        }
    }
}
