using Microsoft.AspNetCore.Mvc;
using ShoppingBasket.DataAccessLayer.Infrastructure.IRepository;
using ShoppingBasket.Models.ViewModels;

namespace ShoppingBasket.App.Areas.Customer.Controllers;

[Area("Customer")]
public class ShopsController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly IUnitOfWork _unitOfWork;

    // GET
    public ShopsController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        var homeVm = new HomeVM()
        {
            Categories = _unitOfWork.CategoryRepository.GetAll(),
        };
        return View(homeVm);
    }

    public IActionResult Details(int? id)
    {
        return View();
    }
}