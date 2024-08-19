namespace ExpenseTracker.API.DTOs
{
    public class SubCategoryDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        
        // Foreign key to Category
        public Guid ParentCategoryId { get; set; }
    }
}
