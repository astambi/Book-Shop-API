namespace BookShop.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BookShop.Services.Models.Books;

    public interface IBookService
    {
        Task<IEnumerable<BookBasicServiceModel>> BySearchAsync(string searchTerm);

        Task<int> CreateAsync(
            string title,
            string description,
            decimal price,
            int copies,
            int? edition,
            int? ageRestriction,
            DateTime? releaseDate,
            int authorId,
            string categories);

        Task<BookDetailsWithAuthorAndCategoriesServiceModel> DetailsAsync(int id);

        Task<bool> ExistsAsync(int id);

        Task<bool> RemoveAsync(int id);

        Task<bool> UpdateAsync(
            int id,
            string title,
            string description,
            decimal price,
            int copies,
            int? edition,
            int? ageRestriction,
            DateTime? releaseDate,
            int authorId);
    }
}
