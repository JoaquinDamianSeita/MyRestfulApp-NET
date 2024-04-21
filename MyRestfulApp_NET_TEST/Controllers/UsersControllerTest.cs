using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyRestfulApp_NET_API.Controllers;
using MyRestfulApp_NET_API.Domain.Services;
using MyRestfulApp_NET_API.Domain.Services.Communication;
using MyRestfulApp_NET_API.Resources;
using Xunit;
using Moq;

namespace MyRestfulApp_NET_TEST.Controllers
{
    public class UsersControllerTest
    {
        [Fact]
        public async Task UsersControllerGetUsersTestOk()
        {
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(service => service.GetUsers())
                .ReturnsAsync(new List<UserResource>());

            var controller = new UsersController(mockUserService.Object);

            var result = await controller.GetUsers();

            var actionResult = Assert.IsType<ActionResult<List<UserResource>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var users = Assert.IsType<List<UserResource>>(okResult.Value);
            Assert.NotNull(users);
        }

        [Fact]
        public async Task UsersControllerSaveUserTestOk()
        {
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(service => service.SaveUser(It.IsAny<UserSaveResource>()))
                .ReturnsAsync(new BasicMessageResponse(true, "User saved successfully", 0));

            var controller = new UsersController(mockUserService.Object);

            var result = await controller.SaveUser(new UserSaveResource());

            var actionResult = Assert.IsType<ActionResult<UserResource>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var response = Assert.IsType<BasicMessageResponse>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal("User saved successfully", response.Message);
        }
        
        [Fact]
        public async Task UsersControllerSaveUserTestUnprocessableEntity()
        {
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(service => service.SaveUser(It.IsAny<UserSaveResource>()))
                .ReturnsAsync(new BasicMessageResponse(false, "Password is required", 1));

            var controller = new UsersController(mockUserService.Object);

            var result = await controller.SaveUser(new UserSaveResource());

            var actionResult = Assert.IsType<ActionResult<UserResource>>(result);
            var okResult = Assert.IsType<UnprocessableEntityObjectResult>(actionResult.Result);
            var response = Assert.IsType<BasicMessageResponse>(okResult.Value);
            Assert.False(response.Success);
            Assert.Equal("Password is required", response.Message);
        }

        [Fact]
        public void UsersControllerSaveUserTestBadRequest()
        {
            var mockUserService = new Mock<IUserService>();

            var controller = new UsersController(mockUserService.Object);
            controller.ModelState.AddModelError("Email", "The Email field is required.");

            _ = controller.SaveUser(new UserSaveResource());

            var isValid = controller.ModelState.IsValid;

            Assert.False(isValid);
        }

        [Fact]
        public async Task UsersControllerUpdateUserTestOk()
        {
            var mockUserService = new Mock<IUserService>();
            var userId = "1";
            var userUpdateResource = new UserUpdateResource();
            var expectedResponse = new UpdateUserMessageResponse(true, "User updated successfully", 0, new UserResource { });

            mockUserService.Setup(service => service.UpdateUser(int.Parse(userId), userUpdateResource))
                .ReturnsAsync(expectedResponse);

            var controller = new UsersController(mockUserService.Object);

            var result = await controller.UpdateUser(userId, userUpdateResource);

            var actionResult = Assert.IsType<ActionResult<UserResource>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var response = Assert.IsType<UpdateUserMessageResponse>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal("User updated successfully", response.Message);
        }

        [Fact]
        public async Task UsersControllerUpdateUserTestNotFound()
        {
            var mockUserService = new Mock<IUserService>();
            var userId = "1";
            var userUpdateResource = new UserUpdateResource();
            var expectedResponse = new UpdateUserMessageResponse(false, "User not found", 1, new UserResource { });

            mockUserService.Setup(service => service.UpdateUser(int.Parse(userId), userUpdateResource))
                .ReturnsAsync(expectedResponse);

            var controller = new UsersController(mockUserService.Object);

            var result = await controller.UpdateUser(userId, userUpdateResource);

            var actionResult = Assert.IsType<ActionResult<UserResource>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            var response = Assert.IsType<UpdateUserMessageResponse>(notFoundResult.Value);
            Assert.False(response.Success);
            Assert.Equal("User not found", response.Message);
        }

        [Fact]
        public void UsersControllerUpdateUserTestBadRequest()
        {
            var mockUserService = new Mock<IUserService>();

            var controller = new UsersController(mockUserService.Object);
            controller.ModelState.AddModelError("Email", "The Email field is required.");

            _ = controller.UpdateUser("1", new UserUpdateResource());

            var isValid = controller.ModelState.IsValid;

            Assert.False(isValid);
        }

        [Fact]
        public async Task UsersControllerDeleteUserTestOk()
        {
            var mockUserService = new Mock<IUserService>();
            var userId = "1";
            var expectedResponse = new UpdateUserMessageResponse(true, "User deleted successfully!", 0, new UserResource { });

            mockUserService.Setup(service => service.DeleteUser(int.Parse(userId)))
                .ReturnsAsync(expectedResponse);

            var controller = new UsersController(mockUserService.Object);

            var result = await controller.DeleteUser(userId);

            var actionResult = Assert.IsType<ActionResult<UserResource>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var response = Assert.IsType<UpdateUserMessageResponse>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal("User deleted successfully!", response.Message);
        }

        [Fact]
        public async Task UsersControllerDeleteUserTestNotFound()
        {
            var mockUserService = new Mock<IUserService>();
            var userId = "1";
            var expectedResponse = new BasicMessageResponse(false, "User not found.", 1);

            mockUserService.Setup(service => service.DeleteUser(int.Parse(userId)))
                .ReturnsAsync(expectedResponse);

            var controller = new UsersController(mockUserService.Object);

            var result = await controller.DeleteUser(userId);

            var actionResult = Assert.IsType<ActionResult<UserResource>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            var response = Assert.IsType<BasicMessageResponse>(notFoundResult.Value);
            Assert.False(response.Success);
            Assert.Equal("User not found.", response.Message);
        }
    }
}
