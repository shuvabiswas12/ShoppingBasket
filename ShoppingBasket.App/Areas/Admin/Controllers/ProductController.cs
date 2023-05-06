using Microsoft.AspNetCore.Mvc;

namespace ShoppingBasket.App.Areas.Admin.Controllers;

public class ProductController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}