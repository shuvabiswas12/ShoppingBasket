using Microsoft.AspNetCore.Mvc;
using ShoppingBasket.Models;

namespace ShoppingBasket.App.Areas.Admin.Controllers;

[Area("Admin")]
public class CategoryController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }

    // GET
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    // POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Category category)
    {
        return RedirectToAction("Index");
    }
}