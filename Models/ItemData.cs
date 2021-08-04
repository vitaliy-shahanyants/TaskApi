using System.ComponentModel.DataAnnotations;

namespace TodoApp.Models
{
    public class ItemData
    {
        public int id { get; set; } 
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public bool Done { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public CategoryData Category { get; set; }
        
    }
}