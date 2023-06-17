using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingBasket.CommonHelper;
using ShoppingBasket.DataAccessLayer.Infrastructure.IRepository;
using ShoppingBasket.Models.ViewModels;
using System.Security.Claims;

namespace ShoppingBasket.App.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "User")]
    public class OrdersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<OrdersController> _logger;
        public OrdersController(ILogger<OrdersController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Details(int orderHeaderId)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var claims = claimsIdentity!.FindFirst(ClaimTypes.NameIdentifier);
            var orderDetailsVm = new OrderDetailsVM() 
            {
                OrderHeader = _unitOfWork.OrderHeaderRepository.GetT(o => o.Id == orderHeaderId && o.ApplicationUserId == claims!.Value),
                OrderDetail = _unitOfWork.OrderDetailsRepository.GetAll(includeProperties: "Product", predicate: o=> o.OrderHeaderId == orderHeaderId)
            };

            if (orderDetailsVm.OrderHeader == null) return View("_404");

            return View(orderDetailsVm);
        }

        [HttpGet]
        public IActionResult GetOrders()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var claims = claimsIdentity!.FindFirst(ClaimTypes.NameIdentifier);
            var orders = _unitOfWork.OrderHeaderRepository.GetAll(predicate: O => O.ApplicationUserId == claims!.Value);
            return Json(new { data = orders });
        }

        public IActionResult Cancel(int orderHeaderId)
        {
            return View();
        }
    }
}
