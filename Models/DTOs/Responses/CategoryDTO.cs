using System.Collections.Generic;

namespace TodoApp.Models.DTOs.Requests
{
    public class CategoryDTO
    {
        public int id { get; set; } 
        public string Title { get; set; }
        
        public List<ItemDTO> Items { get; set; }
    }
}