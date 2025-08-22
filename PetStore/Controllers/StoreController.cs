using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetStore.DTOs;
using PetStore.Models;

namespace PetStore.Controllers
{
    [ApiController]
    [Route("store")]
    public class StoreController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StoreController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("inventory")]
        // Returns pet inventories by status
        public async Task<IActionResult> GetInventory()
        {
            var inventory = await (
                from order in _context.Orders
                group order by order.Status into statusGroup
                select new
                {
                    Status = statusGroup.Key.ToString(),
                    Count = statusGroup.Count(),
                }
            ).ToDictionaryAsync(item => item.Status, item => item.Count);

            return Ok(inventory);
        }

        [HttpPost("order")]
        // Place an order for a pet
        public async Task<IActionResult> CreateOrder(OrderDTO orderRequest)
        {
            var order = new Order
            {
                PetId = orderRequest.PetId,
                Quantity = orderRequest.Quantity,
                ShipDate = orderRequest.ShipDate ?? DateTime.Now,
                Status = Enums.OrderStatus.Placed,
                Complete = false
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return Ok($"New order is taken. Order informations: \n" +
                $"Pet Id: {order.PetId}\nQuantity: {order.Quantity}\nShipdate: {order.ShipDate}\nStatus: {order.Status}\nComplete: {order.Complete}");
        }

        [HttpGet("order/{orderId}")]
        // Find Purchase order by ID
        public async Task<IActionResult> FindOrder([FromRoute] int orderId)
        {
            var order = await GetOrderByIdAsync(orderId);
            if (order == null) return NotFound("Order did not found");
            return Ok(order);
        }

        [HttpDelete("order/{orderId}")]
        // Delete purchase order by ID
        public async Task<IActionResult> DeleteOrder([FromRoute] int orderId)
        {
            var order = await GetOrderByIdAsync(orderId);
            if (order == null) return NotFound("Order did not found");

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return Ok("Order deleted succesfully.");
        }

        private async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders.FirstOrDefaultAsync(u => u.Id == orderId);
        }
    }
}
