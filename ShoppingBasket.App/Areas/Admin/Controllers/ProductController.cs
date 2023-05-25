using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShoppingBasket.DataAccessLayer.Infrastructure.IRepository;
using ShoppingBasket.Models;
using ShoppingBasket.Models.ViewModels;

namespace ShoppingBasket.App.Areas.Admin.Controllers;

[Area("Admin")]
public class ProductController : Controller
{
    private readonly ILogger<ProductController> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public ProductController(ILogger<ProductController> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    // GET
    public IActionResult Index()
    {
        var productVm = new ProductVm()
        {
            ProductsCount = _unitOfWork.ProductRepository.GetAll().Count(),
        };
        return View(productVm);
    }

    // GET
    [HttpGet]
    public IActionResult CreateAndUpdate(int? id)
    {
        ProductVm productVm = new ProductVm()
        {
            Product = new Product(),
            Categories = _unitOfWork.CategoryRepository.GetAll().Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }),
            Stock = new Stock()
        };

        // for product creation: if id is null or 0 
        if (id is null or 0)
        {
            return View(productVm);
        }

        // if product id is not valid or product is deleted.
        try
        {
            productVm.Product =
                _unitOfWork.ProductRepository.GetT(p => p.Id == id, includeProperties: "Category, Stock");
            if (productVm.Product is null) return View("_404");  // if product is not found based on the id
        }
        catch (Exception ex)
        {
            _logger.Log(logLevel: LogLevel.Error, message: ex.Message);
            return View("_404");
        }

        return View(productVm); // for product updating
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateAndUpdate(ProductVm productVm, IFormFile? file)
    {
        if (ModelState.IsValid)
        {
            // convert price to double like: 180 to 180.0. "F" parameter is responsible for this conversion. 
            productVm.Product.Price = double.Parse(productVm.Product.Price.ToString("F"));
            if (productVm.Product.Id == 0)
            {
                _unitOfWork.ProductRepository.Add(productVm.Product);
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully";
            }
            else
            {
                _unitOfWork.ProductRepository.Update(productVm.Product);
                _unitOfWork.Save();
                TempData["success"] = "Product updated successfully";
            }

            return RedirectToAction("Index");
        }

        return RedirectToAction("CreateAndUpdate");
    }

    #region Datatables API

    [HttpGet]
    public IActionResult GetProducts()
    {
        var products = _unitOfWork.ProductRepository.GetAll(includeProperties: "Category, Stock");

        return Json(new { data = products });
    }

    [HttpDelete]
    public IActionResult DeleteProduct(int id)
    {
        if (id.Equals(null) || id == 0) return NotFound("Product is INVALID!");
        
        var productToDelete = _unitOfWork.ProductRepository.GetT(p => p.Id == id, includeProperties: "Stock");
        if (productToDelete == null) return NotFound("Product not found!");
        _unitOfWork.ProductRepository.Delete(productToDelete);
        _unitOfWork.Save();
        return Json(new { success = true, message = "Product successfully deleted!" });
    }

    #endregion
}