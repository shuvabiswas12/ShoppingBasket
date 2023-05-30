using Microsoft.AspNetCore.Mvc;

namespace ShoppingBasket.App.Areas.Customer.Controllers;

[Area("Customer")]
public class CartController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Checkout()
    {
        return View();
    }

    #region update increment or decrement product's count

    public IActionResult IncrementProductCount(int cartId)
    {
        return Ok();
    }

    public IActionResult DecrementProductCount(int cartId)
    {
        return Ok();
    }

    #endregion
}