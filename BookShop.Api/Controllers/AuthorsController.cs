namespace BookShop.Api.Controllers
{
    using System.Threading.Tasks;
    using BookShop.Api.Infrastructure.Extensions;
    using BookShop.Api.Models.Authors;
    using BookShop.Services;
    using Microsoft.AspNetCore.Mvc;

    public class AuthorsController : BaseApiController
    {
        private readonly IAuthorService authorService;
        public AuthorsController(IAuthorService authorService)
        {
            this.authorService = authorService;
        }

        [HttpGet(WebConstants.ById)]
        public async Task<IActionResult> Get(int id)
            => this.OkOrNotFound(await this.authorService.DetailsAsync(id));

        [HttpGet(WebConstants.ById + "/books")]
        public async Task<IActionResult> GetBooks(int id)
            => this.OkOrNotFound(await this.authorService.AllBooksAsync(id));

        [HttpPost]
        public async Task<IActionResult> Post(AuthorRequestModel model)
        {
            var id = await this.authorService.CreateAsync(model.FirstName, model.LastName);

            if (id < 0)
            {
                return this.BadRequest(WebConstants.AuthorNotCreatedMsg);
            }

            return this.CreatedAtAction(nameof(Get), new { id }, model);
        }
    }
}