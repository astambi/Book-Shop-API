namespace BookShop.Services.Implementations
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using BookShop.Data;
    using BookShop.Data.Models;
    using BookShop.Services.Models.Authors;
    using BookShop.Services.Models.Books;
    using Microsoft.EntityFrameworkCore;

    public class AuthorService : IAuthorService
    {
        private readonly BookShopDbContext db;
        private readonly IMapper mapper;

        public AuthorService(
            BookShopDbContext db,
            IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<BookDetailsWithCategoriesServiceModel>> AllBooksAsync(int authorId)
            => await this.db
            .Books
            .Where(b => b.AuthorId == authorId)
            .Select(b => new BookDetailsWithCategoriesServiceModel
            {
                Book = this.mapper.Map<BookDetailsServiceModel>(b),
                Categories = b.Categories
                    .Select(bc => bc.Category.Name)
                    .ToList()
            })
            .ToListAsync();

        public async Task<int> CreateAsync(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName)
                || string.IsNullOrWhiteSpace(lastName))
            {
                return int.MinValue;
            }

            var author = new Author
            {
                FirstName = firstName.Trim(),
                LastName = lastName.Trim()
            };

            await this.db.Authors.AddAsync(author);
            await this.db.SaveChangesAsync();

            return author.Id;
        }

        public async Task<AuthorDetailsServiceModel> DetailsAsync(int id)
            => await this.db
            .Authors
            .Where(a => a.Id == id)
            .Select(a => new AuthorDetailsServiceModel
            {
                Author = this.mapper.Map<AuthorServiceModel>(a),
                Books = a.Books
                    .Select(b => b.Title)
                    .ToList()
            })
            .FirstOrDefaultAsync();

        public async Task<bool> ExistsAsync(int id)
             => await this.db
            .Authors
            .AnyAsync(a => a.Id == id);
    }
}
