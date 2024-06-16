namespace E_come.DTO.CategoryDTO
{
    public class UpdateCategoryDto
    {
        public bool IsDeleted { get; set; } = false;
        public string Name { get; set; }
        public string Description { get; set; }  
        public int TotalProducts { get; set; } = 0;
    }
}

