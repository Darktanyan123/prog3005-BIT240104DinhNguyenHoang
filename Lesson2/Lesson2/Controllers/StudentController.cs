using Lesson2.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lesson2.Controllers
{
    public class StudentController : Controller
    {


        public IActionResult Index()
        {
            ViewBag.Name = "Đinh Nguyên Hoàng";
            ViewData["Age"] = 20;
            var model = new Student { Major = "CNTT" };
            return View("Info", model);
        }
    }
}
