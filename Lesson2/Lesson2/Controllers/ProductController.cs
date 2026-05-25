using Microsoft.AspNetCore.Mvc;


namespace StudentManagement.Controllers
{
    public class ProductController : Controller
    {

        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                ViewBag.Message = "Error: Product ID is required";
            }
            else
            {
                ViewBag.Message = "Product ID = " + id;
            }

            return View();
        }

        public ActionResult Category(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                ViewBag.Message = "Error: Category name is required";
            }
            else
            {
                ViewBag.Message = "Category = " + name;
            }

            return View();
        }
    }
}