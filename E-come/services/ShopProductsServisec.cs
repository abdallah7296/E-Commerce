using E_come.DTO.DTOAccount;
using E_come.DTO.ProductDTO;
using E_come.DTO.ReviewDTO;
using E_come.DTO.ShopDTO;
using E_come.Model;
using E_come.services.Business;
using E_come.services.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.Documents;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Runtime.InteropServices;

namespace E_come.services
{
    public class ShopProductsServisec : IShopProductsRepository
    {
        private readonly DBContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEmailSender emailSender;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ShopProductsServisec(DBContext _context,UserManager<ApplicationUser> userManager,IEmailSender emailSender,
            IHttpContextAccessor httpContextAccessor , IWebHostEnvironment webHostEnvironment)
        {
            context = _context;
            this.userManager = userManager;
            this.emailSender = emailSender;
            this.httpContextAccessor = httpContextAccessor;
            this.webHostEnvironment = webHostEnvironment;
        }


        /* public async Task<AuthModel> Add(ShopUserDto shopDto, IFormFile iamgefile)
         {

             AuthModel authModel = new AuthModel();
             var user = await userManager.FindByIdAsync(shopDto.USerId);

             if (user == null)
             { return new AuthModel { Message = "The user Id not Found  " }; }
             else
             {
                 ShopProducts shop = new ShopProducts();
                 shop.Name = shopDto.Name;
                 shop.PhoneNumber = shopDto.PhoneNumber;
                 shop.TheDoorNumber = shopDto.TheDoorNumber;
                 shop.UserId = shopDto.USerId;
                 shop.Email = shopDto.Email;
                 string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images/ImagesShop");

                 Directory.CreateDirectory(uploadsFolder);
                 string uniqueFileName = Guid.NewGuid().ToString() + "_" + iamgefile.FileName;
                 string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                 using (var stream = new FileStream(filePath, FileMode.Create))
                 {
                     iamgefile.CopyTo(stream);
                 }

                 shop.imagePath = uniqueFileName;
                 var result = await userManager.AddToRoleAsync(user, "Seller");
                 context.shopsProducts.Add(shop);
                 context.SaveChanges();

                 await emailSender.SendEmailAsync(shop.Email, "Postan Mole", $"Welocme {shop.Email} the shope Created  ( {shop.Name} ) done successfully can be Add product ");
                 return new AuthModel { Message = "The user Id not Found  ", IsAuthenticated = true }; ;
             }
         }*/

        public async Task<AuthModel> Add(ShopUserDto shopDto, string imageUrl)
        {
            AuthModel authModel = new AuthModel();
            var user = await userManager.FindByIdAsync(shopDto.USerId);

            if (user == null)
            {
                return new AuthModel { Message = "The user Id not Found  " };
            }
            else
            {
                ShopProducts shop = new ShopProducts();
                shop.Name = shopDto.Name;
                shop.PhoneNumber = shopDto.PhoneNumber;
                shop.TheDoorNumber = shopDto.TheDoorNumber;
                shop.UserId = shopDto.USerId;
                shop.Email = shopDto.Email;

                // Set the imagePath to the received URL
                shop.imagePath = imageUrl;

                var result = await userManager.AddToRoleAsync(user, "Seller");
                context.shopsProducts.Add(shop);
                context.SaveChanges();

                await emailSender.SendEmailAsync(shop.Email, "Postan Mole", $"Welocme {shop.Email} the shop Created  ( {shop.Name} ) done successfully can be Add product ");
                return new AuthModel { Message = "The user Id not Found  ", IsAuthenticated = true }; ;
            }
        }

        public void DeleteById(int id, string userId)
        {

            // var shopProducts = context.shopsProducts.Include(P => P.Products).ThenInclude(c => c.Category).Where(u => u.UserId == userId).ToListAsync();
            var shop = context.shopsProducts.FirstOrDefault(c => c.Id == id);
            if (shop != null)
            {
                context.shopsProducts.Remove(shop);
                context.SaveChanges();
            }
        }


        public async Task<List<ShopDto>> GetAllShop()
        {
            List<ShopProducts> shopProducts =   await context.shopsProducts.Include(P => P.Products).ThenInclude(c=> c.Category).Include(u => u.Seller).ToListAsync();
            HttpContext httpContext = httpContextAccessor.HttpContext;
            List<ShopDto> shopDtos = new List<ShopDto>();
            foreach (var shopproduct in shopProducts )
            {
                 //var prod = context.products.Where(m => m.Id == shopproduct.Id).Include(p => p.Category).Include(u => u.Seller);
                List<productDTO> productsDto = new List<productDTO>();
                foreach (var product in shopproduct.Products)
                {
                    int averageRate = 0;
                    var reviewsAvg = context.reviews.Where(r => r.ProductId == product.Id).ToList();
                    if (reviewsAvg.Count != 0)
                    {
                        averageRate = (int)reviewsAvg.Average(r => r.Rate);
                        product.averageRate = averageRate;
                        context.SaveChanges();
                    }
                    else
                    {
                        averageRate = 0;
                        product.averageRate = averageRate;
                        context.SaveChanges();
                    }
                    productDTO pdto = new productDTO();
                    pdto.imageUrls = new List<string>();
                    pdto.ID = product.Id;
                    pdto.priceProd = product.price;
                    pdto.Item_Name = product.Item_Name;
                    pdto.Descrip = product.Description;
                    pdto.quantityProd = product.quantity;
                    pdto.solditemsProd = product.solditems;
                    pdto.CategoryName = product.Category.Name;
                    pdto.AverageRate = (int)product.averageRate;


                    var images = context.Images.Where(m => m.ProductId == product.Id);
                    foreach (var image in images)
                    {
                        string imageurl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/images/{image.ImagePath}" ;
                        pdto.imageUrls.Add(imageurl);
                        
                    }
                    productsDto.Add(pdto);

                    var review = context.reviews.Where(c => c.ProductId == product.Id);
                    List<ReviewDTo> reviewDTos = new List<ReviewDTo>();
                    foreach (var item in review)
                    {
                        reviewDTos.Add(new ReviewDTo
                        {
                            Rating = item.Rate,
                            Comment = item.Comment,
                        });
                    }
                    pdto.RevDTO = reviewDTos;


                }
                shopDtos.Add(new ShopDto
                {
                    productDTOs = productsDto,
                    Id = shopproduct.Id,
                    PhoneNumber = shopproduct.PhoneNumber,
                    TheDoorNumber = shopproduct.TheDoorNumber,
                    Name = shopproduct.Name,
                    imagePath = shopproduct.imagePath      //$"{httpContext.Request.Scheme}://{httpContext.Request.Host}/images/ImagesShop/{shopproduct.imagePath}"

                }); 



            }
            return shopDtos;
        }

        public Task<ShopProducts> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ShopProducts> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ShopDto>> GetMyShop(string userId)
        {
            List<ShopProducts> shopProducts = await context.shopsProducts.Include(P => P.Products).ThenInclude(c => c.Category).Where(u => u.UserId == userId).ToListAsync();
            HttpContext httpContext = httpContextAccessor.HttpContext;
            List<ShopDto> shopDtos = new List<ShopDto>();
            foreach (var shopproduct in shopProducts)
            {
                //var prod = context.products.Where(m => m.Id == shopproduct.Id).Include(p => p.Category).Include(u => u.Seller);
                List<productDTO> productsDto = new List<productDTO>();
                foreach (var product in shopproduct.Products)
                {
                 
                    int averageRate = 0;
                    var reviewsAvg = context.reviews.Where(r => r.ProductId == product.Id).ToList();
                    if (reviewsAvg.Count != 0)
                    {
                        averageRate = (int)reviewsAvg.Average(r => r.Rate);
                        product.averageRate = averageRate;
                        context.SaveChanges();
                    }
                    else
                    {
                        averageRate = 0;
                        product.averageRate = averageRate;
                        context.SaveChanges();
                    }
                    productDTO pdto = new productDTO();
                    pdto.imageUrls = new List<string>();
                    pdto.ID = product.Id;
                    pdto.priceProd = product.price;
                    pdto.Item_Name = product.Item_Name;
                    pdto.Descrip = product.Description;
                    pdto.quantityProd = product.quantity;
                    pdto.solditemsProd = product.solditems;
                    pdto.CategoryName = product.Category.Name;
                    pdto.AverageRate = (int)product.averageRate;

                    var images = context.Images.Where(m => m.ProductId == product.Id).ToList();
                    foreach (var image in images)
                    {
                        string imageurl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/images/{image.ImagePath}";
                        pdto.imageUrls.Add(imageurl);

                    }
                    productsDto.Add(pdto);

                    var review = context.reviews.Where(c => c.ProductId == product.Id).ToList();
                    List<ReviewDTo> reviewDTos = new List<ReviewDTo>();
                    foreach (var item in review)
                    {
                        reviewDTos.Add(new ReviewDTo
                        {
                            Rating = item.Rate,
                            Comment = item.Comment,
                        });
                    }
                    pdto.RevDTO = reviewDTos;     

                }
                shopDtos.Add(new ShopDto
                {
                    productDTOs = productsDto,
                    Id = shopproduct.Id,
                    PhoneNumber = shopproduct.PhoneNumber,
                    TheDoorNumber = shopproduct.TheDoorNumber,
                    Name = shopproduct.Name,
                    imagePath = shopproduct.imagePath //  $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/images/ImagesShop/{shopproduct.imagePath}"

                });
            }
            return shopDtos;
        }

        /* public void UpdateById(int id, ShopUserUpdateDTO shopDto, IFormFile file)
         {
            var shop = context.shopsProducts.FirstOrDefault(c => c.Id == id);
             if (shop != null)
             {            
                 shop.Name = shopDto.Name;
                 shop.PhoneNumber = shopDto.PhoneNumber;
                 shop.TheDoorNumber = shopDto.TheDoorNumber;
                 string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images/ImagesShop");

                 Directory.CreateDirectory(uploadsFolder);
                 string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                 string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                 using (var stream = new FileStream(filePath, FileMode.Create))
                 {
                     file.CopyTo(stream);
                 }
                 shop.imagePath = uniqueFileName;
             }
             context.SaveChanges();

         }*/


        public void UpdateById(int id, ShopUserUpdateDTO shopDto, string imageUrl)
        {
            var shop = context.shopsProducts.FirstOrDefault(c => c.Id == id);
            if (shop != null)
            {
                shop.Name = shopDto.Name;
                shop.PhoneNumber = shopDto.PhoneNumber;
                shop.TheDoorNumber = shopDto.TheDoorNumber;

                // التحقق مما إذا كان الرابط يحتوي على الصورة نفسها أو يشير إلى موقعها
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    if (Uri.IsWellFormedUriString(imageUrl, UriKind.Absolute))
                    {
                        // إذا كان الرابط يشير إلى موقع الصورة، يمكنك استخدامه مباشرة
                        shop.imagePath = imageUrl;
                    }
                    else
                    {
                        // إذا كان الرابط يشير إلى الصورة نفسها، عليك تحميل الصورة وحفظها في المجلد المناسب
                        string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images/ImagesShop");
                        Directory.CreateDirectory(uploadsFolder);
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageUrl);
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        // قم بتنزيل الصورة من الرابط
                        using (var client = new WebClient())
                        {
                            client.DownloadFile(imageUrl, filePath);
                        }

                        // تحديث مسار الصورة في قاعدة البيانات
                        shop.imagePath = uniqueFileName;
                    }
                }
            }
            context.SaveChanges();
        }
    }
}
