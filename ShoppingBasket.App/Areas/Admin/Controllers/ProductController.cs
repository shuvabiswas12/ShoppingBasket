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
        return View(productVm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateAndUpdate(ProductVm productVm, IFormFile ? file)
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.ProductRepository.Add(productVm.Product);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        return View();
    }
}