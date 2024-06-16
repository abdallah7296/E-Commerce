using E_come.DTO.ProductDTO;

namespace E_come.DTO.OrderDTO
{
    public class OrderDTO
    {
        public List<ProductOrderDTO> Products { get; set; }
        public double OerderPrice { get; set; }
        public string ProductStatus { get; set; }
        public DateTime OerderDate { get; set; }
        public string OrderNumber { get; set; }
    }
}
