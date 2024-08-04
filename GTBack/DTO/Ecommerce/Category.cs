namespace GTBack.Core.DTO.Ecommerce;

public class Category
{
    
        public string CategoryName { get; set; }
        public List<Category> Children { get; set; } = new List<Category>();
    
}

public class MiniCategory
{
    
        public string CategoryName { get; set; }
    
}