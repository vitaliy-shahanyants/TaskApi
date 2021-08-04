using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TodoApp.Models;

namespace TodoApp.Data.Repo
{
    public class ItemRepository:IItemRepository
    {
        private ApiDbContext _context;
        public ItemRepository(ApiDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ItemData>> GetItemAsync()
        {
            var items = await _context.Items.ToListAsync();
            return items;
        }

        public async void CreateItem(ItemData data)
        {
            await _context.Items.AddAsync(data);
            
        }

        public void CreateManyItems(ItemData[] data)
        {
            _context.Items.AddRangeAsync(data);
                
        }

        public async Task<ItemData> GetItemByIdAsync(int id)
        {
            var item = await _context.Items.FirstOrDefaultAsync(x => x.id==id);
            return item;
        }

        public async void UpdateItem(int id, ItemData data)
        {
            var existItem = await _context.Items.FirstOrDefaultAsync(x => x.id == id);
            existItem.Title = data.Title;
            existItem.Description = data.Description;
            existItem.Done = data.Done;
        }

        public async void DeleteItem(int id)
        {
            var existItem = await _context.Items.FirstOrDefaultAsync(x => x.id == id);
            _context.Items.Remove(existItem);
        }
        
    }
}