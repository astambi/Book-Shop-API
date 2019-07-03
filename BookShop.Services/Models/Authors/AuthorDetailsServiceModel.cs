namespace BookShop.Services.Models.Authors
{
    using System.Collections.Generic;

    public class AuthorDetailsServiceModel
    {
        public AuthorServiceModel Author { get; set; }

        public IEnumerable<string> Books { get; set; }
    }
}
