using Microsoft.AspNetCore.Mvc;
using ShoppingBasket.DataAccessLayer.Infrastructure.IRepository;
using ShoppingBasket.Models;
using ShoppingBasket.Models.ViewModels;

namespace ShoppingBasket.App.Areas.Customer.Controllers;

[Area("Customer")]
public class ShopsController : Controller
{
    private readonly ILogger<ShopsController> _logger;

    private readonly IUnitOfWork _unitOfWork;

    // GET
    public ShopsController(ILogger<ShopsController> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        var products = _unitOfWork.ProductRepository.GetAll("Category, Stock");
        return View(products);
    }

    public IActionResult Details(int id)
    {
        var productToView = _unitOfWork.ProductRepository.GetT(predicate: p => p.Id == id, includeProperties: "Category, Stock");
        if (productToView == null)
        {
            return View("_404");
        }
        return View(productToView);
    }
}