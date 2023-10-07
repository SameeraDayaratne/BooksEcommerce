using BooksEcommerceWeb.Data;
using BooksEcommerceWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BooksEcommerceWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<Category> categoryList = _db.Categories.ToList();
            return View(categoryList);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            /*if(obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "Name and Category cannot be the same");
            }*/

            if (ModelState.IsValid)
            {
                _db.Categories.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();  
            
        }
    }
}
