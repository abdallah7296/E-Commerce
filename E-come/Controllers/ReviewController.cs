using E_come.DTO.ReviewDTO;
using E_come.Model;
using E_come.services.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_come.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {

        private readonly IReviewServices reviewServices;
        DBContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;
        public ReviewController(IReviewServices _reviewServices,DBContext _dBContext , UserManager<ApplicationUser> _user)
        {
            reviewServices = _reviewServices;
            dbContext = _dBContext; 
            userManager = _user;
        }
        [HttpPost]
        public async Task<IActionResult> Create(int ProductId,[FromForm] ReviewDTo dTOReview)
        {
            ApplicationUser user = await userManager.GetUserAsync(User);
            // Product product = dbContext.products.SingleOrDefault(d => d.Id == ProductId);

            if (user == null)
                return BadRequest( "You cannot add review to product you must be login ");

            reviewServices.CreateReview(ProductId, dTOReview, user.Id);
                return Ok("add done");
             
           
        }
        

    }
}
