using Lesson2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Lesson2.Controllers
{
    public class HomeController : Controller
    {
        public string StudentName = "Dôn Đô";
        public string StudentEmail = "don.do@st.cmcu.com";
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult About()
        {
            ViewBag.StudentName = StudentName;
            return View();
        }
        public IActionResult Contact()
        {
            ViewBag.StudentEmail = StudentEmail;
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
