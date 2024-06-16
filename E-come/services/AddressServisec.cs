using E_come.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using E_come.services.IRepository;
using E_come.DTO.AdressDTO;

namespace E_come.services
{
    public class AddressServisec : IAddressRepository
    {
        private readonly DBContext context;
        private readonly UserManager<ApplicationUser> userManager;
        public AddressServisec(DBContext _context, UserManager<ApplicationUser> _userManager)
        {
            context = _context;
            userManager = _userManager;
        }

        public async Task Add(AddressDTO adressDTO,string userId)
        {
            //ApplicationUser applicationUser = new ApplicationUser();
            // var userId = ApplicationUser  //userManager.Users.First().Id; 


            //  var userId = ApplicationUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            address address = new address()
            {
                country = adressDTO.country,
                city = adressDTO.city,
                region = adressDTO.region,
                street_number = adressDTO.street_number,
                zipCode = adressDTO.zipCode,
                Unit_number = adressDTO.phone_number,
                UserName = userId
            };

            context.addresses.Add(address);
            context.SaveChanges();

        }

        public void DeleteById(int id)
        {
            address address = context.addresses.FirstOrDefault(r => r.Id == id);
             context.addresses.Remove(address);
            context.SaveChanges();
        }
        public async Task<List<address>> GetAllAddressesAsync()
        {
            var addresses = await context.addresses.ToListAsync();
            return addresses;
        }

        //public async Task<List<AddressDTO>> GetAll()
        //{
        //   // AddressDTO addressDTO = new AddressDTO();
        //    var addresses  = await addressRepository.GetAllAddressesAsync();
        //   return  addresses.Select(p => new AddressDTO
        //    {

        //        Unit_number = p.Unit_number,
        //        street_number = p.street_number, 
        //        zipCode = p.zipCode,
        //        city = p.city,
        //        country = p.country,
        //        region = p.region,

        //    }).ToList();
   
        //}

        public async Task<address> GetById(int id)
        {
            address address = await context.addresses.FirstOrDefaultAsync(a => a.Id == id);
 
            return (address);
        }

        public Task<address> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public void UpdateById(int id, AddressDTO addressDTO)
        {
            address OldAdress = context.addresses.FirstOrDefault(r => r.Id == id);
            
            OldAdress.Unit_number = addressDTO.phone_number;
            OldAdress.street_number = addressDTO.street_number; 
            OldAdress.city = addressDTO.city;
            OldAdress.country = addressDTO.country;
            OldAdress.zipCode = addressDTO.zipCode;
            OldAdress.Unit_number = addressDTO.phone_number;
          
            context.SaveChanges();
        }
    }
}
