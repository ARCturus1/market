using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MobileStore.Domain.Entities
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Please enter a product name")]
        public string Name { get; set; }

        [MaxLength(2000)]
        [Required(ErrorMessage = "Please enter a description")]
        public string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Please enter a positive price")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Please specify a category")]
        public string Category { get; set; }

        public File TitleImage { get; set; }

        public ICollection<File> Images { get; set; }
    }
}
