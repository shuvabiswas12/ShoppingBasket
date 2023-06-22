using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingBasket.DataAccessLayer.Infrastructure.IRepository;
using ShoppingBasket.Models;

namespace ShoppingBasket.App.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
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
    public IActionResult CreateAndUpdate(int? id)
    {
        if (id is not null)
        {
            var categoryToUpdate = _unitOfWork.CategoryRepository.GetT(c => c.Id == id);
            if (categoryToUpdate is null) return View("_404");
            return View(categoryToUpdate);
        }
        return View();
    }

    // POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateAndUpdate(Category category)
    {
        if (ModelState.IsValid)
        {
            if (category.Id == 0)
            {
                _unitOfWork.CategoryRepository.Add(category);
                _unitOfWork.Save();
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                _unitOfWork.CategoryRepository.Update(category);
                _unitOfWork.Save();
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index");
            }
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

    #endregion API DELETE
}