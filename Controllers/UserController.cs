using AutoMapper;
using CRUDNewsApi.Helpers;
using CRUDNewsApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace CRUDNewsApi.Controllers
{
    [Authorize]
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        private IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(
            IUserService userService,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("")]
        public IActionResult GetAll()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            //var users = _userService.GetAllidentity.Id as Clam);
            return Ok(identity);
        }
    }
}
