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
}