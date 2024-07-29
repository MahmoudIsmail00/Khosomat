using Khosomat.Entities.Interfaces;
using Khosomat.Entities.Models;
using Khosomat.Entities.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
            ShoppingCart obj = new ShoppingCart()
            {
                ProductId = id,
                Product = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id, IncludeTable: "Category") , 
                Count = 1
            };

            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCart.ApplicationUserId = claim.Value;

            ShoppingCart CartObj = _unitOfWork.ShoppingCart.GetFirstOrDefault(u=>u.ApplicationUserId == claim.Value && u.ProductId == shoppingCart.ProductId);
            if(CartObj == null)
            {
                _unitOfWork.ShoppingCart.Add(shoppingCart);
            }
            else
            {
                _unitOfWork.ShoppingCart.IncreaseCount(CartObj, shoppingCart.Count);
            }
            _unitOfWork.Complete();
            return RedirectToAction("Index");
        }
    }
}
