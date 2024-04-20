using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using MyRestfulApp_NET.Common;
using MyRestfulApp_NET.Domain.Services;
using MyRestfulApp_NET.Domain.Services.Communication;
using MyRestfulApp_NET.Resources;

namespace MyRestfulApp_NET.Controllers
{
    [Produces("application/json")]
    [ApiController]
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet()]
        [ProducesResponseType(200)]
        public async Task<ActionResult<List<UserResource>>> GetUsers()
        {
            var users = await _userService.GetUsers();
            return Ok(users);
        }

        [HttpPost()]
        [ProducesResponseType(typeof(BasicMessageResponse), 200)]
        [ProducesResponseType(typeof(ValidationErrorResponse), 400)]
        [ProducesResponseType(typeof(BasicMessageResponse), 422)]
        public async Task<ActionResult<UserResource>> SaveUser([FromBody][Required] UserSaveResource userSaveResource)
        {
            var response = await _userService.SaveUser(userSaveResource);

            return response.Success ? Ok(response) : UnprocessableEntity(response);
        }

        [HttpPut("{userId}")]
        [ProducesResponseType(typeof(UpdateUserMessageResponse), 200)]
        [ProducesResponseType(typeof(BasicMessageResponse), 422)]
        [ProducesResponseType(typeof(BasicMessageResponse), 404)]
        public async Task<ActionResult<UserResource>> UpdateUser([FromRoute][Required] string userId, [FromBody] UserUpdateResource userUpdateResource)
        {
            var response = await _userService.UpdateUser(int.Parse(userId), userUpdateResource);

            return response.Success ? Ok(response) : NotFound(response);
        }

        [HttpDelete("{userId}")]
        [ProducesResponseType(typeof(BasicMessageResponse), 200)]
        [ProducesResponseType(typeof(BasicMessageResponse), 404)]
        public async Task<ActionResult<UserResource>> DeleteUser([FromRoute][Required] string userId)
        {
            var response = await _userService.DeleteUser(int.Parse(userId));

            return response.Success ? Ok(response) : NotFound(response);
        }
    }
}