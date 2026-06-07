
using Exercise6_Movies_Management.Models;
using Microsoft.AspNetCore.Mvc;

namespace BaiTapMVC.Controllers
{
    public class MovieController : Controller
    {
        private static List<Movie> movies = new List<Movie>()
        {
            new Movie
            {
                Id = 1,
                Title = "Avengers",
                Genre = "Action",
                Duration = 180,
                Director = "Russo Brothers"
            },
            new Movie
            {
                Id = 2,
                Title = "Avatar",
                Genre = "Sci-Fi",
                Duration = 190,
                Director = "James Cameron"
            }
        };

        public IActionResult Index()
        {
            return View(movies);
        }

        public IActionResult Detail(int id)
        {
            var movie = movies.FirstOrDefault(x => x.Id == id);

            if (movie == null)
                return NotFound();

            return View(movie);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Movie movie)
        {
            if (!ModelState.IsValid)
                return View(movie);

            movie.Id = movies.Max(x => x.Id) + 1;
            movies.Add(movie);

            TempData["Message"] = "Thêm phim thành công!";

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var movie = movies.FirstOrDefault(x => x.Id == id);

            if (movie == null)
                return NotFound();

            return View(movie);
        }

        [HttpPost]
        public IActionResult Edit(Movie movie)
        {
            if (!ModelState.IsValid)
                return View(movie);

            var oldMovie = movies.FirstOrDefault(x => x.Id == movie.Id);

            if (oldMovie != null)
            {
                oldMovie.Title = movie.Title;
                oldMovie.Genre = movie.Genre;
                oldMovie.Duration = movie.Duration;
                oldMovie.Director = movie.Director;
            }

            TempData["Message"] = "Cập nhật thành công!";

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var movie = movies.FirstOrDefault(x => x.Id == id);

            if (movie == null)
                return NotFound();

            return View(movie);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var movie = movies.FirstOrDefault(x => x.Id == id);

            if (movie != null)
            {
                movies.Remove(movie);
            }

            TempData["Message"] = "Xóa thành công!";

            return RedirectToAction("Index");
        }


    }
}