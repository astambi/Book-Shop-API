namespace BookShop.Api.Models.Authors
{
    using System.ComponentModel.DataAnnotations;
    using BookShop.Data;

    public class AuthorRequestModel
    {
        [Required]
        [MaxLength(DataConstants.AuthorNameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(DataConstants.AuthorNameMaxLength)]
        public string LastName { get; set; }
    }
}
