namespace BookShop.Services.Models.Books
{
    using System.Collections.Generic;

    public class BookDetailsWithCategoriesServiceModel
    {
        public BookDetailsServiceModel Book { get; set; }

        public IEnumerable<string> Categories { get; set; }
    }
}
