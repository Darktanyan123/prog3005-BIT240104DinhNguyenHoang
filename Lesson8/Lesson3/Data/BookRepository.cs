using BookManagement.Data;
using Lesson3.Models;

namespace Lesson3.Data
{
    public class BookRepository
    {
        private readonly BookDbContext _context;

        public BookRepository(BookDbContext context)
        {
            _context = context;
        }

        public List<Book> GetAll()
        {
            return _context.Books
                           .OrderBy(b => b.Id)
                           .ToList();
        }

        public Book? GetById(int id)
        {
            return _context.Books.Find(id);
        }

        public void Add(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
        }

        public bool Update(Book book)
        {
            var existingBook = _context.Books.Find(book.Id);

            if (existingBook == null)
                return false;

            existingBook.Name = book.Name;
            existingBook.Price = book.Price;

            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var book = _context.Books.Find(id);

            if (book == null)
                return false;

            _context.Books.Remove(book);
            _context.SaveChanges();

            return true;
        }
    }
}