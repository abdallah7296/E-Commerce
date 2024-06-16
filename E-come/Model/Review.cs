using System.ComponentModel.DataAnnotations.Schema;

namespace E_come.Model
{
    public class Review
    {

        public int Id { get; set; } 
        public string Comment { get; set; }
        public int  Rate { get; set; }
        public DateTime dateTime { get; set; } = DateTime.Now;
        
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int ProductId {  get; set; }

        public Product Product { get; set; }

    }
}
