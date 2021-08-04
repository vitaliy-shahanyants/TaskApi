using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Interfaces;
using TodoApp.Models;
using TodoApp.Models.DTOs.Requests;

namespace TodoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork uow;
        public CategoryController(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var category = await uow.CategoryRepository.GetCategoriesAsync();
            if (category == null)
            {
                return NotFound();
            }
            
            var categories = category.Select(c => new CategoryDTO {id = c.id, Title = c.Title, 
                Items = c.Items.Select(i => new ItemDTO{id = i.id, Title=i.Title,Description=i.Description,Done = i.Done,CategoryId = i.CategoryId}).ToList()}).ToList();
            
            return Ok(categories);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryDTO data)
        {
            if(ModelState.IsValid)
            {
                var category = new CategoryData
                {
                    Title = data.Title,
                };
                uow.CategoryRepository.CreateCategoryAsync(category);
                
                var result = await uow.SaveAsync();

                if (result)
                {
                    var tempCat = await uow.CategoryRepository.GetCategoryByIdAsync(category.id);
                    if (tempCat == null)
                    {
                        return CreatedAtAction("GetCategory", new {category.id}, category);
                    }
                    var newCat = new CategoryDTO
                    {
                        id = tempCat.id,
                        Title = tempCat.Title,
                        Items = tempCat != null ? 
                            tempCat.Items.Where(i => i != null)
                                .Select(i => new ItemDTO{id = i.id, Title=i.Title,Description=i.Description,Done = i.Done,CategoryId = i.CategoryId}).ToList() :  null
                    };
                    return CreatedAtAction("GetCategory", new {newCat.id}, newCat);
                }
                    
                
                return new JsonResult("Could not create a new item"){StatusCode = 500};
            }

            return new JsonResult("Something went wrong"){StatusCode = 500};
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await uow.CategoryRepository.GetCategoryByIdAsync(id);
            if(category == null){
                return NotFound();
            }
            
            var newCat = new CategoryDTO
            {
                id = category.id,
                Title = category.Title,
                Items = category.Items != null ?category.Items.Select(i => new ItemDTO{id = i.id, Title=i.Title,Description=i.Description,Done = i.Done,CategoryId = i.CategoryId}).ToList(): null
            };
            return Ok(newCat);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCategory(int id, CategoryDTO data)
        {
            if (id != data.id)
            {
                return BadRequest();
            }

            var existingCategory = await uow.CategoryRepository.GetCategoryByIdAsync(id);
            if (existingCategory == null)
            {
                return NotFound();
            }

            var category = new CategoryData
            {
                id = data.id,
                Title = data.Title
            };
            uow.CategoryRepository.UpdateCategoryAsync(id, category);
            
            var result =  await uow.SaveAsync();
            
            if (result)
                return NoContent();
            
            return new JsonResult($"The item: {data.Title} could not be updated."){StatusCode = 500}; 
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var existingCategory = await uow.CategoryRepository.GetCategoryByIdAsync(id);

            if (existingCategory == null)
            {
                return NotFound();
            }
            uow.CategoryRepository.DeleteCategory(id);
            var result = await uow.SaveAsync();
            
            if (result)
                return Ok(existingCategory);
            
            return new JsonResult($"The item: {existingCategory.Title} could not be updated."){StatusCode = 500}; 
        }
    }
}