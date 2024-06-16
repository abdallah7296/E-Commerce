using E_come.DTO.ProductDTO;
using E_come.DTO.ReviewDTO;
using E_come.DTO.ShopDTO;
using E_come.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_come.services
{
    public class ProductServisec : IProductRepository
    {
        private readonly DBContext context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> userManager;

        public ProductServisec(DBContext _context, IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor,UserManager<ApplicationUser> userManager)
        {
            context = _context;
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
        }

        public async Task<string> Add(ProdWithCategory prodWithCategory, List<IFormFile> ImageFiles, string userId)
        {
            var user = await userManager.Users.SingleAsync(u => u.Id == userId);
            if (userId == null)
                return "You cannot add review to product you must be login ";
            Product product = new Product();
            
            product.price = prodWithCategory.priceProd;
            product.Item_Name = prodWithCategory.Item_Name;
            product.Description = prodWithCategory.Descrip;
            product.quantity = prodWithCategory.quantityProd;
            product.solditems = prodWithCategory.solditemsProd;
            Category category = context.Categories.FirstOrDefault(s => s.Name == prodWithCategory.categoryName);
            product.CategoryId = category.Id;
            List<ProductImages> images = new List<ProductImages>();
            foreach (var file in ImageFiles)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");

                Directory.CreateDirectory(uploadsFolder);
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                ProductImages image = new ProductImages();
                image.ImagePath = uniqueFileName;
                image.ProductId = product.Id;


                await context.Images.AddAsync(image);
                images.Add(image);

            }
            product.Images = images;

            await context.products.AddAsync(product);
            context.SaveChanges();
            return $"{product}";
           

        }

        public void DeleteById(int id)
        {
            Product oldprod = context.products.FirstOrDefault(p => p.Id == id);
            context.products.Remove(oldprod);
            context.SaveChanges();

        }

        public async Task<List<productDTO>> GetAll()
        {

            List<Product> products = await context.products.Include(r => r.Reviews).Include(p => p.Category).Include(p => p.Images).Include(u => u.Seller)
                  .ToListAsync();

            if (!products.Any())
            { return null; }

            int averageRate;
            List<productDTO> productsDto = new List<productDTO>();
            foreach (var product in products)
            {

                List<Review> reviews = product.Reviews.ToList();
                if (reviews.Count != 0)
                {
                    averageRate = (int)reviews.Average(r => r.Rate);
                    product.averageRate = averageRate;
                    context.SaveChanges();
                }
                else
                {
                    averageRate = 0;
                    product.averageRate = averageRate;
                    context.SaveChanges();
                }

                productDTO pdto = new productDTO
                {
                    ID = product.Id,
                    imageUrls = new List<string>(),
                    priceProd = product.price,
                    Item_Name = product.Item_Name,
                    Descrip = product.Description,
                    quantityProd = product.quantity,
                    solditemsProd = product.solditems,
                    CategoryName = product.Category.Name,
                    AverageRate = (int)product.averageRate,
                };

                List<ReviewDTo> reviewDTos = new List<ReviewDTo>();
                foreach (var item in product.Reviews)
                { 
                    reviewDTos.Add(new ReviewDTo
                    {
                        Rating =item.Rate,
                        Comment = item.Comment,
                    });
                }
                pdto.RevDTO = reviewDTos;


                HttpContext httpContext = _httpContextAccessor.HttpContext;
                var images = context.Images.Where(m => m.ProductId == product.Id);
                foreach (var item in images)
                {
                    pdto.imageUrls.Add($"{httpContext.Request.Scheme}://{httpContext.Request.Host}/images/{item.ImagePath}");
                }

               
                var shop = context.shopsProducts.FirstOrDefault(p => p.Id == product.ShopId);
                ShopDto shopDto = new ShopDto
                {
                    Id = product.Id,
                    Name = shop.Name,
                    PhoneNumber = shop.PhoneNumber,
                    TheDoorNumber = shop.TheDoorNumber
                }; 
                pdto.Shop = shopDto;

                productsDto.Add(pdto);
            }
            return productsDto;
        }
        public async Task<productDTO> GetById(int id)
        {
            Product product = await context.products.Include(r => r.Reviews).Include(c => c.Category).Include(p => p.Images).Include(p => p.ShopProducts).FirstOrDefaultAsync(c => c.Id == id);
           
            var images = context.reviews.Where(m => m.ProductId == product.Id);
            int averageRate;
            List<Review> reviews = product.Reviews.ToList();
            if (reviews.Count != 0)
            {
                averageRate = (int)reviews.Average(r => r.Rate);
                product.averageRate = averageRate;
                context.SaveChanges();

            }
            else
            {
                averageRate = 0;
                product.averageRate = averageRate;
                context.SaveChanges();
            }

            productDTO pdto = new productDTO
            {
                    ID = product.Id,
                    imageUrls = new List<string>(),
                    priceProd = product.price,
                    Item_Name = product.Item_Name,
                    Descrip = product.Description,
                    quantityProd = product.quantity,
                    solditemsProd = product.solditems,
                    CategoryName = product.Category.Name,
                    AverageRate = averageRate,
            };


            HttpContext httpContext = _httpContextAccessor.HttpContext;
            var imag = context.Images.Where(m => m.ProductId == product.Id);
            foreach (var image in imag)
            {
                string imageUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/images/{image.ImagePath}";
                pdto.imageUrls.Add(imageUrl);
        
            }

            var shop = context.shopsProducts.FirstOrDefault(p => p.Id == product.ShopId);
            ShopDto shopDto = new ShopDto
            {
                Id = product.Id,
                Name = shop.Name,
                PhoneNumber = shop.PhoneNumber,
                TheDoorNumber = shop.TheDoorNumber
            };
            pdto.Shop = shopDto;

           // List<Review> review =  context.reviews.ToList();
            List<ReviewDTo> reviewDTos = new List<ReviewDTo>();
            foreach (var item in product.Reviews)
            {
                reviewDTos.Add(new ReviewDTo
                {
                    Rating = item.Rate,
                    Comment = item.Comment,
                });

            }
            pdto.RevDTO = reviewDTos;

            return pdto;
        }

        public async Task<List<productDTO>> GetByName(string item)
        {
            List<Product> products = await context.products.Include(s => s.ShopProducts).Include(r => r.Reviews).Include(p => p.Category).Include(p => p.Images).Include(u => u.Seller)
                   .ToListAsync();
            if (!products.Any())
                return null;

            if (!string.IsNullOrEmpty(item))
            {
                products = products.Where(p => p.Item_Name.Contains(item) ||
                p.price.ToString().Contains(item) ||
                p.Id.ToString().Contains(item) ||
                p.Category.Name.Contains(item) ||
                p.ShopProducts.TheDoorNumber.Contains(item) ||
                p.ShopProducts.Name.Contains(item)

                ).ToList();//|| p.Salary.ToString().Contains(query)
            }
            
            int averageRate;
            List<productDTO> productsDto = new List<productDTO>();
            foreach (var product in products)
            {

                List<Review> reviews = product.Reviews.ToList();
                if (reviews.Count != 0)
                {
                    averageRate = (int)reviews.Average(r => r.Rate);
                    product.averageRate = averageRate;
                    context.SaveChanges();

                }
                else
                {
                    averageRate = 0;
                    product.averageRate = averageRate;
                    context.SaveChanges();
                }

                productDTO pdto = new productDTO
                {
                    ID = product.Id,
                    imageUrls = new List<string>(),
                    priceProd = product.price,
                    Item_Name = product.Item_Name,
                    Descrip = product.Description,
                    quantityProd = product.quantity,
                    solditemsProd = product.solditems,
                    CategoryName = product.Category.Name,
                    AverageRate = (int)product.averageRate,

                };

                List<ReviewDTo> reviewDTos = new List<ReviewDTo>();
                foreach (var items in product.Reviews)
                { 
                    reviewDTos.Add(new ReviewDTo
                    {
                        Rating =items.Rate,
                        Comment = items.Comment,
                    });
                }
                pdto.RevDTO = reviewDTos;


                HttpContext httpContext = _httpContextAccessor.HttpContext;
                var images = context.Images.Where(m => m.ProductId == product.Id);
                foreach (var items in images)
                {
                    pdto.imageUrls.Add($"{httpContext.Request.Scheme}://{httpContext.Request.Host}/images/{items.ImagePath}");
                }

               
                var shop = context.shopsProducts.FirstOrDefault(p => p.Id == product.ShopId);
                ShopDto shopDto = new ShopDto
                {
                    Id = product.Id,
                    Name = shop.Name,
                    PhoneNumber = shop.PhoneNumber,
                    TheDoorNumber = shop.TheDoorNumber
                }; 
                pdto.Shop = shopDto;

                productsDto.Add(pdto);
            }
            return productsDto;
         
            
        }

        public void UpdateById(int id, ProdWithCategory prodWithCategory, List<IFormFile> ImageFiles)
        {
            Product product = context.products.Include(p => p.Images).FirstOrDefault(c => c.Id == id);

            product.price = prodWithCategory.priceProd;
            product.Item_Name = prodWithCategory.Item_Name;
            product.Description = prodWithCategory.Descrip;
            product.quantity = prodWithCategory.quantityProd;
            product.solditems = prodWithCategory.solditemsProd;
            Category category = context.Categories.FirstOrDefault(s => s.Name == prodWithCategory.categoryName);
            product.CategoryId = category.Id;
            var shop = context.shopsProducts.FirstOrDefault(s => s.Name == prodWithCategory.nameShop);
            product.ShopId = shop.Id;

            if (ImageFiles != null && ImageFiles.Count > 0)
            {
                List<ProductImages> images = new List<ProductImages>();

                foreach (var file in ImageFiles)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    Directory.CreateDirectory(uploadsFolder);
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    ProductImages image = new ProductImages();
                    image.ImagePath = uniqueFileName;
                    image.ProductId = product.Id;

                    context.Images.Add(image);
                    images.Add(image);
                }

                product.Images.Clear();
                product.Images = images;
            }


            context.products.Update(product);
            context.SaveChanges();
        }

        public async Task<int> Create(Product product, List<IFormFile> ImageFiles)//, string userId)
        {
            //var user = await userManager.Users.SingleAsync(u => u.Id == userId);
           
            
            List<ProductImages> images = new List<ProductImages>();
            foreach (var file in ImageFiles)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");

                Directory.CreateDirectory(uploadsFolder);
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                ProductImages image = new ProductImages();
                image.ImagePath = uniqueFileName;
                image.ProductId = product.Id;


                context.Images.Add(image);
                images.Add(image);

            }
            product.Images = images;

            context.products.Add(product);
            return context.SaveChanges();
           

        }

        public Product Mapping(ProdWithCategory prodWithCategory,string userId)
        {
            //var user =  userManager.Users.SingleAsync(u => u.Id == userId);
            var category = context.Categories.FirstOrDefault(s => s.Name == prodWithCategory.categoryName);
            var shop = context.shopsProducts.FirstOrDefault(s => s.Name == prodWithCategory.nameShop);

            Product product = new Product
            {
                price = prodWithCategory.priceProd,
                Item_Name = prodWithCategory.Item_Name,
                Description = prodWithCategory.Descrip,
                quantity = prodWithCategory.quantityProd,
                solditems = prodWithCategory.solditemsProd,
                CategoryId = category.Id,
                ShopId = shop.Id,
                UserId = userId
            };    
            return product;
        }

        public async Task<List<ProdRev>> GetAllBySortReview()
        {
            //int averageRate ;
            //List<Product>  products = await context.products.Include(p=>p.Reviews)
            // .ToListAsync();
            //if (!products.Any())
            //    return null;
            
            //foreach (var pro in products)
            //{
            //    List<Review> reviews =   pro.Reviews.ToList();
            //    if (reviews.Count == 0)
            //        continue;
            //     averageRate = (int)reviews.Average(r => r.Rate);
            //    pro.averageRate = averageRate;
            //    context.SaveChanges();
            //}


            List<Product> products1 = await context.products.Include(r => r.Reviews).Include(p => p.Category).Include(p => p.Images).Include(u => u.Seller)
                  .OrderByDescending(d => d.averageRate).ToListAsync();
            

             if (!products1.Any())
                return null;

            int averageRate;
            List<ProdRev> productsDto = new List<ProdRev>();
            foreach (var product in products1)
            {
                List<Review> reviews = product.Reviews.ToList();
                if (reviews.Count != 0)
                {
                    averageRate = (int)reviews.Average(r => r.Rate);
                    product.averageRate = averageRate;
                    context.SaveChanges();

                }
                else
                {
                    averageRate = 0;
                    product.averageRate = averageRate;
                    context.SaveChanges();
                }

                ProdRev pdto = new ProdRev
                {
                    ID = product.Id,
                    imageUrls = new List<string>(),
                    priceProd = product.price,
                    Item_Name = product.Item_Name,
                    Descrip = product.Description,
                    quantityProd = product.quantity,
                    solditemsProd = product.solditems,
                    CategoryName = product.Category.Name,
                    AverageRate  = (int)product.averageRate,

                };
                List<Product> products2 = await context.products.OrderByDescending(d => d.averageRate).ToListAsync();

                HttpContext httpContext = _httpContextAccessor.HttpContext;
                var images = context.Images.Where(m => m.ProductId == product.Id);
                foreach (var item in images)
                {
                    pdto.imageUrls.Add($"{httpContext.Request.Scheme}://{httpContext.Request.Host}/images/{item.ImagePath}");
                }


                var shop = context.shopsProducts.FirstOrDefault(p => p.Id == product.ShopId);
                ShopDto shopDto = new ShopDto
                {
                    Id = product.Id,
                    Name = shop.Name,
                    PhoneNumber = shop.PhoneNumber,
                    TheDoorNumber = shop.TheDoorNumber
                };
                pdto.Shop = shopDto;

                productsDto.Add(pdto);
            }
            return productsDto;


        }

        public async Task<List<productDTO>> GetMyProduct(string userId)
        {
            List<Product> products = await context.products.Include(r => r.Reviews).Include(p => p.Category).Include(p => p.Images).Where(u => u.UserId == userId)
                 .ToListAsync();
            List<productDTO> productsDto = new List<productDTO>();
            foreach (var product in products)
            {
                productDTO pdto = new productDTO
                {
                    ID = product.Id,
                    imageUrls = new List<string>(),
                    priceProd = product.price,
                    Item_Name = product.Item_Name,
                    Descrip = product.Description,
                    quantityProd = product.quantity,
                    solditemsProd = product.solditems,
                    CategoryName = product.Category.Name,

                };

                // List<Review> reviews = context.reviews.FirstOrDefault(r => r.Id == product. );
                List<ReviewDTo> reviewDTos = new List<ReviewDTo>();
                foreach (var item in product.Reviews)
                {
                    reviewDTos.Add(new ReviewDTo
                    {
                        Rating = item.Rate,
                        Comment = item.Comment,
                    });

                }
                pdto.RevDTO = reviewDTos;


                HttpContext httpContext = _httpContextAccessor.HttpContext;
                var imag = context.Images.Where(m => m.ProductId == product.Id);
                foreach (var image in imag)
                {
                    string imageUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/images/{image.ImagePath}";
                    pdto.imageUrls.Add(imageUrl);
                }

                var shop = context.shopsProducts.FirstOrDefault(p => p.Id == product.ShopId);
                ShopDto shopDto = new ShopDto
                {
                    Id = product.Id,
                    Name = shop.Name,
                    PhoneNumber = shop.PhoneNumber,
                    TheDoorNumber = shop.TheDoorNumber
                };
                pdto.Shop = shopDto;

                productsDto.Add(pdto);
            }
            return productsDto;
        }

        public async Task<List<productDTO>> GetProducts(int numberOfProducts)
        {
            var products = await context.products.Include(r => r.Reviews).Include(p => p.Category).Include(p => p.Images).Take(numberOfProducts).ToListAsync();
            List<productDTO> productsDto = new List<productDTO>();
            foreach (var product in products)
            {
                productDTO pdto = new productDTO
                {
                    ID = product.Id,
                    imageUrls = new List<string>(),
                    priceProd = product.price,
                    Item_Name = product.Item_Name,
                    Descrip = product.Description,
                    quantityProd = product.quantity,
                    solditemsProd = product.solditems,
                    CategoryName = product.Category.Name,

                };

                // List<Review> reviews = context.reviews.FirstOrDefault(r => r.Id == product. );
                List<ReviewDTo> reviewDTos = new List<ReviewDTo>();
                foreach (var item in product.Reviews)
                {
                    reviewDTos.Add(new ReviewDTo
                    {
                        Rating = item.Rate,
                        Comment = item.Comment,
                    });

                }
                pdto.RevDTO = reviewDTos;


                HttpContext httpContext = _httpContextAccessor.HttpContext;
                var imag = context.Images.Where(m => m.ProductId == product.Id);
                foreach (var image in imag)
                {
                    string imageUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/images/{image.ImagePath}";
                    pdto.imageUrls.Add(imageUrl);
                }

                var shop = context.shopsProducts.FirstOrDefault(p => p.Id == product.ShopId);
                ShopDto shopDto = new ShopDto
                {
                    Id = product.Id,
                    Name = shop.Name,
                    PhoneNumber = shop.PhoneNumber,
                    TheDoorNumber = shop.TheDoorNumber
                };
                pdto.Shop = shopDto;

                productsDto.Add(pdto);
            }
            return productsDto;

        }
    }
}
