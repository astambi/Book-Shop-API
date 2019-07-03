namespace BookShop.Services.Models.Authors
{
    using BookShop.Common.Mapping;
    using BookShop.Data.Models;

    public class AuthorServiceModel : IMapFrom<Author>
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
