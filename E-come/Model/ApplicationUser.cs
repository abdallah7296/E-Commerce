using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace E_come.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName  { get; set; }
        public string LastName { get; set; }

        public string? ProfileImage { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string Phone { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string City { get; set; }


        public ICollection<UserCart>? MyCart { get; set; }
        public ICollection<Order>? MyOrders { get; set; } = new List<Order>();
        public ICollection<address>? Address { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public ICollection<Product>? products  { get; set; }
        public ICollection<ShopProducts>? shops { get; set; }
    }
}
