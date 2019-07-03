namespace BookShop.Services.Implementations
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using BookShop.Data;
    using BookShop.Data.Models;
    using BookShop.Services.Models.Categories;
    using Microsoft.EntityFrameworkCore;

    public class CategoryService : ICategoryService
    {
        private readonly BookShopDbContext db;
        private readonly IMapper mapper;

        public CategoryService(
            BookShopDbContext db,
            IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<CategoryServiceModel>> AllAsync()
            => await this.db
            .Categories
            .Select(c => this.mapper.Map<CategoryServiceModel>(c))
            .ToListAsync();

        public async Task<int> CreateAsync(string name)
        {
            if (await this.ExistsAsync(name))
            {
                return int.MinValue;
            }

            var category = new Category { Name = name };

            await this.db.Categories.AddAsync(category);
            await this.db.SaveChangesAsync();

            return category.Id;
        }

        public async Task<CategoryServiceModel> DetailsAsync(int id)
            => await this.db
            .Categories
            .Where(c => c.Id == id)
            .Select(c => this.mapper.Map<CategoryServiceModel>(c))
            .FirstOrDefaultAsync();

        public async Task<bool> ExistsAnotherAsync(int id, string name)
            => await this.db
            .Categories
            .Where(c => c.Id != id)
            .AnyAsync(c => c.Name.ToLower() == name.ToLower().Trim());

        public async Task<bool> ExistsAsync(int id)
            => await this.db
            .Categories
            .AnyAsync(c => c.Id == id);

        public async Task<bool> ExistsAsync(string name)
            => await this.db
            .Categories
            .AnyAsync(c => c.Name.ToLower() == name.ToLower().Trim());

        public async Task<bool> RemoveAsync(int id)
        {
            var category = await this.db.Categories.FindAsync(id);
            if (category == null)
            {
                return false;
            }

            this.db.Categories.Remove(category);
            var result = await this.db.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> UpdateAsync(int id, string name)
        {
            var category = await this.db.Categories.FindAsync(id);
            if (category == null)
            {
                return false;
            }

            name = name.Trim();
            if (category.Name == name
                || await this.ExistsAnotherAsync(id, name))
            {
                return false;
            }

            category.Name = name;
            var result = await this.db.SaveChangesAsync();

            return result > 0;
        }
    }
}
