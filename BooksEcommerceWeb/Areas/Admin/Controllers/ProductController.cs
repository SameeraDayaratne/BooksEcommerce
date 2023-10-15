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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IProductRepository dbProd, ICategoryRepository dbCat, IWebHostEnvironment webHostEnvironment)
        {
            _productRepo = dbProd;
            _categoryRepo = dbCat;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> productList = _productRepo.GetAll("Category").ToList();

            return View(productList);
        }

        public IActionResult Upsert(int? id)
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


            if (id == null || id == 0)
            {
                //Create
                return View(productVM);
            }
            else
            {
                //Update
                productVM.Product = _productRepo.Get(u => u.Id == id);
                return View(productVM);
            }


        }
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            /*if(obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "Name and Product cannot be the same");
            }*/

            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    if (!string.IsNullOrEmpty(productVM.Product.Imageurl))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.Imageurl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, filename), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    productVM.Product.Imageurl = @"\images\product\" + filename;
                }

                if (productVM.Product.Id == 0)
                {
                    _productRepo.Add(productVM.Product);
                }
                else
                {
                    _productRepo.Update(productVM.Product);
                }


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



        /*public IActionResult Delete(int? id)
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

        }*/

        #region
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> productList = _productRepo.GetAll("Category").ToList();

            return Json(new { data = productList });

        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            Product productFromDb = _productRepo.Get(p => p.Id == id);

            if (productFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });

            }

            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, productFromDb.Imageurl.TrimStart('\\'));

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _productRepo.Remove(productFromDb);
            _productRepo.SaveChanges();

            return Json(new { success = true, message = "Successfully deleted" });

        }
        #endregion
    }
}