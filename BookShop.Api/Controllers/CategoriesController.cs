namespace BookShop.Api.Controllers
{
    using System.Threading.Tasks;
    using BookShop.Api.Infrastructure.Extensions;
    using BookShop.Api.Models.Categories;
    using BookShop.Services;
    using Microsoft.AspNetCore.Mvc;

    public class CategoriesController : BaseApiController
    {
        private readonly ICategoryService categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
            => this.Ok(await this.categoryService.AllAsync());

        [HttpGet(WebConstants.ById)]
        public async Task<IActionResult> Get(int id)
            => this.OkOrNotFound(await this.categoryService.DetailsAsync(id));

        [HttpDelete(WebConstants.ById)]
        public async Task<IActionResult> Delete(int id)
        {
            var exists = await this.categoryService.ExistsAsync(id);
            if (!exists)
            {
                return this.NotFound(WebConstants.CategoryNotFoundMsg);
            }

            var success = await this.categoryService.RemoveAsync(id);
            if (!success)
            {
                return this.BadRequest(WebConstants.CategoryNotDeletedMsg);
            }

            return this.NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Post(CategoryRequestModel model)
        {
            var nameExists = await this.categoryService.ExistsAsync(model.Name);
            if (nameExists)
            {
                this.ModelState.AddModelError(nameof(CategoryRequestModel.Name), WebConstants.CategoryNameExistsMsg);
                return this.BadRequest(this.ModelState);
            }

            var id = await this.categoryService.CreateAsync(model.Name);
            if (id < 0)
            {
                return this.NotFound(WebConstants.CategoryNotCreatedMsg);
            }

            return this.CreatedAtAction(nameof(Get), new { id }, model);
        }

        [HttpPut(WebConstants.ById)]
        public async Task<IActionResult> Put(int id, CategoryRequestWithIdModel model)
        {
            if (id != model.Id)
            {
                return this.BadRequest(WebConstants.InvalidIdMsg);
            }

            var idExists = await this.categoryService.ExistsAsync(id);
            if (!idExists)
            {
                return this.NotFound(WebConstants.CategoryNotFoundMsg);
            }

            var nameExists = await this.categoryService.ExistsAnotherAsync(id, model.Name);
            if (nameExists)
            {
                this.ModelState.AddModelError(nameof(CategoryRequestModel.Name), WebConstants.CategoryNameExistsMsg);
                return this.BadRequest(this.ModelState);
            }

            var success = await this.categoryService.UpdateAsync(id, model.Name);
            if (!success)
            {
                return this.BadRequest(WebConstants.CategoryNotUpdatedMsg);
            }

            return this.NoContent();
        }
    }
}