namespace BookShop.Services.Models.Categories
{
    using BookShop.Common.Mapping;
    using BookShop.Data.Models;

    public class CategoryServiceModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
