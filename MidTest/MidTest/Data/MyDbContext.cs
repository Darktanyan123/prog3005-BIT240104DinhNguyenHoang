using Microsoft.EntityFrameworkCore;
using MidTest.Models;


namespace MidTest.Data
{
    // Phải kế thừa từ : DbContext
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        // Đảm bảo đã khai báo các DbSet này
        public DbSet<Event_BIT240104> Events { get; set; }
        public DbSet<EventCategory_BIT240104> EventCategories { get; set; }
        public DbSet<EventImage_BIT240104> EventImages { get; set; }
    }
}