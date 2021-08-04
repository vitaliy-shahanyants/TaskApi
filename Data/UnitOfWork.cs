using System.Threading.Tasks;
using TodoApp.Data.Repo;
using TodoApp.Data.Repo.Category;
using TodoApp.Interfaces;

namespace TodoApp.Data
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly ApiDbContext context;
        public UnitOfWork(ApiDbContext context)
        {
            this.context = context;
        }

        public IItemRepository ItemRepository => new ItemRepository(context);
        public ICategoryRepository CategoryRepository => new CategoryRepository(context);
        
        public async Task<bool> SaveAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }
    }
}