using System.ComponentModel.DataAnnotations;

namespace E_come.DTO.ShopDTO
{
    public class ShopProdDTO
    {

        public string Name { get; set; }
        public string TheDoorNumber { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        public string? imagePath { get; set; }

    }
}
