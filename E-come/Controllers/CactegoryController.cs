using E_come.DTO.CategoryDTO;
using E_come.DTO.ShopDTO;
using E_come.Model;
using E_come.services.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_come.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CactegoryController : ControllerBase
    {

        private readonly DBContext context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICategoryRepository categoryRepository;

        public CactegoryController(DBContext _context, IWebHostEnvironment webHostEnvironment,
         ICategoryRepository _categoryRepository)
        {
            context = _context;
            _webHostEnvironment = webHostEnvironment;
            categoryRepository = _categoryRepository;
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public IActionResult Creat([FromForm] AddCategoryDto categoryDTO, IFormFile ImageFile)
        {
            var shop = context.Categories.FirstOrDefault(x => x.Name == categoryDTO.Name);
            if (shop != null)
            {
                return BadRequest("Category already exists");
            }

            categoryRepository.Add(categoryDTO, ImageFile);

            //       string url = Url.Link("GetOneOrdRoute", new { Id = product.Id });
            return Ok("Create Done");
        }
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public IActionResult update(int id, [FromForm] UpdateCategoryDto categoryDTO, IFormFile ImageFile)
        {
            categoryRepository.UpdateById(id, categoryDTO, ImageFile);

            //       string url = Url.Link("GetOneOrdRoute", new { Id = product.Id });
            return Ok("Update Done");
        }

        [HttpGet]
        public  IActionResult GetAllCategory()
        {
           var  cats =  categoryRepository.GetAll();
            return Ok(cats);
        }
    }
}
