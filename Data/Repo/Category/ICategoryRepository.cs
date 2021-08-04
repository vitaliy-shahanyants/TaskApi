using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApp.Models;
using TodoApp.Models.DTOs.Requests;

namespace TodoApp.Data.Repo.Category
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<CategoryData>> GetCategoriesAsync();
        void CreateCategoryAsync(CategoryData category);
        Task<CategoryData> GetCategoryByIdAsync(int id);
        void UpdateCategoryAsync(int id, CategoryData data);
        void DeleteCategory(int id);
        
    }
}