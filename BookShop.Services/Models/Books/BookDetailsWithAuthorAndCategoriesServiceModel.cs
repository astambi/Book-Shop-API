namespace BookShop.Services.Models.Books
{
    using BookShop.Services.Models.Authors;

    public class BookDetailsWithAuthorAndCategoriesServiceModel : BookDetailsWithCategoriesServiceModel
    {
        public AuthorServiceModel Author { get; set; }
    }
}
