
using BooksEcommerceWeb.Data;
using BooksEcommerceWeb.Models;
using Microsoft.AspNetCore.Mvc;
using BooksEcommece.DataAccess.Repository.IRepository;


namespace BooksEcommerceWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepo;

        public CategoryController(ICategoryRepository db)
        {
            _categoryRepo = db;
        }
        public IActionResult Index()
        {
            List<Category> categoryList = _categoryRepo.GetAll().ToList();
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
                _categoryRepo.Add(obj);
                _categoryRepo.SaveChanges();
                TempData["success"] = "Category Created Successfully";
                return RedirectToAction("Index");
            }
            return View();

        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0 ) 
            {
                return NotFound();
            }

            Category categoryFromDb = _categoryRepo.Get(p => p.Id==id);

            if (categoryFromDb == null) 
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
           
            if (ModelState.IsValid)
            {
                _categoryRepo.Update(obj);
                _categoryRepo.SaveChanges();
                TempData["success"] = "Category Updated Successfully";
                return RedirectToAction("Index");
            }
            return View();

        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category categoryFromDb = _categoryRepo.Get(p => p.Id == id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category categoryObj = _categoryRepo.Get(p => p.Id == id);
            if (categoryObj == null) 
            {
                return NotFound();
            }

            _categoryRepo.Remove(categoryObj);
            _categoryRepo.SaveChanges();
            TempData["success"] = "Category Deleted Successfully";
            return RedirectToAction("Index");
                     
        }

    }
}
