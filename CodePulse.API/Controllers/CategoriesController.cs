using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequestDto request)
        {
            var category = new Category
            {
                Name = request.Name,
                UrlHandle = request.UrlHandle,
            };

            await _categoryRepository.CreateAsync(category);

            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllAsync();

            var response = new List<CategoryDto>();
            foreach (var categoryDto in categories)
            {
                response.Add(new CategoryDto
                {
                    Id = categoryDto.Id,
                    Name = categoryDto.Name,
                    UrlHandle = categoryDto.UrlHandle
                });
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
        {
            var existingCategory = await _categoryRepository.GetByIdAsync(id);
            if (existingCategory is null)
            {
                return NotFound();
            }

            var response = new CategoryDto
            {
                Id = existingCategory.Id,
                Name = existingCategory.Name,
                UrlHandle = existingCategory.UrlHandle
            };

            return Ok(response);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> EditCategory([FromRoute] Guid id, UpdateCategoryRequestDto request)
        {
            var category = new Category
            {
                Id = id,
                Name = request.Name,
                UrlHandle = request.UrlHandle
            };

            category = await _categoryRepository.UpdateAsync(category);

            if (category == null)
            {
                return NotFound();
            }

            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };

            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            var category = await _categoryRepository.DeleteAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };

            return Ok(response);
        }


    }
}
