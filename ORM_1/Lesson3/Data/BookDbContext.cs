using Microsoft.EntityFrameworkCore;
using Lesson3.Models;

namespace BookManagement.Data
{
    public class BookDbContext : DbContext
    {

        public BookDbContext(DbContextOptions<BookDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
    }
}
