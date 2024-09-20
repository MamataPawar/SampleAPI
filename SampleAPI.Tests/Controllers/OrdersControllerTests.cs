using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using SampleAPI.Controllers;
using SampleAPI.Entities;
using SampleAPI.Repositories;
using SampleAPI.Requests;
using Xunit.Sdk;

namespace SampleAPI.Tests.Controllers
{
    public class OrdersControllerTests
    {
        // TODO: Write controller unit tests
        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly OrdersController _controller;

        public OrdersControllerTests()
        {
            _mockOrderRepository = new Mock<IOrderRepository>();
            _controller = new OrdersController(_mockOrderRepository.Object);
        }

        [Fact]
        public async Task GetOrders_ReturnsOkResult_WithRecentOrders()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order { ID = 1, Name = "Order1", Description = "Description1", EntryDate = DateTime.Now },
                new Order { ID = 2, Name = "Order2", Description = "Description2", EntryDate = DateTime.Now }
            };

            _mockOrderRepository.Setup(repo=>repo.GetRecentOrders()).ReturnsAsync(orders);

            //Act
            var result=await _controller.GetOrders();

            //Assert
            var okResult=Assert.IsType<ActionResult<List<Order>>>(result);
            var returnValue = Assert.IsType<List<Order>>(okResult.Value);

            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task CreateOrders_ReturnsCreatedOrder()
        {
            // Arrange
            var createOrderRequest = new CreateOrderRequest
            {
                Name = "New Order",
                Description = "New Description"
            };

            var order = new Order
            {
                ID = 1,
                Name = createOrderRequest.Name,
                Description = createOrderRequest.Description,
                EntryDate = DateTime.Now
            };

            _mockOrderRepository.Setup(repo => repo.AddNewOrder(It.IsAny<Order>())).ReturnsAsync(order);

            // Act
            var result = await _controller.CreateOrders(createOrderRequest);

            // Assert
            var createdResult = Assert.IsType<ActionResult<Order>>(result);
            Assert.IsType<Order>(createdResult.Value);
            Assert.Equal("New Order", createdResult.Value.Name);
        }

        [Fact]
        public async Task DeleteOrders_ReturnsOkResult_WhenOrderExists()
        {
            // Arrange
            var orderId = 1;
            var order = new Order { ID = orderId, Name = "Order1" };

            _mockOrderRepository.Setup(repo => repo.FindOrder(orderId)).ReturnsAsync(order);
            _mockOrderRepository.Setup(repo => repo.DeleteOrder(orderId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteOrders(orderId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task DeleteOrders_ReturnsNotFound_WhenOrderDoesNotExist()
        {
            // Arrange
            var orderId = 1;
            _mockOrderRepository.Setup(repo => repo.FindOrder(orderId)).ReturnsAsync((Order)null);

            // Act
            var result = await _controller.DeleteOrders(orderId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
