namespace BookShop.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BookShop.Services.Models.Categories;

    public interface ICategoryService
    {
        Task<IEnumerable<CategoryServiceModel>> AllAsync();

        Task<int> CreateAsync(string name);

        Task<CategoryServiceModel> DetailsAsync(int id);

        Task<bool> ExistsAnotherAsync(int id, string name);

        Task<bool> ExistsAsync(int id);

        Task<bool> ExistsAsync(string name);

        Task<bool> RemoveAsync(int id);

        Task<bool> UpdateAsync(int id, string name);
    }
}
