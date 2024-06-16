using E_come.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;
using System.Numerics;
using Microsoft.AspNetCore.Authorization;
using E_come.services.IRepository;
using E_come.services;
using System.Security.Claims;
using E_come.DTO.ProductDTO;

namespace E_come.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class productController : ControllerBase
    {
        private readonly DBContext context;
        private readonly IProductRepository productRepository;

        public productController(DBContext _context,
         IProductRepository _productRepository)
        {
            context = _context;
            productRepository = _productRepository;
        }

        [HttpGet("GetById", Name = "GetOneOrdRoute")]
        public async Task<IActionResult> GetById(int id)
        {
            productDTO cat = await productRepository.GetById(id);

            if (cat == null)
            {
                return NotFound();
            }
            return Ok(cat);
        }
        [HttpGet("GetByname")]
        public async Task<IActionResult> GetByname(string name)
        {
            var cat = await productRepository.GetByName(name);

            if (cat == null)
            {
                return NotFound();
            }
            return Ok(cat);
        }
        
        [HttpGet("GetAllProd")]
        public async Task<IActionResult> GetAllProd()
        {

            var prod = await productRepository.GetAll();
            return Ok(prod);
        }
        [HttpGet("MyProduct")]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> MyProduct()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return BadRequest("You cannot view product you must be login ");
            }

            var prod = await productRepository.GetMyProduct(userId);
            return Ok(prod);
        }

        [HttpPost]
        [Authorize(Roles = "Seller,Admin") ]
        public IActionResult Creat([FromForm] ProdWithCategory prodWithCategory, List<IFormFile> ImageFiles)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            { return BadRequest("You cannot add product to product you must be login "); }

            Product product = productRepository.Mapping(prodWithCategory, userId);
            productRepository.Create(product, ImageFiles);

            string url = Url.Link("GetOneOrdRoute", new { Id = product.Id });
            // return Created(url,product);
            return Ok(prodWithCategory);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Seller,Admin")]
        public IActionResult update(int id, [FromForm] ProdWithCategory prodWithCategory, List<IFormFile> ImageFiles)
        {
            if (ModelState.IsValid)
            {
                Product product = context.products.FirstOrDefault(p => p.Id == id);
                if (product != null)
                {
                    productRepository.UpdateById(id, prodWithCategory, ImageFiles);
                    return Ok("Update Done");
                }
                return BadRequest("Id Not Vaild");
            }
            return BadRequest();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Seller , Admin")]
        public IActionResult delete(int id)
        {
            var oldprod = context.products.FirstOrDefault(p => p.Id == id);
            if (oldprod != null)
            {
                productRepository.DeleteById(oldprod.Id);

                return Ok("record is remove");

            }
            return BadRequest("Id not found");
        }
        [HttpGet("GetAllProductbySortReview")]
        public async Task<IActionResult> GetAllProductbySortReview()
        {
            //await productRepository.GetAll()
            var products = await productRepository.GetAllBySortReview();

            if (products.Any())
            {
                return Ok(products);
            }
            return BadRequest("not found Review");
        }
        [HttpPost("get")]
        public async Task<IActionResult> GetProducts(int numberOfProducts)
        {
            var products = await productRepository.GetProducts(numberOfProducts);
            if (products.Count == 0)
            {
                return NotFound();
            }
            return Ok(products);
        }
        //Implement Pagination
        [HttpPost("GetPaginationProducts")]
        public async Task<IActionResult> GetPaginationProducts([FromForm] int page, [FromForm] int pageSize)
        {
            var products = await productRepository.GetAll();
            var totalCount = products.Count;
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var paginatedProducts = products.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return Ok(new
            {
                TotalCount = totalCount,
                TotalPages = totalPages,
                CurrentPage = page,
                PageSize = pageSize,
                Products = paginatedProducts
            });
        }
        //Implement Sort
        [HttpPost("GetSortProducts")]
        public async Task<IActionResult> GetProducts([FromForm]int page ,[FromForm] int pageSize ,[FromForm] string sortBy = " " , string sortOrder = "asc")
        {
            var products = await productRepository.GetAll();

            // Sorting
            var property = typeof(Product).GetProperty(sortBy);
            if (property != null)
            {
                if (sortOrder.ToLower() == "desc")
                    products = products.OrderByDescending(x => property.GetValue(x, null)).ToList();
                else
                    products = products.OrderBy(x => property.GetValue(x, null)).ToList();
            }

            // Pagination
            var totalCount = products.Count;
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            var paginatedProducts = products.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return Ok(new
            {
                TotalCount = totalCount,
                TotalPages = totalPages,
                CurrentPage = page,
                PageSize = pageSize,
                Products = paginatedProducts
            });
        }
    }
}
