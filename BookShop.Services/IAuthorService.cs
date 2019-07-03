namespace BookShop.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BookShop.Services.Models.Authors;
    using BookShop.Services.Models.Books;

    public interface IAuthorService
    {
        Task<IEnumerable<BookDetailsWithCategoriesServiceModel>> AllBooksAsync(int authorId);

        Task<int> CreateAsync(string firstName, string lastName);

        Task<AuthorDetailsServiceModel> DetailsAsync(int id);

        Task<bool> ExistsAsync(int id);
    }
}
