namespace BookShop.Services.Models.Books
{
    using System;
    using BookShop.Common.Mapping;
    using BookShop.Data.Models;

    public class BookDetailsServiceModel : IMapFrom<Book>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Copies { get; set; }

        public int? Edition { get; set; } = 1;

        public int? AgeRestriction { get; set; }

        public DateTime? ReleaseDate { get; set; }
    }
}
