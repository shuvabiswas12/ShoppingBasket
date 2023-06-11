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
        var wishlistVm = new WishlistVM()
        {
            Wishlists = _unitOfWork.WishlistRepository.GetAll(includeProperties: "Product"),
        };
        return View(wishlistVm);
    }

    /** If API is false then it is consider as like "The Action method called from directly!". Otherwise if api=true then it 
        is considered as like "The action method called from api call". 
        If api call happens, The method returns a json file instead of a Html Pages.
    */
    [HttpGet]
    public IActionResult AddOrDeleteWishlist(int productId, int? reload = 0)
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

        // for api call
        // if ajax call's headers has "isApiResponse" property set to true, Then reponse will be api response.
        // Otherwise, response will be View() response.
        var isApiResponse = HttpContext.Request.Headers["isApiResponse"].ToString().ToLower();

        if (isApiResponse == "true") return Ok(new { success = "The Product just added to wishlist!" });

        return reload == 1 ? RedirectToAction("Index") : RedirectToAction("Details", "Shops", new { id = productId });
    }

    #endregion
}