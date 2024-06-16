using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace E_come.Model
{
    public class DBContext : IdentityDbContext<ApplicationUser>
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }
        
        public DbSet<address> addresses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<UserCart> userCarts { get; set; }   
        public DbSet<Product> products { get; set; }
        public DbSet<ProductImages> Images  { get; set; }
        public DbSet<ShopProducts> shopsProducts { get; set; }  
        public DbSet<Review> reviews { get; set; }
        public DbSet<ApplyShop>  applyShops { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Fluent Api
            //builder.Entity<UserShop>().HasKey(tbl => new
            //{
            //    tbl.ApplicationUserId,tbl.ShopProductsId
            //});
            base.OnModelCreating(builder);
        }
    }
}
