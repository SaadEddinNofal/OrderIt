using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OrderITDemo.Abstraction;
using OrderITDemo.Controllers;
using OrderITDemo.Models;
using System.Security.Claims;

namespace OrderITDemo.Tests.Controllers
{
    public class CartControllerTests
    {
        private readonly Mock<ICartService> _cartServiceMock;
        private readonly Mock<UserManager<AppUser>> _userManagerMock;
        private readonly CartController _controller;

        public CartControllerTests()
        {
            _cartServiceMock = new Mock<ICartService>();
            var store = new Mock<IUserStore<AppUser>>();
            _userManagerMock = new Mock<UserManager<AppUser>>(store.Object, null, null, null, null, null, null, null, null);
            _controller = new CartController(_cartServiceMock.Object, _userManagerMock.Object);
        }

        [Fact]
        public async Task Index_ReturnsNotFound_WhenUserIsNull()
        {
            _userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync((AppUser)null);
            var result = await _controller.Index();
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task AddToCart_RedirectsToLogin_WhenUserIsNull()
        {

            _userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync((AppUser)null);
            var result = await _controller.AddToCart(1);
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Login", redirect.ActionName);
            Assert.Equal("Account", redirect.ControllerName);
        }

        [Fact]
        public async Task Minus_CallsDecreaseItemQuantityAsync_AndRedirectsToIndex()
        {
            var user = new AppUser { Id = "user1" };
            _userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(user);

            _cartServiceMock.Setup(x => x.DecreaseItemQuantityAsync(It.IsAny<int>(), user.Id))
                .Returns(Task.CompletedTask);

            var result = await _controller.Minus(2);

            _cartServiceMock.Verify(x => x.DecreaseItemQuantityAsync(2, user.Id), Times.Once);
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }
        [Fact]
        public async Task AddToCart_AddsItemAndRedirectsToIndex_WhenUserIsNotNull()
        {
            var user = new AppUser { Id = "user1" };
            _userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(user);

            _cartServiceMock.Setup(x => x.AddToCartAsync(1, user.Id))
                .Returns(Task.CompletedTask);
            var result = await _controller.AddToCart(1);
            _cartServiceMock.Verify(x => x.AddToCartAsync(1, user.Id), Times.Once);

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

    }
}
