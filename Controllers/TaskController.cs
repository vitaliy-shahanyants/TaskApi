using Microsoft.AspNetCore.Mvc;
using TodoApp.Data;
using TodoApp.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using TodoApp.Interfaces;
using TodoApp.Models.DTOs.Requests;
using TodoApp.Models.DTOs.Responses;

namespace TodoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TaskController : ControllerBase
    {
 
        private readonly IUnitOfWork uow;
        public TaskController(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        [HttpGet]
        public async Task<IActionResult> GetItems()
        {
            var items = await uow.ItemRepository.GetItemAsync();
            if (items == null)
            {
                return NotFound();
            }
            return Ok(items);
        }
        [HttpPost]
        public async Task< IActionResult> CreateItems(ItemData data)
        {
            if(ModelState.IsValid)
            {
                
                var newItemData = new ItemData
                {
                    Title = data.Title,
                    Description = data.Description,
                    Done = data.Done,
                    CategoryId = data.CategoryId,
                };
                uow.ItemRepository.CreateItem(newItemData);
                
                
                var result = await uow.SaveAsync();
                if (result)
                    return CreatedAtAction("GetItem", new {newItemData.id}, newItemData);
                
                return new JsonResult("Could not create a new item"){StatusCode = 500};
            }

            return new JsonResult("Something went wrong"){StatusCode = 500};
        }
        [HttpPost]
        [Route("many")]
        public async Task<IActionResult> CreateManyItem(ItemData [] data)
        {

            if(ModelState.IsValid)
            {
                uow.ItemRepository.CreateManyItems(data);
                var result = await uow.SaveAsync();
                
                if(result)
                    return CreatedAtAction("GetItem", null, data);
                
                return new JsonResult("Could not create items"){StatusCode = 500}; 
            }

            return new JsonResult("Something went wrong"){StatusCode = 500}; 
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetItem(int id)
        {
            var item = await uow.ItemRepository.GetItemByIdAsync(id);

            if(item == null){
                return NotFound();
            }

            var category = await uow.CategoryRepository.GetCategoryByIdAsync(item.CategoryId);
            if (category == null)
            {
                return Ok(item);
            }
            var itemVideModel = new ItemDTO
            {
                id = item.id,
                Title = item.Title,
                Description = item.Description,
                CategoryId = item.CategoryId,
                ItemCategoryDto =  new ItemCategoryDTO
                {
                    id = category.id,
                    Title = category.Title,
                }
            };
            return Ok(itemVideModel);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateItem(int id, ItemData item){
            if (id != item.id)
            {
                return BadRequest();
            }

            var existItem = await uow.ItemRepository.GetItemByIdAsync(id);

            if (existItem == null)
            {
                return NotFound();
            }

            uow.ItemRepository.UpdateItem(id, item);

            var result =  await uow.SaveAsync();
            
            if (result)
                return NoContent();
            
            return new JsonResult($"The item: {item.Title} could not be updated."){StatusCode = 500}; 

        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteItem(int id){
            
            var existItem = await uow.ItemRepository.GetItemByIdAsync(id);

            if (existItem == null)
            {
                return NotFound();
            }

            uow.ItemRepository.DeleteItem(id);
            
            var result = await uow.SaveAsync();
            
            if (result)
                return Ok(existItem);
            
            return new JsonResult($"The item: {existItem.Title} could not be updated."){StatusCode = 500}; 
        }
    }
}