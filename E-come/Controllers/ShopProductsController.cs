using E_come.DTO.ShopDTO;
using E_come.Model;
using E_come.services;
using E_come.services.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace E_come.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopProductsController : ControllerBase
    {
        private readonly IShopProductsRepository shopProductsRepository;
        private readonly DBContext Context;

        public ShopProductsController(IShopProductsRepository _shopProductsRepository,DBContext _Context)
        {
            shopProductsRepository = _shopProductsRepository;
            Context = _Context;
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromForm] ShopUserDto shopDto, string iamgefil)
        {
            var shop = Context.shopsProducts.FirstOrDefault(x => x.Name == shopDto.Name);
            if (shop != null)
            {
                return BadRequest("Shop already exists");
            }

            var result = await shopProductsRepository.Add(shopDto, iamgefil);
            if (result.IsAuthenticated == true)
            { return Ok(shopDto); }

            return BadRequest(result.Message);
        }
        /*public async Task<IActionResult> Create([FromForm] ShopProdDTO shopDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return BadRequest("You cannot add shop  you must be login ");

            }
            var shop = Context.shopsProducts.FirstOrDefault(x => x.Name == shopDto.Name);
            if (shop != null)
            {
                return BadRequest("Shop already exists");
            }
            await shopProductsRepository.Add(shopDto, userId);

            return Ok(shopDto);
        }*/

        [HttpGet("GetAllShop")]
        public async Task<IActionResult> GetAllShop()
        {
            var shops = await shopProductsRepository.GetAllShop();
            return Ok(shops);
        }
        [HttpGet("MyShop")]
        [Authorize(Roles = "Seller,Admin")]
        public async Task<IActionResult> MyShop()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return BadRequest("You cannot view product you must be login ");
            }

            var prod = await shopProductsRepository.GetMyShop(userId);
            return Ok(prod);
        }
        [HttpPut("Edit")]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id ,[FromForm] ShopUserUpdateDTO shopProdDTO , string formFile)
        {
            var shop =  Context.shopsProducts.FirstOrDefault(x => x.Id == id);
            if (shop != null)
            {
                 shopProductsRepository.UpdateById(id, shopProdDTO,formFile);
                return Ok("Updated successfully");
            }
            return BadRequest("Id incorrect");
        }
        [HttpDelete("Delete")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return BadRequest("You cannot view product you must be login ");
            }
            var shop = Context.shopsProducts.FirstOrDefault(x => x.Id == id);
            if (shop != null)// && shopProduct == null )
            {
                //   var shopProduct = productRepository.GetMyProduct(userId)//,id);
                var shopProduct = Context.products.Include(r => r.Reviews).Include(p => p.Category).Include(p => p.Images)
                .Where(u => u.UserId == userId).Where(s => s.ShopId == id)
                 .ToList();
                if (!shopProduct.Any())
                {
                    shopProductsRepository.DeleteById(id, userId);
                    return Ok("Removed successfully");
                }

            }
            return BadRequest("Shop product contains product");
        }
        }
    }
