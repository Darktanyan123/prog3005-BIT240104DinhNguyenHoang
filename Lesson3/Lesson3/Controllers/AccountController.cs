using Microsoft.AspNetCore.Mvc;

namespace Lesson3.Controllers
{
    public class AccountController : Controller
    {
        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (username == "admin" && password == "123")
            {
                ViewBag.Message = "Login success";
            }
            else
            {
                ViewBag.Message = "Login failed";
            }

            return View();
        }
    }
}