using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using Moq;
using SampleAPI.Entities;
using SampleAPI.Repositories;
using SampleAPI.Requests;

namespace SampleAPI.Tests.Repositories
{
    public class OrderRepositoryTests
    {
        // TODO: Write repository unit tests
        private readonly Mock<SampleApiDbContext> _mockContext;
        private readonly OrderRepository _repository;

        public OrderRepositoryTests()
        {
            // Create a mock DbContext
            _mockContext = new Mock<SampleApiDbContext>(new DbContextOptions<SampleApiDbContext>());
            

            // Initialize the repository with the mocked context
            _repository = new OrderRepository(MockSampleApiDbContextFactory.GenerateMockContext());
        }

        [Fact]
        public async Task AddNewOrder_Should_Add_Order_To_Database()
        {
            // Arrange
            var newOrder = new Order { ID = 1, EntryDate = DateTime.Now, IsDeleted = false, Name="Test1", Description="Test 1" };

            _mockContext.Setup(x => x.Orders.AddAsync(newOrder, default)).ReturnsAsync((Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Order>)null);

            // Act
            var result = await _repository.AddNewOrder(newOrder);

            // Assert
            _mockContext.Verify(x => x.Orders.AddAsync(newOrder, default), Times.Once);
            _mockContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task DeleteOrder_Should_Set_IsDeleted_To_True()
        {
            // Arrange
            var existingOrder = new Order { ID = 1, IsDeleted = false };

            _mockContext.Setup(x => x.Orders.FindAsync(1))
                .ReturnsAsync(existingOrder);

            // Act
            await _repository.DeleteOrder(1);

            // Assert
            Assert.True(existingOrder.IsDeleted);
            _mockContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task FindOrder_Should_Return_Order_If_Exists()
        {
            // Arrange
            var existingOrder = new Order { ID = 1 };
            _mockContext.Setup(x => x.Orders.FindAsync(1))
                .ReturnsAsync(existingOrder);

            // Act
            var result = await _repository.FindOrder(1);

            // Assert
            Assert.Equal(existingOrder, result);
        }

        [Fact]
        public async Task GetRecentOrders_Should_Return_List_Of_Orders()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order { ID = 1, EntryDate = DateTime.Now.AddDays(-2), IsDeleted = false },
                new Order { ID = 2, EntryDate = DateTime.Now.AddDays(-1), IsDeleted = false }
            };

            _mockContext.Setup(x => x.Orders).Returns(MockSampleApiDbContextFactory.GenerateMockContext().Orders);

            // Act
            var result = await _repository.GetRecentOrders();

            // Assert
            Assert.Equal(0, result.Count);
            Assert.True(result.All(o => o.IsDeleted == false));
        }
    }
}