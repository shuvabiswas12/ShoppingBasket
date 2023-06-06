using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingBasket.DataAccessLayer.Infrastructure.IRepository;
using ShoppingBasket.Models;
using ShoppingBasket.Models.ViewModels;

namespace ShoppingBasket.App.Areas.Customer.Controllers;

[Area("Customer")]
[Authorize(Roles = "User")]
public class WishlistController : Controller
{
    private ILogger<WishlistController> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public WishlistController(ILogger<WishlistController> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    #region Wishlist api call

    public IActionResult Index()
    {
        var wishlistVm = new WishlistVM() { 
            Wishlists = _unitOfWork.WishlistRepository.GetAll(includeProperties: "Product"),
        };
        return View(wishlistVm);
    }

    [HttpGet]
    public IActionResult AddOrDeleteWishlist(int productId)
    {
        var claimsIdentity = User.Identity as ClaimsIdentity;
        var claims = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);

        try
        {
            var wishlist = _unitOfWork.WishlistRepository.GetT(w =>
                claims != null && (w.ProductId == productId && w.ApplicationUserId == claims.Value));
            _unitOfWork.WishlistRepository.Delete(wishlist);
            _unitOfWork.Save();
        }
        catch (Exception)
        {
            var newWishlist = new Wishlist()
            {
                ProductId = productId,
                ApplicationUserId = claims!.Value,
            };
            _unitOfWork.WishlistRepository.Add(newWishlist);
            _unitOfWork.Save();
        }
        
        return RedirectToAction("Details", "Shops", new { id = productId });
    }

    #endregion
}