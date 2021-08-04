using System.Collections.Generic;

namespace TodoApp.Models
{
    public class CategoryData
    {
        public int id { get; set; } 
        public string Title { get; set; }
        public ICollection<ItemData> Items { get; set; }
    }
}