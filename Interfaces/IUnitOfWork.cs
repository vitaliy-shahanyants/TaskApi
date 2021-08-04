using System.Threading.Tasks;
using TodoApp.Data.Repo;
using TodoApp.Data.Repo.Category;

namespace TodoApp.Interfaces
{
    public interface IUnitOfWork
    {
        IItemRepository ItemRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        Task<bool> SaveAsync();
    }
}