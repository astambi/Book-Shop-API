namespace BookShop.Api.Controllers
{
    using System.Threading.Tasks;
    using BookShop.Api.Infrastructure.Extensions;
    using BookShop.Api.Models.Books;
    using BookShop.Services;
    using Microsoft.AspNetCore.Mvc;

    public class BooksController : BaseApiController
    {
        private readonly IAuthorService authorService;
        private readonly IBookService bookService;

        public BooksController(
            IAuthorService authorService,
            IBookService bookService)
        {
            this.authorService = authorService;
            this.bookService = bookService;
        }

        [HttpGet(WebConstants.ById)]
        public async Task<IActionResult> Get(int id)
            => this.OkOrNotFound(await this.bookService.DetailsAsync(id));

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]string search = "")
            => this.Ok(await this.bookService.BySearchAsync(search));

        [HttpDelete(WebConstants.ById)]
        public async Task<IActionResult> Delete(int id)
        {
            var exists = await this.bookService.ExistsAsync(id);
            if (!exists)
            {
                return this.NotFound(WebConstants.BookNotFoundMsg);
            }

            var success = await this.bookService.RemoveAsync(id);
            if (!success)
            {
                return this.BadRequest(WebConstants.BookNotDeletedMsg);
            }

            return this.NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Post(BookWithCategoriesRequestModel model)
        {
            var authorExists = await this.authorService.ExistsAsync(model.AuthorId);
            if (!authorExists)
            {
                this.ModelState.AddModelError(nameof(model.AuthorId), WebConstants.AuthorNotFoundMsg);
                return this.BadRequest(this.ModelState);
            }

            var id = await this.bookService.CreateAsync(
                model.Title,
                model.Description,
                model.Price,
                model.Copies,
                model.Edition,
                model.AgeRestriction,
                model.ReleaseDate,
                model.AuthorId,
                model.Categories);

            if (id < 0)
            {
                return this.BadRequest(WebConstants.BookNotCreatedMsg);
            }

            return this.CreatedAtAction(nameof(Get), new { id }, model);
        }

        [HttpPut(WebConstants.ById)]
        public async Task<IActionResult> Put(int id, BookWithIdRequestModel model)
        {
            if (id != model.Id)
            {
                return this.BadRequest(WebConstants.InvalidIdMsg);
            }

            var exists = await this.bookService.ExistsAsync(id);
            if (!exists)
            {
                return this.NotFound(WebConstants.BookNotFoundMsg);
            }

            var authorExists = await this.authorService.ExistsAsync(model.AuthorId);
            if (!authorExists)
            {
                this.ModelState.AddModelError(nameof(model.AuthorId), WebConstants.AuthorNotFoundMsg);
                return this.BadRequest(this.ModelState);
            }

            var success = await this.bookService.UpdateAsync(
                 id,
                 model.Title,
                 model.Description,
                 model.Price,
                 model.Copies,
                 model.Edition,
                 model.AgeRestriction,
                 model.ReleaseDate,
                 model.AuthorId);

            if (!success)
            {
                return this.BadRequest(WebConstants.BookNotUpdatedMsg);
            }

            return this.NoContent();
        }
    }
}