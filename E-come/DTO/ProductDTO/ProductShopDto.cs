using System.ComponentModel.DataAnnotations;

namespace E_come.DTO.ProductDTO
{
    public class ProductShopDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TheDoorNumber { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
    }
}
