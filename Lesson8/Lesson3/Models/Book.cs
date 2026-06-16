using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace Lesson3.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Không được để trống")]
        public string Name { get; set; } = "";

        [Range(0.01, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}