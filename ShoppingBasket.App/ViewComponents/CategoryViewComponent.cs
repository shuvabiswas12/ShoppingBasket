using Microsoft.AspNetCore.Mvc;
using ShoppingBasket.DataAccessLayer.Infrastructure.IRepository;

namespace ShoppingBasket.App.ViewComponents;

public class CategoryViewComponent : ViewComponent
{
    private readonly ILogger<CategoryViewComponent> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public CategoryViewComponent(ILogger<CategoryViewComponent> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public IViewComponentResult Invoke()
    {
        var categories = _unitOfWork.CategoryRepository.GetAll();
        return View(categories);
    }
}