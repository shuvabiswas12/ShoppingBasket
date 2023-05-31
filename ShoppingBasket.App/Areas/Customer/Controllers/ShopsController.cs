using Microsoft.AspNetCore.Mvc;
using ShoppingBasket.DataAccessLayer.Infrastructure.IRepository;
using ShoppingBasket.Models;
using ShoppingBasket.Models.ViewModels;
using System.Security.Claims;

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
        var claimsIdentity = User.Identity as ClaimsIdentity;
        var claims = claimsIdentity!.FindFirst(ClaimTypes.NameIdentifier);
        
        string includeProperties = "Category, Stock";

        if (claims != null)
        {
            includeProperties = "Category, Stock, Wishlist";
        }
        var productToView = _unitOfWork.ProductRepository.GetT(predicate: p => p.Id == id, includeProperties: includeProperties);

        if (productToView == null)
        {
            return View("_404");
        }
        return View(productToView);
    }
}