using E_come.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace E_come.DTO.ProductDTO
{
    public class ProdWithCategory
    {

        public string categoryName { get; set; }
        public string Item_Name { get; set; }
        public int priceProd { get; set; }
        public string Descrip { get; set; }
       // public int stockProd { get; set; }
        public int solditemsProd { get; set; }
        public int quantityProd { get; set; }
        public string nameShop { get; set; }

        //public ICollection<IFormFile> ImageFiles { get; set; }
    }
}
