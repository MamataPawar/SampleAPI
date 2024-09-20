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
        private readonly SampleApiDbContext _mockContext;
        private readonly IOrderRepository _repository;

        public OrderRepositoryTests()
        {
            // Create a mock DbContext
            _mockContext = MockSampleApiDbContextFactory.GenerateMockContext();             

            // Initialize the repository with the mocked context
            _repository = new OrderRepository(_mockContext);

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            _mockContext.Orders.AddRange(
                    new Order { ID = 1, EntryDate = DateTime.Now.AddDays(-1), IsDeleted = false, Name = "Test1", Description = "Test 1" },
                    new Order { ID = 2, EntryDate = DateTime.Now.AddDays(-5), IsDeleted = false, Name = "Test2", Description = "Test 2" },
                    new Order { ID = 3, EntryDate = DateTime.Now.AddDays(-10), IsDeleted = false, Name = "Test3", Description = "Test 3" } 
                );
            _mockContext.SaveChangesAsync();
            
        }

        [Fact]
        public async Task AddNewOrder_Should_Add_Order_To_Database()
        {
            // Arrange
            var newOrder = new Order { ID = 4, EntryDate = DateTime.Now, IsDeleted = false, Name="Test1", Description="Test 1" };

            // Act
            var result = await _repository.AddNewOrder(newOrder);

            // Assert
            var addedOrder = await _mockContext.Orders.FindAsync(4);
            Assert.NotNull(addedOrder);
            Assert.Equal(newOrder.ID, addedOrder.ID);
            Assert.Equal(newOrder.EntryDate, addedOrder.EntryDate);
        }

        [Fact]
        public async Task DeleteOrder_Should_Set_IsDeleted_To_True()
        {
            int orderID = 3;
            // Act
            await _repository.DeleteOrder(orderID);

            // Assert
            Assert.True(_mockContext.Orders.FindAsync(orderID).Result.IsDeleted);            
        }

        [Fact]
        public async Task FindOrder_Should_Return_Order_If_Exists()
        {
            int orderID = 1;
            // Act
            var result = await _repository.FindOrder(orderID);

            // Assert
            Assert.Equal(_mockContext.Orders.FindAsync(orderID).Result, result);
        }

        [Fact]
        public async Task GetRecentOrders_Should_Return_List_Of_Orders()
        {            
            // Act
            var result = await _repository.GetRecentOrders();

            // Assert
            Assert.Equal(3, result.Count);
            Assert.True(result.All(o => o.IsDeleted == false));
        }

        [Fact]
        public async Task GetOrdersForLastNDays_Should_Return_List_Of_Orders()
        {
            int dayCount = 5;

            // Act
            var result = await _repository.GetOrdersForLastNDays(5);

            // Assert
            Assert.Equal(1, result.Count);
            Assert.True(result.All(o => o.IsDeleted == false));
        }
    }
}