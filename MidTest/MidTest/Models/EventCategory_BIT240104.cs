using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MidTest.Models
{
    [Table("EventCategories_BIT240104")]
    public class EventCategory_BIT240104
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên loại sự kiện không được để trống.")]
        public string Name { get; set; }

        public string Description { get; set; }

        // Quan hệ: 1 Loại có nhiều Sự kiện
        public virtual ICollection<Event_BIT240104> Events { get; set; } = new List<Event_BIT240104>();
    }
}