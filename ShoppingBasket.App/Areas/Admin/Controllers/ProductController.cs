using Microsoft.AspNetCore.Mvc;
using ShoppingBasket.Models.ViewModels;

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
        ProductVm productVm = new ProductVm();
        return View(productVm);
    }
}