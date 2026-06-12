using Lesson3.Data;
using Lesson3.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lesson3.Controllers
{
    public class BookController : Controller
    {
        private readonly BookRepository _repository;

        public BookController(BookRepository repository)
        {
            _repository = repository;
        }

        // READ ALL
        public IActionResult Index()
        {
            var books = _repository.GetAll();
            return View(books);
        }

        // READ ONE
        public IActionResult Detail(int id)
        {
            var book = _repository.GetById(id);

            if (book == null)
                return NotFound("Không tìm thấy sách");

            return View(book);
        }

        // CREATE
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Book book)
        {
            if (!ModelState.IsValid)
                return View(book);

            _repository.Add(book);

            return RedirectToAction("Index");
        }

        // UPDATE
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var book = _repository.GetById(id);

            if (book == null)
                return NotFound();

            return View(book);
        }

        [HttpPost]
        public IActionResult Edit(Book book)
        {
            if (!ModelState.IsValid)
                return View(book);

            _repository.Update(book);

            return RedirectToAction("Index");
        }

        // DELETE
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var book = _repository.GetById(id);

            if (book == null)
                return NotFound();

            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _repository.Delete(id);

            return RedirectToAction("Index");
        }
    }
}