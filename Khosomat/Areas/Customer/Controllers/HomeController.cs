using Khosomat.Entities.Interfaces;
using Khosomat.Entities.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Khosomat.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var products = _unitOfWork.Product.GetAll();
            return View(products);
        }
        public IActionResult Details(int id)
        {
            ShoppingCart obj = new ShoppingCart() { Product = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id, IncludeTable: "Category") };

            return View(obj);
        }
    }
}
