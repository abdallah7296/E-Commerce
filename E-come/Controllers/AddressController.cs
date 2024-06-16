using E_come.DTO.AdressDTO;
using E_come.Model;
using E_come.services.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_come.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AddressController : ControllerBase
    {
        private readonly IAddressRepository addressRepository;
        private readonly DBContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public AddressController(IAddressRepository _addressRepository,DBContext _dBContext,UserManager<ApplicationUser> _userManager)
        {
            addressRepository = _addressRepository;
            context = _dBContext;
            userManager = _userManager;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm]AddressDTO adressDTO)
        {
            // var userId = ApplicationUser.FindFirst(ClaimTypesNameIdenti.fier)?.Value;
            ApplicationUser user = await userManager.GetUserAsync(User);
           
            await addressRepository.Add(adressDTO,user.Id);

            return Ok("Created Done");
        }
        [HttpGet("GetAllAddress")]
        public async Task<IActionResult> GetAllAddress()
        {
            var products = await addressRepository.GetAllAddressesAsync();
            return Ok(products);
        }
        [HttpPut("update")]
        public IActionResult update(int id,[FromForm] AddressDTO addressDTO)
        {
            if (ModelState.IsValid)
            {
                address addresses = context.addresses.FirstOrDefault(p => p.Id == id);
                if (addresses != null)
                {
                    addressRepository.UpdateById(id, addressDTO);
                    return Ok("Update Done");
                }
                return BadRequest("Id Not Vaild");
            }
            return BadRequest();
        }
        [HttpGet("{id:int}", Name = "GetOneRoute")]
        public async Task<IActionResult> GetById(int id)
        {
            var address = await addressRepository.GetById(id);

            if (address == null)
            {
                return NotFound();
            }
            return Ok(address);
        }
    }
}
