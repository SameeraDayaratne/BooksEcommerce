using BooksEcommece.DataAccess.Repository.IRepository;
using BooksEcommece.Models.Models;
using BooksEcommerceWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BooksEcommerceWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IProductRepository _productRepo;

        public HomeController(IProductRepository db)
        {
            _productRepo = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = _productRepo.GetAll("Category").ToList();
            return View(productList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}