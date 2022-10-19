using AutoMapper;
using CRUDNewsApi.Entities;
using CRUDNewsApi.Helpers;
using CRUDNewsApi.Helpers.Pagination;
using CRUDNewsApi.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CRUDNewsApi.Controllers
{
    [Route("api/v1/user")]
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

        [Authorize(ERoles.Admin)]
        [HttpGet("")]
        public IActionResult GetAll([FromQuery]UserPaginationParams pagination)
        {
            var identity = (User)HttpContext.Items["User"];
            var users = _userService.GetAll(identity.Id,pagination);

            var metadata = new
            {
                users.TotalCount,
                users.PageSize,
                users.CurrentPage,
                users.TotalPages,
                users.HasNext,
                users.HasPrevious
            };

            //Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(new {pagination = metadata, data = users});
        }
    }
}
