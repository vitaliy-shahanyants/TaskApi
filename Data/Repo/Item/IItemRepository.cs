using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApp.Models;

namespace TodoApp.Data.Repo
{
    public interface IItemRepository
    {
        Task<IEnumerable<ItemData>> GetItemAsync();
        void CreateItem(ItemData data);
        void CreateManyItems(ItemData [] data);
        Task<ItemData> GetItemByIdAsync(int id);
        void UpdateItem(int id, ItemData data);
        void DeleteItem(int id);
 
    }
}