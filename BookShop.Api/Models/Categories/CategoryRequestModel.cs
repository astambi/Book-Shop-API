namespace BookShop.Api.Models.Categories
{
    using System.ComponentModel.DataAnnotations;
    using BookShop.Data;

    public class CategoryRequestModel
    {
        [Required]
        [MaxLength(DataConstants.CategoryNameMaxLength)]
        public string Name { get; set; }
    }
}
