using BooksEcommece.DataAccess.Repository.IRepository;
using BooksEcommece.Models.Models;
using BooksEcommerceWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BooksEcommerceWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepo;

        public ProductController(IProductRepository db)
        {
            _productRepo = db;
        }
        public IActionResult Index()
        {
            List<Product> productList = _productRepo.GetAll().ToList();
            return View(productList);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product obj)
        {
            /*if(obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "Name and Product cannot be the same");
            }*/

            if (ModelState.IsValid)
            {
                _productRepo.Add(obj);
                _productRepo.SaveChanges();
                TempData["success"] = "Product Created Successfully";
                return RedirectToAction("Index");
            }
            return View();

        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Product productFromDb = _productRepo.Get(p => p.Id == id);

            if (productFromDb == null)
            {
                return NotFound();
            }
            return View(productFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Product obj)
        {

            if (ModelState.IsValid)
            {
                _productRepo.Update(obj);
                _productRepo.SaveChanges();
                TempData["success"] = "Product Updated Successfully";
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

            Product productFromDb = _productRepo.Get(p => p.Id == id);

            if (productFromDb == null)
            {
                return NotFound();
            }
            return View(productFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Product productObj = _productRepo.Get(p => p.Id == id);
            if (productObj == null)
            {
                return NotFound();
            }

            _productRepo.Remove(productObj);
            _productRepo.SaveChanges();
            TempData["success"] = "Product Deleted Successfully";
            return RedirectToAction("Index");

        }
    }
}
