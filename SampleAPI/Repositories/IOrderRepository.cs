using SampleAPI.Entities;
using SampleAPI.Requests;

namespace SampleAPI.Repositories
{
    public interface IOrderRepository
    {
        // TODO: Create repository methods.

        // Suggestions for repo methods:
        public Task<List<Order>> GetRecentOrders();
        public Task<Order> AddNewOrder(Order order);

        public Task DeleteOrder(int orderID);

        public Task<Order> FindOrder(int orderID);

        public Task<List<Order>> GetOrdersForLastNDays(int days);

        Task SaveAsync();
    }
}
