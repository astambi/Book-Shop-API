namespace BookShop.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using BookShop.Common.Extensions;
    using BookShop.Data;
    using BookShop.Data.Models;
    using BookShop.Services.Models.Authors;
    using BookShop.Services.Models.Books;
    using Microsoft.EntityFrameworkCore;

    public class BookService : IBookService
    {
        private readonly BookShopDbContext db;
        private readonly IAuthorService authorService;
        private readonly IMapper mapper;

        public BookService(
            BookShopDbContext db,
            IAuthorService authorService,
            IMapper mapper)
        {
            this.db = db;
            this.authorService = authorService;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<BookBasicServiceModel>> BySearchAsync(string searchTerm)
            => await this.db
            .Books
            .Where(b => b.Title.ToLower()
                .Contains((searchTerm ?? string.Empty).ToLower().Trim()))
            .OrderBy(b => b.Title)
            .Take(10)
            .Select(b => this.mapper.Map<BookBasicServiceModel>(b))
            .ToListAsync();

        public async Task<int> CreateAsync(
            string title,
            string description,
            decimal price,
            int copies,
            int? edition,
            int? ageRestriction,
            DateTime? releaseDate,
            int authorId,
            string categoriesStr)
        {
            var authorExists = await this.authorService.ExistsAsync(authorId);
            if (!authorExists)
            {
                return int.MinValue;
            }

            // Create book
            var book = new Book
            {
                AuthorId = authorId,
                Title = title.Trim(),
                Description = description.Trim(),
                Price = price,
                Copies = copies,
                Edition = edition,
                AgeRestriction = ageRestriction,
                ReleaseDate = releaseDate?.ToUniversalTime()
            };

            // Add Book Categories
            if (!string.IsNullOrWhiteSpace(categoriesStr))
            {
                var categoryIds = await this.CreateNewCategoriesAsync(categoriesStr);

                foreach (var categoryId in categoryIds)
                {
                    book.Categories.Add(new BookCategory { CategoryId = categoryId });
                }
            }

            await this.db.AddAsync(book);
            await this.db.SaveChangesAsync();

            return book.Id;
        }

        public async Task<BookDetailsWithAuthorAndCategoriesServiceModel> DetailsAsync(int id)
            => await this.db
            .Books
            .Where(b => b.Id == id)
            .Select(b => new BookDetailsWithAuthorAndCategoriesServiceModel
            {
                Book = this.mapper.Map<BookDetailsServiceModel>(b),
                Author = this.mapper.Map<AuthorServiceModel>(b.Author),
                Categories = b.Categories
                    .Select(bc => bc.Category.Name)
                    .ToList()
            })
            .FirstOrDefaultAsync();

        public async Task<bool> ExistsAsync(int id)
            => await this.db
            .Books
            .AnyAsync(b => b.Id == id);

        public async Task<bool> RemoveAsync(int id)
        {
            var book = await this.db.Books.FindAsync(id);
            if (book == null)
            {
                return false;
            }

            this.db.Books.Remove(book);
            var result = await this.db.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> UpdateAsync(
            int id,
            string title,
            string description,
            decimal price,
            int copies,
            int? edition,
            int? ageRestriction,
            DateTime? releaseDate,
            int authorId)
        {
            var book = await this.db.Books.FindAsync(id);
            if (book == null)
            {
                return false;
            }

            var authorExists = await this.authorService.ExistsAsync(authorId);
            if (!authorExists)
            {
                return false;
            }

            book.Title = title.Trim();
            book.Description = description.Trim();
            book.Price = price;
            book.Copies = copies;
            book.Edition = edition;
            book.AgeRestriction = ageRestriction;
            book.ReleaseDate = releaseDate;
            book.AuthorId = authorId;

            var result = await this.db.SaveChangesAsync();

            return result > 0;
        }

        private async Task<List<int>> CreateNewCategoriesAsync(string categoriesStr)
        {
            // Request category names
            var categoryNames = categoriesStr
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .ToHashSet();

            var existingCategories = await this.db
                .Categories
                .Where(c => categoryNames
                    .Select(n => n.ToLower())
                    .Contains(c.Name.ToLower()))
                .ToListAsync();

            var newCategoryNames = categoryNames
                .Where(c => !existingCategories
                    .Select(x => x.Name.ToLower())
                    .Contains(c.ToLower()))
                .ToList();

            // Create new categories
            foreach (var name in newCategoryNames)
            {
                if (!existingCategories.Select(c => c.Name.ToLower()).Contains(name.ToLower()))
                {
                    var category = new Category { Name = name };
                    await this.db.Categories.AddAsync(category);

                    existingCategories.Add(category);
                }
            }

            await this.db.SaveChangesAsync();

            // Category Ids
            return existingCategories
                .Select(c => c.Id)
                .ToList();
        }
    }
}
