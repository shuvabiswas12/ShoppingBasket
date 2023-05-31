using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingBasket.DataAccessLayer.Infrastructure.IRepository;

namespace ShoppingBasket.App.Areas.Customer.Controllers;

[Area("Customer")]
[Authorize(Roles = "User")]
public class WishlistController : Controller
{
    private ILogger<WishlistController> _logger;
    private IUnitOfWork _unitOfWork;

    public WishlistController(ILogger<WishlistController> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        return Ok();
    }

    public IActionResult AddToWishlist(int productId)
    {
        return Ok();
    }

    public IActionResult RemoveFromWishlist(int productId)
    {
        return Ok();
    }
}