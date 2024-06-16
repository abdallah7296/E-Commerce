using System.ComponentModel.DataAnnotations.Schema;
using static System.Net.Mime.MediaTypeNames;
namespace E_come.Model
{
    public class ProductImages
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
    public class Product
    {
        public int Id { get; set; }
        public string Item_Name { get; set; }
        public int price { get; set;}
        public string Description { get; set; }
        public int solditems { get; set;}
        public int quantity { get; set;}
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }
      
        public virtual ICollection<UserCart>? usercarts { get; set; }

        public  ICollection<ProductImages>? Images { get; set; }
     
         public ICollection<Review>? Reviews { get; set; }
         public int? averageRate { get; set; }
        [ForeignKey("ShopProducts")]
        public int? ShopId { get; set; }
        public ShopProducts? ShopProducts { get; set; }
        public ICollection<Order>? Orders { get; set; }

        [ForeignKey("Seller")]
        public string? UserId { get; set; }
        public ApplicationUser? Seller { get; set; }
    }
}
