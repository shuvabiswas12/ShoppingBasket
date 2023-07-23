using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using ShoppingBasket.CommonHelper;
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

    [HttpGet]
    public IActionResult Index(int? category, string? sortBy, int page = 1)
    {

        var claimsIdentity = User.Identity as ClaimsIdentity;
        var claims = claimsIdentity!.FindFirst(ClaimTypes.NameIdentifier);

        const int pageSize = 9;
        var shopsVm = new ShopsVM();

        if (category is not null && category != 0)
        {
            shopsVm.products = _unitOfWork.ProductRepository.GetAll("Category, Stock, Wishlist", predicate: p => p.CategoryId == category);
        }
        else if (sortBy is not null && sortBy == FilterBy.PRICE_ASC)
        {
            shopsVm.products = _unitOfWork.ProductRepository.GetAll().OrderBy(product => product.Price).ToList();
            shopsVm.SortBy = FilterBy.PRICE_ASC.ToString();
        }
        else if (sortBy is not null && sortBy == FilterBy.PRICE_DESC)
        {
            shopsVm.products = _unitOfWork.ProductRepository.GetAll().OrderByDescending(product => product.Price).ToList();
            shopsVm.SortBy = FilterBy.PRICE_DESC.ToString();
        }
        else if (sortBy is not null && sortBy == FilterBy.NEW)
        {
            shopsVm.products = _unitOfWork.ProductRepository.GetAll().OrderByDescending(product => product.Id).ToList();
            shopsVm.SortBy = FilterBy.NEW;
        }
        else
        {
            shopsVm.products = _unitOfWork.ProductRepository.GetAll("Category, Stock, Wishlist");
        }

        // for checking wishlist icon fill-heart or empty heart.
        if (claims != null)
        {
            shopsVm.AuthenticatedUser = claims.Value;
        }

        shopsVm.PageCount = (int)Math.Ceiling((decimal)shopsVm.products.Count() / 9);
        shopsVm.ProductsCount = shopsVm.products.Count();
        shopsVm.products = shopsVm.products.Skip((page - 1) * pageSize).Take(pageSize);

        return View(shopsVm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Index(string? item = null)
    {
        if (item!.Trim() is null)
        {
            return View();
        }
        var shopsVm = new ShopsVM();
        shopsVm.products = _unitOfWork.ProductRepository.GetAll(includeProperties: "Category, Stock, Wishlist",
                p => p.Name.Contains(item));
        return View(shopsVm);
    }

    /** This id is a product's Id */
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
        var productDetailsVm = new ProductDetailsVM()
        {
            Product = productToView,
            Cart = new Cart()
        };
        return View(productDetailsVm);
    }
}