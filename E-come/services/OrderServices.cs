using E_come.DTO.OrderDTO;
using E_come.DTO.ProductDTO;
using E_come.Migrations;
using E_come.Model;
using E_come.services.Business;
using E_come.services.IRepository;
using GSF.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_come.services
{
    public class OrderServices : IOrderRepository
    {
        private readonly DBContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEmailSender emailSender;
        private readonly ILogger<OrderServices> logger;
        private readonly IHttpContextAccessor httpContextAccessor;

        public OrderServices(DBContext context, UserManager<ApplicationUser> userManager,IEmailSender emailSender, ILogger<OrderServices> logger,IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            this.userManager = userManager;
            this.emailSender = emailSender;
            this.logger = logger;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> CancelOrder(string userId, string orderN)
        {
            var user = await userManager.FindByIdAsync(userId);
            var orderDB = await context.Orders.Include(o => o.Products).FirstAsync(o => o.OrderNumber == orderN && o.User.Id == userId);
            if (orderDB == null)
                return "The order number is inavlid";
            if (DateTime.Now >= orderDB.Date.AddDays(3))
                return "Sorry you cannot cancel the order after 3 days";
            /*int indx = 0;*/
            /*foreach (var item in orderDB.Quantities)
            {
                orderDB.Products[indx].quantity += item;
                if (orderDB.Products[indx].UserId != null)
                {
                    var seller = await userManager.FindByIdAsync(orderDB.Products[indx].UserId);
                    await emailSender.SendEmailAsync(seller.Email, "E-Commerce", $"The user ( {user.UserName} ) has Cancelled the order of a {orderDB.Products[indx].quantity} of your product ( {orderDB.Products[indx].Item_Name} ) ");

                }
                indx++;
            }*/
            context.Orders.Remove(orderDB);
            context.SaveChanges();
            await emailSender.SendEmailAsync(user.Email, "Bustan Mall", $"You have cancelled Order with number{orderN}");
            return "Order Cancelled Successfully";

        }

        /* public async Task<string> CheckOut(string userId)
         {
             var user =await userManager.Users.Include(u => u.MyOrders).Include(u => u.MyCart).SingleAsync(u => u.Id == userId);
             var userProducts = await context.userCarts.Where(c => c.User.Id == userId).Include(c => c.Product).ToListAsync();
             if (userProducts.Count == 0)
                 return "You have no item in your cart";
             double Payment = 0;
             var productsDb = new List<Product>();
             var quantities = new List<KeyValuePair<int, int>>();
             var sortedQuantities = new List<int>();
             foreach (var item in userProducts)
             {
                 if (item.Product.quantity < item.Qunatity)
                     return $"Sorry This Qunatity of {item.Product.Item_Name} is Not Available";
                 Payment = Payment + item.Product.price * item.Qunatity;
                 item.Product.quantity -= item.Qunatity;


                 productsDb.Add(item.Product);
                 //if (item.Product.UserId != null)
                 //{
                 //    var seller = await userManager.FindByIdAsync(item.Product.UserId);
                 //    await emailSender.SendEmailAsync(seller.Email, "E-Commerce", $"The user ( {user.UserName} ) has bought a {item.Qunatity} of your product ( {item.Product.Item_Name} ) ");
                 //}
                 quantities.Add(new KeyValuePair<int, int>(item.Product.Id, item.Qunatity));
                 context.SaveChanges();
             }
             quantities = quantities.OrderBy(o => o.Key).ToList();
             foreach (var item in quantities)
             {
                 sortedQuantities.Add(item.Value);
             }
             var orderNumber = Guid.NewGuid().ToString();
             user.MyOrders.Add(new Order
             {
                 Date = DateTime.Now,
                 Products = productsDb,
                 Price = Payment,
                 Quantities = sortedQuantities,
                 OrderNumber = orderNumber,
                 Status = "Pending"

             }); 
            user.MyCart.Clear();
            context.SaveChanges();
             await emailSender.SendEmailAsync(user.Email, "Amazon", $"You Made Order with number ( {orderNumber} ) and total payment={Payment}  تم إتمامه بنجاح   .: https://amzneg.com/d/aRyt2Y8 ");
             return $"Total Payment is {Payment} ";
         } */

        public async Task<string> CheckOut(string userId)
        {
            var user = await userManager.Users
                                        .Include(u => u.MyOrders)
                                        .Include(u => u.MyCart)
                                        .SingleAsync(u => u.Id == userId);

            var userProducts = await context.userCarts
                                            .Where(c => c.User.Id == userId)
                                            .Include(c => c.Product)
                                            .ToListAsync();

            if (userProducts.Count == 0)
                return "You have no item in your cart";

            double Payment = 0;
            var productsDb = new List<Product>();
            var quantities = new List<KeyValuePair<int, int>>();
            var sortedQuantities = new List<int>();

            foreach (var item in userProducts)
            {
                if (item.Product.quantity < item.Qunatity)
                    return $"Sorry, this quantity of {item.Product.Item_Name} is not available";

                Payment += item.Product.price * item.Qunatity;
                item.Product.quantity -= item.Qunatity;

                productsDb.Add(item.Product);
                quantities.Add(new KeyValuePair<int, int>(item.Product.Id, item.Qunatity));
            }

            quantities = quantities.OrderBy(o => o.Key).ToList();

            foreach (var item in quantities)
            {
                sortedQuantities.Add(item.Value);
            }

            var orderNumber = Guid.NewGuid().ToString();
            var orderDate = DateTime.Now;
            var shippingDate = orderDate.AddHours(48);

            user.MyOrders.Add(new Order
            {
                Date = orderDate,
                Products = productsDb,
                Price = Payment,
                Quantities = sortedQuantities,
                OrderNumber = orderNumber,
                Status = "Pending"
            });

            // Remove all items from the user's cart in the database
            context.userCarts.RemoveRange(user.MyCart);

            await context.SaveChangesAsync();

            StringBuilder productsText = new StringBuilder();
            foreach (var item in userProducts)
            {
                productsText.AppendLine($"- {item.Product.Item_Name} ({item.Qunatity})");
            }
            string emailBody = $@"
                <div style=""text-align:center;"">
                <p>You made an order with number {orderNumber} and total payment = {Payment}.</p>
                <p>Your order will be shipped on {shippingDate}.</p>
                <p>تم إتمام الطلب بنجاح!</p>";
            await emailSender.SendEmailAsync(user.Email, "Bustan Mall", emailBody);
           
            return $"Total payment is {Payment}";
        }


        public Task<string> TrackOrder(string userId, string orderN)
        {
            throw new NotImplementedException();
        }

        /*public async Task<List<OrderDTO>> ViewOrders(string userId)
        {
            //   var ordersDB = await context.Orders.Include(o => o.Products).Where(o => o.User.Id == userId).ToListAsync();
             //var userProducts = await context.userCarts.Where(c => c.User.Id == userId).Include(c => c.Product).ToListAsync();
            var user = await userManager.Users.Include(u => u.MyOrders).ThenInclude(o => o.Products).SingleAsync(u => u.Id == userId);
            List<OrderDTO> userOrders = new List<OrderDTO>();//return
            foreach (var item in user.MyOrders)
            {
                int indx = 0;
                List<ProductOrderDTO> productsView = new List<ProductOrderDTO>();
                foreach (var product in item.Products)
                {
                    productsView.Add(new ProductOrderDTO
                    {
                        Description = product.Description,
                        Name = product.Item_Name,
                        Price = product.price,
                        Quantity = product.quantity.ToString()[indx],
                    });
                    indx++;
                   
                }
                userOrders.Add(new OrderDTO
                {
                    OerderDate = item.Date,
                    ProductStatus = item.Status,
                    OerderPrice = item.Price,
                    Products = productsView,
                    OrderNumber = item.OrderNumber

                });
            }
            return userOrders;
        }*/

        public async Task<List<OrderDTO>> ViewOrders(string userId)
        {
            try
            {
                logger.LogInformation("Started fetching orders for user {UserId}", userId);

                var user = await context.Users
                    .Include(u => u.MyOrders)
                    .ThenInclude(o => o.Products)
                    .SingleOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    logger.LogWarning("User with ID {UserId} not found.", userId);
                    return null;
                }

                var userOrders = user.MyOrders.Select(item => new OrderDTO
                {
                    OerderDate = item.Date,
                    ProductStatus = item.Status,
                    OerderPrice = item.Price,
                    Products = item.Products.Select(product => new ProductOrderDTO
                    {
                        Description = product.Description,
                        Name = product.Item_Name,
                        Price = product.price,
                        Quantity = product.quantity 
                    }).ToList(),
                    OrderNumber = item.OrderNumber
                }).ToList();

                logger.LogInformation("Successfully fetched orders for user {UserId}", userId);
                return userOrders;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while fetching orders for user {UserId}", userId);
                throw new ApplicationException($"An error occurred while fetching orders for user {userId}.", ex);
            }
        }
    }

    }

