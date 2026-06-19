
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MidTest.Models
{
    [Table("EventImages_BIT240104")]
    public class EventImage_BIT240104
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Đường dẫn ảnh không được để trống.")]
        public string ImageUrl { get; set; }

        public bool IsThumbnail { get; set; }

        public int EventId { get; set; }

        [ForeignKey("EventId")]
        public virtual Event_BIT240104 Event { get; set; }
    }
}