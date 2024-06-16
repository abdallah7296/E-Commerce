using E_come.services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_come.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        [HttpGet("CheckOut")]
        public async Task<IActionResult> CheckOut()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = await orderRepository.CheckOut(userId);
            
            return Ok(res);
        }
        [HttpGet("ViewOrders")]
        public async Task<IActionResult> ViewOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = await orderRepository.ViewOrders(userId);

            return Ok(res);
        }
        [HttpPost("CancelOrder")]
        public async Task<IActionResult> CancelOrder([FromForm] string orderNumber)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = await orderRepository.CancelOrder(userId, orderNumber);
          
            return Ok(res);
        }
    }
}
