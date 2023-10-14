using BooksEcommece.DataAccess.Repository.IRepository;
using BooksEcommece.Models.Models;
using BooksEcommece.Models.Models.ViewModels;
using BooksEcommerceWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BooksEcommerceWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepo;
        private readonly ICategoryRepository _categoryRepo;

        public ProductController(IProductRepository dbProd, ICategoryRepository dbCat)
        {
            _productRepo = dbProd;
            _categoryRepo = dbCat;
        }
        public IActionResult Index()
        {
            List<Product> productList = _productRepo.GetAll().ToList();
            
            return View(productList);
        }

        public IActionResult Create()
        {
            
            IEnumerable<SelectListItem> categoryList = _categoryRepo.GetAll().Select(u =>
            new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            }
            );

            ProductVM productVM = new ProductVM()
            {
                CategoryList = categoryList,
                Product = new Product()
            };

            return View(productVM);
        }
        [HttpPost]
        public IActionResult Create(ProductVM productVM)
        {
            /*if(obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "Name and Product cannot be the same");
            }*/

            if (ModelState.IsValid)
            {
                _productRepo.Add(productVM.Product);
                _productRepo.SaveChanges();
                TempData["success"] = "Product Created Successfully";
                return RedirectToAction("Index");
            }
            else
            {
                IEnumerable<SelectListItem> categoryList = _categoryRepo.GetAll().Select(u =>
                new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }
                );

                productVM.CategoryList = categoryList;
            }
            return View(productVM);

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
