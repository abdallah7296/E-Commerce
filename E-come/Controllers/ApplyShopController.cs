using E_come.DTO.AccountDTO;
using E_come.DTO.AdressDTO;
using E_come.DTO.ApplyShopDTO;
using E_come.DTO.ProductDTO;
using E_come.DTO.ShopDTO;
using E_come.Migrations;
using E_come.Model;
using E_come.services;
using E_come.services.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace E_come.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplyShopController : ControllerBase
    {
        private readonly IApplyShopRepository applyShopRepository;
        private readonly DBContext context;

        public ApplyShopController(IApplyShopRepository applyShopRepository,DBContext context)

        {
            this.applyShopRepository = applyShopRepository;
            this.context = context;
        }

        [HttpPost("ApplayShop")]
        public async Task<IActionResult> ApplayShop([FromForm] ApplayShopDto applyShopDto, IFormFile IDPhoto)
        {  
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
           if (userName == null)
           { return BadRequest("You cannot add product to product you must be login "); }

            var shop = context.applyShops.FirstOrDefault(x => x.BusinessName == applyShopDto.BusinessName);
            if (shop != null)
            {
                return BadRequest("Shop already exists");
            }
            var result  = await applyShopRepository.Create(applyShopDto, IDPhoto, userName);
            return Ok(applyShopDto);
        }

        [HttpGet("GetAllApply")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllApply()
        {
            var apply = await applyShopRepository.GetAll();
            return Ok(apply);
        }
        [HttpDelete("DeleteById")]
        [Authorize(Roles = "Admin")]
        public  IActionResult DeleteById(int Id)
        {
           applyShopRepository.DeleteById(Id);

            return Ok("Delete successfully");
        }

        [HttpGet("GetById", Name = "GetOneOrdRoute2")]
        public async Task<IActionResult> GetById(int id)
        {
            var cat = await applyShopRepository.GetById(id);

            if (cat == null)
            {
                return NotFound();
            }
            return Ok(cat);
        }

    }
}
