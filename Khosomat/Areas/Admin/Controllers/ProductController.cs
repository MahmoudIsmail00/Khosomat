using Khosomat.Entities.Interfaces;
using Khosomat.Entities.Models;
using Khosomat.Entities.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Khosomat.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {

            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetData()
        {
            var products = _unitOfWork.Product.GetAll(IncludeTable:"Category");

            return Json(new {data=products});
        }

        [HttpGet]
        public IActionResult Create()
        {
            ProductVM productVM = new ProductVM();
            productVM.Product = new Product();
            productVM.CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value=x.Id.ToString()
            });
            return View(productVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductVM productVM,IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string RootPath = _webHostEnvironment.WebRootPath;
                if(file != null)
                {
                    string filename = Guid.NewGuid().ToString();
                    var upload = Path.Combine(RootPath, @"img\product");
                    var ext = Path.GetExtension(file.FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload,filename+ext),FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVM.Product.Img = @"img\product\" + filename + ext;

                }
                _unitOfWork.Product.Add(productVM.Product);
                _unitOfWork.Complete();

                TempData["Create"] = "Item has been Created Successfully";
                return RedirectToAction("Index");

            }
            return View(productVM);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (id == null || id == 0)
            {
                NotFound();
            }
            var productVM = new ProductVM()
            {
                Product = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id),
                CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            return View(productVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductVM productVM, IFormFile? file)
        {
            productVM.CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            if (ModelState.IsValid)
            {
               
                    string RootPath = _webHostEnvironment.WebRootPath;
                    string filename = Guid.NewGuid().ToString();
                    var upload = Path.Combine(RootPath, @"img\product");
                    var ext = Path.GetExtension(file.FileName);

                    if(productVM.Product.Img != null)
                    {
                        var oldImg = Path.Combine(RootPath,productVM.Product.Img.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImg))
                        {
                            System.IO.File.Delete(oldImg);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(upload, filename + ext), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVM.Product.Img = @"img\product\" + filename + ext;
                _unitOfWork.Product.Update(productVM.Product);
                _unitOfWork.Complete();
                TempData["Update"] = "Item has been updated Successfully";
                return RedirectToAction("Index");

            
                }

            return View(productVM);
        }
        
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var product = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return Json(new { success = false, messanger = "Error while deleting " });
            }
            _unitOfWork.Product.Delete(product);

            var oldImg = Path.Combine(_webHostEnvironment.WebRootPath, product.Img.TrimStart('\\'));

            if (System.IO.File.Exists(oldImg))
            {
                System.IO.File.Delete(oldImg);
            }
            _unitOfWork.Complete();
            return Json(new { success = true, message = "File has been deleted successfuly" });
        }
    }
}
