using Microsoft.AspNetCore.Mvc;
using ShoppingBasket.DataAccessLayer.Infrastructure.IRepository;
using System.Security.Claims;

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
        if (HttpContext.Request.Method == HttpMethods.Get && HttpContext.User.Identity!.IsAuthenticated)
        {
            var claimIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var claims = claimIdentity!.FindFirst(ClaimTypes.NameIdentifier);

            HttpContext.Session.SetInt32("carts", _unitOfWork.CartRepository.GetAll(predicate: cart => cart.ApplicationUserId == claims!.Value).ToList().Count);
        }
        var categories = _unitOfWork.CategoryRepository.GetAll();
        return View(categories);
    }
}