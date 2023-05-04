using Microsoft.AspNetCore.Mvc;

namespace ShoppingBasket.App.Areas.Customer.Controllers;

[Area("Customer")]
public class ShopsController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}