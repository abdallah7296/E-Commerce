namespace E_come.DTO.CartDTO
{
    public class CartDTO
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public string ProductDiscription { get; set; }
        public int Quantity { get; set; }
        public int AverageRate { get; set; }
        public List<string> imageUrls { get; set; }
    }
}
