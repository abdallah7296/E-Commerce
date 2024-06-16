using E_come.DTO.ProductDTO;
using E_come.Model;
using System.ComponentModel.DataAnnotations;

namespace E_come.DTO.ShopDTO
{
    public class ShopDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TheDoorNumber { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        public string imagePath { get; set; }
        public List<productDTO> productDTOs { get; set; }

    }
}
