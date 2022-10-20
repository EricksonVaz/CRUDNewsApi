using AutoMapper;
using CRUDNewsApi.Entities;
using CRUDNewsApi.Helpers;
using CRUDNewsApi.Helpers.Pagination;
using CRUDNewsApi.Models.User;
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

        [Authorize(ERoles.Admin)]
        [HttpGet("{id}")]
        public IActionResult GetOne([FromRoute] int id)
        {
            var identity = (User)HttpContext.Items["User"];
            var user = _userService.GetById(identity.Id, id);
            return Ok(user);
        }

        [Authorize]
        [HttpPut("change-my-password")]
        public IActionResult ChangePassword(ChangePasswordRequest model)
        {
            var identity = (User)HttpContext.Items["User"];
            _userService.ChangePassword(identity.Id, model);
            return Ok(new {message = "Password changed successfully" });
        }

        [Authorize(ERoles.Admin)]
        [HttpPut("change-password")]
        public IActionResult ChangeUserPassword(UpdatePasswordRequest model)
        {
            var identity = (User)HttpContext.Items["User"];
            _userService.UpdatePassword(identity.Id, model);
            return Ok(new { message = "Password changed successfully" });
        }

        [Authorize(ERoles.Admin)]
        [HttpPut("status")]
        public IActionResult UpdateStatus(UpdateStatusRequest model)
        {
            var identity = (User)HttpContext.Items["User"];
            _userService.ChangeStatus(identity.Id, model);
            return Ok(new { message = "User Status updated successfully" });
        }

        [Authorize(ERoles.Admin)]
        [HttpDelete("")]
        public IActionResult DeleteUser(int id)
        {
            var identity = (User)HttpContext.Items["User"];
            _userService.Delete(identity.Id, id);
            return NoContent();
        }

        [Authorize(ERoles.Admin)]
        [HttpPost("")]
        public IActionResult RegisterUser(RegisterRequest model)
        {
            _userService.Register(model);
            return Created("User Created",new {message = "New User created successfully" });
        }

        [Authorize(ERoles.Admin)]
        [HttpPut("")]
        public IActionResult UpdateUser(UpdateRequestUser model)
        {
            var identity = (User)HttpContext.Items["User"];
            _userService.Update(identity.Id, model);
            return Ok(new { message = "User Updated Successfully" });
        }

        [Authorize(ERoles.Admin)]
        [HttpPost("update-photo")]
        public IActionResult UpdatePhoto([FromForm]UpdatePhotoRequest model)
        {
            var identity = (User)HttpContext.Items["User"];
            _userService.UpdatePhoto(identity.Id, model);
            return Ok(new { message = "User Photo Updated Successfully" });
        }
    }
}
