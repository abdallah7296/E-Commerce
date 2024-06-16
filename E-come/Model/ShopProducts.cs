using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_come.Model
{
    public class ShopProducts
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string TheDoorNumber { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
     
        public string? UserName { get; set; }
     
        [EmailAddress]
        public string? Email { get; set; }

        public string? imagePath { get; set; }
        public ICollection<Product>? Products { get; set;}
        [ForeignKey("Seller")]
        public string? UserId { get; set; }
        public ApplicationUser? Seller { get; set; }
    }  
    }

