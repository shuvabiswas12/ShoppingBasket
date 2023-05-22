using Microsoft.AspNetCore.Mvc;
using ShoppingBasket.DataAccessLayer.Infrastructure.IRepository;
using ShoppingBasket.Models;

namespace ShoppingBasket.App.Areas.Admin.Controllers;

[Area("Admin")]
public class CategoryController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    // GET
    public CategoryController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        var categories = _unitOfWork.CategoryRepository.GetAll();
        return View(categories);
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
        if (ModelState.IsValid)
        {
            _unitOfWork.CategoryRepository.Add(category);
            _unitOfWork.Save();
            TempData["success"] = "Category created successfully";
        return RedirectToAction("Index");
        }
        return View();
    }

    #region API DELETE

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var categoryToDelete = _unitOfWork.CategoryRepository.GetT(c => c.Id == id);
        if (categoryToDelete == null) return NotFound("Category not found");
        _unitOfWork.CategoryRepository.Delete(categoryToDelete);
        _unitOfWork.Save();
        TempData["success"] = "Category deleted successfully";
        return Json(new { success = true, message = "Deleted!" });
    }

    #endregion API delete
}