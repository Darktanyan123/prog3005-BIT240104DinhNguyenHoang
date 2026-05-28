using Lesson3.Models;
using Microsoft.AspNetCore.Mvc;

namespace YourProjectName.Controllers
{
    public class BookController : Controller
    {
 
        static List<Book> books = new List<Book>()
        {
            new Book { Id = 1, Name = "Clean Code", Price = 20 },
            new Book { Id = 2, Name = "ASP.NET MVC", Price = 15 },
            new Book { Id = 3, Name = "Design Pattern", Price = 25 }
        };


        public IActionResult Index()
        {
            return View(books);
        }

        public IActionResult Detail(int id)
        {
            var book = books.FirstOrDefault(b => b.Id == id);

            return View(book);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(string name, double price)
        {
            ViewBag.Message = "Add book successfully";

            return View();
        }
    }
}