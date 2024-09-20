using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SampleAPI.Entities;
using SampleAPI.Repositories;
using SampleAPI.Requests;
using System.Net;

namespace SampleAPI.Controllers
{
    [ApiController]
    [Route("[controller]/v1")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        // Add more dependencies as needed.

        public OrdersController(IOrderRepository orderRepository)
        {
            this._orderRepository = orderRepository;
        }

        [HttpGet("RecentOrders")] 
        [ProducesResponseType(StatusCodes.Status200OK)] 
        public async Task<ActionResult<List<Order>>> GetOrders()
        {
            var orders = await _orderRepository.GetRecentOrders();
            return orders;
        }

        /// TODO: Add an endpoint to allow users to create an order using <see cref="CreateOrderRequest"/>.
        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Order>> CreateOrders(CreateOrderRequest orderReq)
        {
            if (orderReq == null)
                return BadRequest();

            var order = new Order();
            order.Name = orderReq.Name;
            order.Description = orderReq.Description;
            order.EntryDate = DateTime.Now;

            var addedOrder = _orderRepository.AddNewOrder(order).Result;

            return addedOrder;
        }

        [HttpDelete("Remove/{id}")] 
        public async Task<ActionResult> DeleteOrders(int id)
        {
            var order= await _orderRepository.FindOrder(id);
            if(order==null)
                return NotFound();

            await _orderRepository.DeleteOrder(id);

            return Ok(HttpStatusCode.NoContent);
        }

        [HttpGet("Filter/{days}")]
        public async Task<ActionResult<List<Order>>> GetFilterOrder(int days)
        {
            if (days <= 0)
                return BadRequest();

            var orders = await _orderRepository.GetOrdersForLastNDays(days);
            return orders;
        }
    }
}
