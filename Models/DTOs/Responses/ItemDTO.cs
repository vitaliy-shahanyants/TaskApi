using TodoApp.Models.DTOs.Responses;

namespace TodoApp.Models.DTOs.Requests
{
    public class ItemDTO
    {
        public int id { get; set; } 
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Done { get; set; }
        public int CategoryId { get; set; }
        public ItemCategoryDTO ItemCategoryDto {get; set;}
    }
}