
using MIDTest.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MidTest.Models
{
    [Table("Events_BIT240104")]
    public class Event_BIT240104
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên sự kiện không được để trống.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Giá không được để trống.")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá không được nhỏ hơn 0.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Ngày bắt đầu không được để trống.")]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Ngày kết thúc không được để trống.")]
        [DataType(DataType.DateTime)]
        [DateGreaterThan("StartDate", ErrorMessage = "Ngày kết thúc phải lớn hơn ngày bắt đầu.")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Địa điểm không được để trống.")]
        public string Location { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn loại sự kiện.")]
        public int EventCategoryId { get; set; }

        [ForeignKey("EventCategoryId")]
        public virtual EventCategory_BIT240104 EventCategory { get; set; }

        // Quan hệ: 1 Sự kiện có nhiều Ảnh
        public virtual ICollection<EventImage_BIT240104> EventImages { get; set; } = new List<EventImage_BIT240104>();

        // Thuộc tính không lưu DB để tính trạng thái (Chức năng 3)
        [NotMapped]
        public string Status
        {
            get
            {
                var now = DateTime.Now; // Hệ thống tự động đồng bộ theo thời gian thực (2026)
                if (now < StartDate) return "Sắp diễn ra";
                if (now >= StartDate && now <= EndDate) return "Đang diễn ra";
                return "Đã kết thúc";
            }
        }
    }
}