using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingBasket.DataAccessLayer.Infrastructure.IRepository;
using ShoppingBasket.Models;
using ShoppingBasket.Models.ViewModels;
using System.Security.Claims;

namespace ShoppingBasket.App.Areas.Customer.Controllers;

[Area("Customer")]
[Authorize(Roles = "User")]
public class CartController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CartController> _logger;

    public CartController(ILogger<CartController> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    // GET
    public IActionResult Index()
    {
        var carts = _unitOfWork.CartRepository.GetAll(includeProperties: "Product");
        return View(carts);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddToCart(ProductDetailsVM productDetailsVm, int productId)
    {
        var claimIdentity = User.Identity as ClaimsIdentity;
        var claims = claimIdentity!.FindFirst(ClaimTypes.NameIdentifier);

        var newCart = new Cart()
        {
            ProductId = productId,
            ApplicationUserId = claims!.Value,
            Count = productDetailsVm.Cart!.Count
        };

        _unitOfWork.CartRepository.Add(newCart);
        _unitOfWork.Save();

        TempData["success"] = "The Product added to carts!";

        return RedirectToAction("Details", "Shops", new { id = productId });
    }

    public IActionResult Checkout()
    {
        return View();
    }

    #region update increment or decrement product's count

    [HttpGet, ActionName("Increment")]
    public IActionResult IncrementProductCount(int cartId, int count)
    {
        var cart = _unitOfWork.CartRepository.GetT(c => c.Id == cartId, includeProperties: "Product");
        if (cart != null)
        {
            cart.Count += count;
            return Ok(new { data = cart, success = "Increment successfull!" });
        }
        return NotFound(new { error = "Cart not exists!" });
    }

    [HttpGet, ActionName("Decrement")]
    public IActionResult DecrementProductCount(int cartId, int count)
    {
        var cart = _unitOfWork.CartRepository.GetT(c => c.Id == cartId, includeProperties: "Product");
        if (cart != null)
        {
            cart.Count -= count;
            return Ok(new { data = cart, success = "Decrement successfull!" });
        }
        return NotFound(new { error = "Cart not exists!" });
    }

    #endregion
}