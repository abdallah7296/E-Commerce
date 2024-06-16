using E_come.DTO.CartDTO;
using E_come.Migrations;
using E_come.Model;
using E_come.services.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace E_come.services
{
    public class CartServices : ICartReposatory
    {
        private readonly DBContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ClaimsPrincipal User;

        public CartServices(DBContext context,UserManager<ApplicationUser> userManager,IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task<string> AddToCard(int productID, string userId, int quantity)
        {
            // user = await userManager.GetUserAsync(User);
            var user = await userManager.Users.SingleAsync(u => u.Id == userId );
            var product = await context.products.FindAsync(productID);
            if (product != null)
            {
                var duplicateProduct = await context.userCarts.SingleOrDefaultAsync(c => c.User.Id == userId && c.Product.Id == productID);
                if (duplicateProduct != null)
                {
                    duplicateProduct.Qunatity += quantity;
                    await context.SaveChangesAsync();
                    return "Product Addedd Successfully";
                }
                await context.userCarts.AddAsync(new UserCart
                {
                    Date = DateTime.Now,
                    Qunatity = quantity,
                    Product = product,
                    User = user,
                });
                await context.SaveChangesAsync();
                return "Product Addedd Successfully";
            }
            return "Sorry there is problem within the system";
        }

        public async Task<string> RemoveFromCard(int productID, string userId, int quantity)
        {
            var user = await userManager.Users.Include(u => u.MyCart).SingleAsync(u => u.Id == userId);
            var Product = await context.userCarts.Include(c => c.Product).FirstOrDefaultAsync(c => c.User.Id == userId && c.Product.Id == productID);
            if (Product != null && (quantity == Product.Qunatity || quantity > Product.Qunatity))
            {
                context.userCarts.Remove(Product);
                context.SaveChanges();
                return "Product Removed Successfully";
            }
            else if (Product != null && Product.Qunatity > quantity)
            {
                Product.Qunatity -= quantity;
                context.SaveChanges();
                return "Done";
            }
            else
                return "Sorry there is problem within the system";
        }

        public async Task<string> TotalPayment(string userId)
        {
            var user = await userManager.Users.Include(u => u.MyOrders).Include(u => u.MyCart).SingleAsync(u => u.Id == userId);
            var userProducts = await context.userCarts.Where(c => c.User.Id == userId).Include(c => c.Product).ToListAsync();
            double Payment = 0;
            var productsDb = new List<Product>();
            foreach (var item in userProducts)
            {
                Payment = Payment + item.Product.price * item.Qunatity;
                productsDb.Add(item.Product);
            }
            return $"Total Payment is {Payment} ";
        }

        public async Task<IEnumerable<CartDTO>> ViewCard(string userID)
        {
            HttpContext httpContext = httpContextAccessor.HttpContext;

            var userProducts = await context.userCarts
                .Where(c => c.User.Id == userID)
                .Include(c => c.Product)
                    .ThenInclude(p => p.Images)
                .ToListAsync();

            List<CartDTO> userCart = new List<CartDTO>();

            foreach (var product in userProducts)
            {
                var productDTO = new CartDTO
                {
                    Id = product.Product.Id,
                    ProductDiscription = product.Product.Description,
                    ProductName = product.Product.Item_Name,
                    ProductPrice = product.Product.price,
                    Quantity = product.Qunatity,
                    AverageRate = (int)product.Product.averageRate,
                    imageUrls = product.Product.Images.Select(img => $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/images/{img.ImagePath}").ToList()
                };

                userCart.Add(productDTO);
            }

            return userCart;
        }
    }
}
