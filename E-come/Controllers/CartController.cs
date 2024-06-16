using E_come.Model;
using E_come.services;
using E_come.services.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_come.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartReposatory cartReposatory;
        private readonly UserManager<ApplicationUser> userManager;

        public CartController(ICartReposatory cartReposatory,UserManager<ApplicationUser> userManager)
        {
            this.cartReposatory = cartReposatory;
            this.userManager = userManager;
        }

        [HttpPost("AddToCart/{productId}")]
        public async Task<IActionResult> AddToCart(int productId, [FromForm] int quantity = 1)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = await cartReposatory.AddToCard(productId, userId, quantity);
            
            return Ok(res);
        }
        [HttpPost("RemoveFromCart/{productId}")]
        public async Task<IActionResult> RemoveFromCart(int productId, [FromForm] int quantity = 0)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = await cartReposatory.RemoveFromCard(productId, userId, quantity);
            
            return Ok(res);
        }

        [HttpGet("ViewCart")]
        public async Task<IActionResult> ViewCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = await cartReposatory.ViewCard(userId);
            return Ok(res);
        }
        [HttpGet("TotalPayment")]
        public async Task<IActionResult> TotalPayment()
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = await cartReposatory.TotalPayment(userId);

            return Ok(res);
        }
    }
}
