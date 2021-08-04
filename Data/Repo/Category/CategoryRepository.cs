using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TodoApp.Models;
using TodoApp.Models.DTOs.Requests;

namespace TodoApp.Data.Repo.Category
{
    public class CategoryRepository: ICategoryRepository
    {
        private ApiDbContext _context;
        public CategoryRepository(ApiDbContext context)
        {
            this._context = context;
        }
        public async Task<IEnumerable<CategoryData>> GetCategoriesAsync()
        {
            var category = await _context.Category
                .Include(c=>c.Items).ToListAsync();
            return category;
        }

        public async void CreateCategoryAsync(CategoryData data)
        {
            await _context.Category.AddAsync(data);
            
        }

        public async Task<CategoryData> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Category.Include(c=>c.Items).FirstOrDefaultAsync(x => x.id==id);
            return category;
        }

        public async void UpdateCategoryAsync(int id, CategoryData data)
        {
            var existCategory = await _context.Category.FirstOrDefaultAsync(x => x.id == id);
            existCategory.Title = data.Title;
        }

        public async void DeleteCategory(int id)
        {
            var existCategory = await _context.Category.FirstOrDefaultAsync(x => x.id == id);
            _context.Category.Remove(existCategory);
        }
    }
}