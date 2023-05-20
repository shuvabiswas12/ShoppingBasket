using Microsoft.AspNetCore.Mvc;

namespace ShoppingBasket.App.Areas.Admin.Controllers;

[Area("Admin")]
public class ProductController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }

    // GET
    public IActionResult Create()
    {
        return View();
    }
}