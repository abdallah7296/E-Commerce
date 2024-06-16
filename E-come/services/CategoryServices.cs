using E_come.DTO.CategoryDTO;
using E_come.DTO.ProductDTO;
using E_come.DTO.ReviewDTO;
using E_come.DTO.ShopDTO;
using E_come.Model;
using E_come.services.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Climate;
using static System.Net.Mime.MediaTypeNames;

namespace E_come.services
{
    public class CategoryServices : ICategoryRepository
    {
        private readonly DBContext context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;

        public CategoryServices(DBContext _context, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            context = _context;
            _webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
        }

        public void Add(AddCategoryDto categoryDTO, IFormFile file)
        {
            Category category = new Category();
            category.Description = categoryDTO.Description;
            category.Name = categoryDTO.Name;
            category.IsDeleted = categoryDTO.IsDeleted;
            category.TotalProducts = categoryDTO.TotalProducts;

            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/ImagesCategory");

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            category.Picture = uniqueFileName;
            context.Categories.Add(category);
            context.SaveChanges();
        }

        public void DeleteById(int id)
        {
            Category category = context.Categories.FirstOrDefault(r => r.Id == id);
            if (category.Products == null)
            context.Categories.Remove(category);
            else
            {
                foreach (var product in category.Products)
                {
                    context.products.Remove(product);
                }
                context.Categories.Remove(category);
                context.SaveChanges();
            }
         
        }

        public List<CategoryDTO> GetAll()
        {
            HttpContext httpContext = httpContextAccessor.HttpContext;
            List<Category> categories =context.Categories.Include(p => p.Products).ThenInclude(r=> r.Reviews).ToList();
            List<CategoryDTO> categoriesDTO = new List<CategoryDTO>();
            foreach (var category in categories)
            {
                CategoryDTO categoryDTO = new CategoryDTO();
                categoryDTO.categoryId = category.Id;
                categoryDTO.Description = category.Description;
                categoryDTO.Name = category.Name;
                categoryDTO.IsDeleted = category.IsDeleted;
                categoryDTO.Picture = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/images/ImagesCategory/{category.Picture}";

           
                List<productDTO> productsDto = new List<productDTO>();
                foreach (var product in category.Products)
                {
                    int averageRate = 0;
                     var reviewsAvg = product.Reviews.ToList();
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
                        pdto.CategoryName = category.Name;
                        pdto.AverageRate = (int)product.averageRate;
                    
                    var images = context.Images.Where(m => m.ProductId == product.Id);
                    foreach (var item in images)
                    {
                        pdto.imageUrls.Add($"{httpContext.Request.Scheme}://{httpContext.Request.Host}/images/{item.ImagePath}");
                    }
                   
                    var reviews = product.Reviews.ToList();
                    List<ReviewDTo> reviewDTos = new List<ReviewDTo>();
                    foreach (var item in reviews)
                    {
                        reviewDTos.Add(new ReviewDTo
                        {
                            Comment = item.Comment,
                            Rating = item.Rate,
                        });
                    }
                    pdto.RevDTO = reviewDTos;

                    ShopProducts shop = context.shopsProducts.FirstOrDefault(p => p.Id == product.ShopId);
                        ShopDto shopDto = new ShopDto();
                        shopDto.Id = shop.Id;
                        shopDto.Name = shop.Name;
                        shopDto.PhoneNumber = shop.PhoneNumber;
                        shopDto.TheDoorNumber = shop.TheDoorNumber;
                        pdto.Shop = shopDto;
                    productsDto.Add(pdto);
                }
                categoryDTO.productDTOs = productsDto;
                categoriesDTO.Add(categoryDTO);
            }
            return  categoriesDTO;

        }

        public Task<Category> GetById(int id)
        {
            Category category = context.Categories.Include(p => p.Products).FirstOrDefault(c=> c.Id == id);
            return Task.FromResult(category);
         }

        public Task<Category> GetByName(string name)
        {
            Category category = context.Categories.Include(p => p.Products).FirstOrDefault(c => c.Name == name);
            return Task.FromResult(category);
        }

        public void UpdateById(int id, UpdateCategoryDto categoryDTO, IFormFile file)
        {
            var Oldcategory = context.Categories.FirstOrDefault(r => r.Id == id);
            Oldcategory.Description = categoryDTO.Description;
            Oldcategory.Name = categoryDTO.Name;
            Oldcategory.IsDeleted = categoryDTO.IsDeleted;
            Oldcategory.TotalProducts = categoryDTO.TotalProducts;

            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/ImagesCategory");
            //Directory.CreateDirectory(uploadsFolder);
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            Oldcategory.Picture = uniqueFileName;
            context.SaveChanges();
        }
    }
}
