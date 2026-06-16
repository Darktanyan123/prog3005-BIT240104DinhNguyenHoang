using Lesson3.Data;
using Lesson3.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lesson3.Controllers
{
    public class BookController : Controller
    {
        private readonly BookRepository _repository;
        private readonly IWebHostEnvironment _environment;
        public BookController(BookRepository repository, IWebHostEnvironment environment)
        {
            _repository = repository;
            _environment = environment;
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
            if (book.ImageFile != null)
            {
                string extension =
                    Path.GetExtension(book.ImageFile.FileName)
                    .ToLower();

                if (extension != ".jpg" &&
                    extension != ".jpeg" &&
                    extension != ".png")
                {
                    ModelState.AddModelError(
                        "ImageFile",
                        "Chỉ được upload file JPG hoặc PNG");
                }
            }

            if (!ModelState.IsValid)
            {
                return View(book);
            }

            if (book.ImageFile != null)
            {
                string extension =
                    Path.GetExtension(book.ImageFile.FileName)
                    .ToLower();

                string fileName =
                    Guid.NewGuid().ToString() + extension;

                string folder =
                    Path.Combine(
                        _environment.WebRootPath,
                        "images");

                Directory.CreateDirectory(folder);

                string filePath =
                    Path.Combine(folder, fileName);

                using (var stream =
                    new FileStream(filePath, FileMode.Create))
                {
                    book.ImageFile.CopyTo(stream);
                }

                book.ImageUrl = "/images/" + fileName;
            }

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