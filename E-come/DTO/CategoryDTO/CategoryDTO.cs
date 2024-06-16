using E_come.DTO.ProductDTO;

namespace E_come.DTO.CategoryDTO
{
    public class CategoryDTO
    {
        public int categoryId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string Name { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public int TotalProducts { get; set; } = 0;
        public List<productDTO> productDTOs { get; set; }

    }
}
