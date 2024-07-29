using Khosomat.Entities.Interfaces;
using Khosomat.Entities.Models;
using Khosomat.Entities.ViewModels;
using Khosomat.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Khosomat.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public ShoppingCartVM ShoppingCartVM{ get; set; }


        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM = new ShoppingCartVM { 
                CartsList = _unitOfWork.ShoppingCart.GetAll(u=>u.ApplicationUserId == claim.Value, IncludeTable: "Product")
            };
            foreach (var item in ShoppingCartVM.CartsList)
            {
                ShoppingCartVM.TotalCarts += (item.Count *  item.Product.Price);
            }
			if (ShoppingCartVM.CartsList.Count() <= 0)
				return RedirectToAction("Index", "Home" , new {area="Customer"});

			return View(ShoppingCartVM);
        }
		public IActionResult Plus(int cartid)
        {
            var shoppingCart = _unitOfWork.ShoppingCart.GetFirstOrDefault(x=>x.Id == cartid);
            _unitOfWork.ShoppingCart.IncreaseCount(shoppingCart, 1);
            _unitOfWork.Complete();
            return RedirectToAction("Index");
        }
		public IActionResult Minus(int cartid)
		{
			var shoppingCart = _unitOfWork.ShoppingCart.GetFirstOrDefault(x => x.Id == cartid);
			_unitOfWork.ShoppingCart.DecreaseCount(shoppingCart, 1);
            if(shoppingCart.Count == 0)
            {
                _unitOfWork.ShoppingCart.Delete(shoppingCart);
            }
			_unitOfWork.Complete();
			return RedirectToAction("Index");
		}
		public IActionResult Remove(int cartid)
        {
			var shoppingCart = _unitOfWork.ShoppingCart.GetFirstOrDefault(x => x.Id == cartid);
            _unitOfWork.ShoppingCart.Delete(shoppingCart);
			_unitOfWork.Complete();
			return RedirectToAction("Index");
		}
        [HttpGet]
		public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new ShoppingCartVM
            {
                CartsList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, IncludeTable: "Product"),
                OrderHeader = new()
            };

            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(x => x.Id == userId);

            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
            ShoppingCartVM.OrderHeader.Address = ShoppingCartVM.OrderHeader.ApplicationUser.Address;
            ShoppingCartVM.OrderHeader.Phone = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;

            foreach (var item in ShoppingCartVM.CartsList)
            {
                ShoppingCartVM.OrderHeader.TotalPrice += (item.Count * item.Product.Price);
                ShoppingCartVM.TotalCarts += (item.Count * item.Product.Price);

            }
            return View(ShoppingCartVM);
		}
        [HttpPost]
        [ActionName("Summary")]
        [ValidateAntiForgeryToken]
		public IActionResult PostSummary()
        {
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM.CartsList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, IncludeTable: "Product");

            ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
            ShoppingCartVM.OrderHeader.ApplicationUserId = userId;
            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(u=>u.Id == userId);


            //ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
            //ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
            //ShoppingCartVM.OrderHeader.Address = ShoppingCartVM.OrderHeader.ApplicationUser.Address;
            //ShoppingCartVM.OrderHeader.Phone = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;



            foreach (var item in ShoppingCartVM.CartsList)
            {
                ShoppingCartVM.OrderHeader.TotalPrice += (item.Count * item.Product.Price);
            }

            ShoppingCartVM.OrderHeader.OrderStatus = SD.Pending;
			ShoppingCartVM.OrderHeader.PaymentStatus = SD.Pending;

			

            _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
            _unitOfWork.Complete();

            foreach (var item in ShoppingCartVM.CartsList)
            {
                OrderDetail orderDetail = new OrderDetail()
                {
                    ProductId = item.ProductId,
                    Count = item.Count,
                    Price = item.Product.Price,
                    OrderHeaderId = ShoppingCartVM.OrderHeader.Id
                };
                _unitOfWork.OrderDetail.Add(orderDetail);
                _unitOfWork.Complete();
            }

            return RedirectToAction("Index","Home");
		}
	}
}
