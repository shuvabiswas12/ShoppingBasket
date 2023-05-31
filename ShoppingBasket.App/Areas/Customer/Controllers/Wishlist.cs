using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingBasket.DataAccessLayer.Infrastructure.IRepository;

namespace ShoppingBasket.App.Areas.Customer.Controllers;

[Area("Customer")]
[Authorize(Roles = "User")]
public class Wishlist : Controller
{
    private ILogger<Wishlist> _logger;
    private IUnitOfWork _unitOfWork;

    public Wishlist(ILogger<Wishlist> logger, IUnitOfWork unitOfWork)
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