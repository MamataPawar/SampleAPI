using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SampleAPI.Entities;
using SampleAPI.Requests;

namespace SampleAPI.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly SampleApiDbContext _dContext;
        public OrderRepository(SampleApiDbContext dContext)
        {
            this._dContext = dContext;
        }

        public async Task<Order> AddNewOrder(Order order)
        {
            var result= await _dContext.Orders.AddAsync(order);
            await SaveAsync();

            return result.Entity;
        }

        public async Task DeleteOrder(int orderID)
        {
            var order = await FindOrder(orderID);

            if (order != null)
                order.IsDeleted = true;
            await SaveAsync();
        }

        public async Task<Order> FindOrder(int orderID)
        {
            return await _dContext.Orders.FindAsync(orderID);
        }

        public async Task<List<Order>> GetRecentOrders()
        {
            var orders = await _dContext.Orders.Where(x => x.EntryDate < DateTime.Now && x.IsDeleted == false).OrderBy(x => x.EntryDate).ToListAsync();
            return orders;
        }

        public async Task<List<Order>> GetOrdersForLastNDays(int days)
        {
            var startDate = CalculateStartDateExcludingWeekends(days);
            return await GetOrdersAfterDateExcludingWeekends(startDate);
        }

        private async Task<List<Order>> GetOrdersAfterDateExcludingWeekends(DateTime startDate)
        {
            var orders = await _dContext.Orders
                .Where(x => x.EntryDate >= startDate
                            && x.IsDeleted == false
                            && x.EntryDate.DayOfWeek != DayOfWeek.Saturday
                            && x.EntryDate.DayOfWeek != DayOfWeek.Sunday)
                .OrderBy(x => x.EntryDate)
                .ToListAsync();

            return orders;
        }

        private DateTime CalculateStartDateExcludingWeekends(int days)
        {
            var currentDate = DateTime.Now;
            var validDaysCount = 0;
            var currentDateMinusOneDay = currentDate.AddDays(-1);

            while (validDaysCount < days)
            {
                if (currentDateMinusOneDay.DayOfWeek != DayOfWeek.Saturday
                    && currentDateMinusOneDay.DayOfWeek != DayOfWeek.Sunday)
                {
                    validDaysCount++;
                }
                currentDateMinusOneDay = currentDateMinusOneDay.AddDays(-1);
            }

            return currentDateMinusOneDay;
        }

        public async Task SaveAsync()
        {
            await _dContext.SaveChangesAsync();
        }
    }
}
