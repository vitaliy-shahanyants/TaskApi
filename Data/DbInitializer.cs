using TodoApp.Models;
using System.Linq;
namespace TodoApp.Data
{
    public class DbInitializer
    {
        public static void Initialize(ApiDbContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Category.Any())
            {
                return;   // DB has been seeded
            }
            var categories = new CategoryData[]
            {
                new CategoryData {Title = "Active"},
                new CategoryData {Title = "In Process"},
                new CategoryData {Title = "Completed"}
            };
            foreach (CategoryData c in categories)
            {
                context.Category.Add(c);
            }
            context.SaveChanges();
            var items = new ItemData[]
            {
                new ItemData {Title = "First Item", Description = "First Item Sample In Actice", CategoryId = 1, Done=false},
                new ItemData {Title = "Second Item", Description = "Second Item Sample In Process", CategoryId = 2, Done=false},
                new ItemData {Title = "Third Item", Description = "First Item Sample In Completed", CategoryId = 3, Done = true},
            };
            foreach (ItemData i in items)
            {
                context.Add(i);
            }
            context.SaveChanges();
        }
    }
    
}