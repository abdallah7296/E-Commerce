using E_come.DTO.AccountDTO;
using E_come.DTO.AdressDTO;
using E_come.DTO.ApplyShopDTO;
using E_come.DTO.ShopDTO;
using E_come.Migrations;
using E_come.Model;
using E_come.services.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_come.services
{
    public class ApplyShopServices : IApplyShopRepository
    {
        private readonly DBContext context;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ApplyShopServices(DBContext context, IWebHostEnvironment webHostEnvironment,UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            this.webHostEnvironment = webHostEnvironment;
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task<int> Create(ApplayShopDto applyShopDto, IFormFile IDPhoto, string UserName)
        {
          

            string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");

            Directory.CreateDirectory(uploadsFolder);
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + IDPhoto.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                IDPhoto.CopyTo(stream);
            }

            ApplyShop applyShop = new ApplyShop
            {
                TheDoorNumber = applyShopDto.TheDoorName,
                BusinessName = applyShopDto.BusinessName,
                BusinessPhone = applyShopDto.BusinessPhone,
                ISBN = applyShopDto.ISBN,
                Email = applyShopDto.Email,
                IdPhoto = uniqueFileName,
                Country = applyShopDto.Country,
                UserName = UserName,
            };

            context.applyShops.Add(applyShop);
            return context.SaveChanges();
        }

        public  void DeleteById(int id)
        {
            ApplyShop apply = context.applyShops.FirstOrDefault(a => a.Id == id);
            context.applyShops.Remove(apply);
            context.SaveChanges();
        }

        public async Task<List<GetApplyDto>> GetAll()
        {
            HttpContext httpContext = httpContextAccessor.HttpContext;
            var applies = await context.applyShops.ToListAsync();
            List<GetApplyDto> GetApplyDto = new List<GetApplyDto>();
            foreach (var item in applies)
            {
                GetApplyDto.Add(new GetApplyDto
                {
                    Id = item.Id,
                    TheDoorNumber = item.TheDoorNumber,
                    BusinessName = item.BusinessName,
                    BusinessPhone = item.BusinessPhone,
                    Country = item.Country,
                    ISBN = item.ISBN,
                    Email = item.Email,
                    IDPhoto = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/images/{item.IdPhoto}",
                    userId = item.UserName

                });
            }
            return GetApplyDto;

        }

        public async Task<GetApplyDto> GetById(int id)
        {
            var applyShop = await context.applyShops.FirstOrDefaultAsync(a => a.Id == id);

            GetApplyDto applyDto = new GetApplyDto
            { 
                Id = applyShop.Id,
                TheDoorNumber = applyShop.TheDoorNumber,
                BusinessName = applyShop.BusinessName,
                BusinessPhone = applyShop.BusinessPhone,
                Country = applyShop.Country,
                ISBN = applyShop.ISBN,
                Email = applyShop.Email,
                IDPhoto = applyShop.IdPhoto,
                userId = applyShop.UserName
            };

            return applyDto;
        }
    }
}
