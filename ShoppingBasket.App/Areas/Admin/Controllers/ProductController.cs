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
        return View();
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
        }
        catch (Exception ex)
        {
            return View("_404");
        }

        productVm.Stock = _unitOfWork.StockRepository.GetT(p => p.ProductId == id);
        return View(productVm);  // for product updating
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateAndUpdate(ProductVm productVm, IFormFile? file)
    {
        if (ModelState.IsValid)
        {
            productVm.Product.Price = double.Parse(productVm.Product.Price.ToString("F"));
            productVm.Stock.Product = productVm.Product;
            _unitOfWork.StockRepository.Add(productVm.Stock);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        return RedirectToAction("CreateAndUpdate");
    }
}