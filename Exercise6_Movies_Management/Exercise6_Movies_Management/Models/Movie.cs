using System.ComponentModel.DataAnnotations;

namespace Exercise6_Movies_Management.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên phim không được để trống")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Thể loại không được để trống")]
        public string Genre { get; set; }

        [Range(1, 500, ErrorMessage = "Thời lượng phải lớn hơn 0")]
        public int Duration { get; set; }

        [Required(ErrorMessage = "Đạo diễn không được để trống")]
        public string Director { get; set; }
    }
}