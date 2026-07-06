using Cinema_Management.Data;
using Cinema_Management.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Cinema_Management.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        try
        {
            var movies = _context.Movies
                .Select(m => new MovieViewModel
                {
                    MovieId = m.MovieId,
                    Title = m.Title,
                    Duration = m.Duration,
                    PosterURL = m.PosterURL,
                    Genre = string.Join(", ", m.MovieGenres.Select(mg => mg.Genre.Name))
                })
                .ToList();

            return View(movies);
        }
        catch (SqlException)
        {
            ViewBag.DatabaseError = "Khong ket noi duoc SQL Server. Hay bat SQL Server va kiem tra connection string.";
            return View(new List<MovieViewModel>());
        }
    }

    public IActionResult Details(int id)
    {
        try
        {
            var movie = _context.Movies
                .Where(m => m.MovieId == id)
                .Select(m => new MovieViewModel
                {
                    MovieId = m.MovieId,
                    Title = m.Title,
                    Duration = m.Duration,
                    PosterURL = m.PosterURL,
                    ReleaseDate = m.ReleaseDate,
                    AgeRating = m.AgeRating,
                    Synopsis = m.Synopsis,
                    Trailer = m.Trailer,
                    Showtimes = m.Showtimes,
                    Language = m.Language,
                    Country = m.Country,
                    Genre = string.Join(", ", m.MovieGenres.Select(mg => mg.Genre.Name)),
                    MovieDirector = string.Join(", ", m.MovieDirectors.Select(md => md.person.FullName)),
                    MovieCast = string.Join(", ", m.MovieCasts.Select(mc => mc.person.FullName))
                })
                .FirstOrDefault();

            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }
        catch (SqlException)
        {
            return RedirectToAction(nameof(Index));
        }
    }
}
