using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using ShoppingBasket.CommonHelper;
using ShoppingBasket.DataAccessLayer.Infrastructure.IRepository;
using ShoppingBasket.Models;
using ShoppingBasket.Models.ViewModels;
using Stripe;
using System.Security.Claims;

namespace ShoppingBasket.App.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "User, Admin")]
    public class OrdersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<OrdersController> _logger;
        private readonly IEmailSender _emailSender;

        public OrdersController(ILogger<OrdersController> logger, IUnitOfWork unitOfWork, IEmailSender emailSender)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
            return View();
        }

        /* This orderId is a Order Header ID */

        [HttpGet]
        public IActionResult Details(int orderId)
        {
            if (User.IsInRole(WebsiteRoles.AdminRole) || User.IsInRole(WebsiteRoles.EmployeeRole))
            {
                var orderDetailsVm = new OrderDetailsVM()
                {
                    OrderHeader =
                        _unitOfWork.OrderHeaderRepository.GetT(o => o.Id == orderId,
                            includeProperties: "ApplicationUser"),
                    OrderDetail = _unitOfWork.OrderDetailsRepository.GetAll(includeProperties: "Product",
                        predicate: o => o.OrderHeaderId == orderId)
                };

                if (orderDetailsVm.OrderHeader == null) return View("_404");

                return View(orderDetailsVm);
            }
            else
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;
                var claims = claimsIdentity!.FindFirst(ClaimTypes.NameIdentifier);
                var orderDetailsVm = new OrderDetailsVM()
                {
                    OrderHeader =
                        _unitOfWork.OrderHeaderRepository.GetT(o =>
                            o.Id == orderId && o.ApplicationUserId == claims!.Value),
                    OrderDetail = _unitOfWork.OrderDetailsRepository.GetAll(includeProperties: "Product",
                        predicate: o => o.OrderHeaderId == orderId)
                };

                if (orderDetailsVm.OrderHeader == null) return View("_404");

                return View(orderDetailsVm);
            }
        }

        #region API CALL

        [HttpGet]
        public IActionResult GetOrders()
        {
            if (User.IsInRole(WebsiteRoles.AdminRole) || User.IsInRole(WebsiteRoles.EmployeeRole))
            {
                // for admin or employees
                IEnumerable<OrderHeader> orders = _unitOfWork.OrderHeaderRepository.GetAll().OrderByDescending(order => order.Id).ToList();
                return Json(new { data = orders });
            }
            else
            {
                // for customers
                var claimsIdentity = User.Identity as ClaimsIdentity;
                var claims = claimsIdentity!.FindFirst(ClaimTypes.NameIdentifier);
                var orders =
                    _unitOfWork.OrderHeaderRepository.GetAll(predicate: O => O.ApplicationUserId == claims!.Value).OrderBy(order => order.Id).ToList();
                return Json(new { data = orders });
            }
        }

        #endregion API CALL

        public IActionResult Cancel(int orderId)
        {
            var order = _unitOfWork.OrderHeaderRepository.GetT(o => o.Id == orderId, includeProperties: "ApplicationUser");
            if (order == null) return View("_404");

            if (order.OrderStatus != OrderStatus.STATUS_CANCELLED)
            {
                if (order.PaymentType == PaymentTypes.CashOnDelivery)
                {
                    _unitOfWork.OrderHeaderRepository.UpdateStatus(orderId, OrderStatus.STATUS_CANCELLED);
                    _unitOfWork.Save();
                    
                    // sending email to customer
                    string message = $"Your order <b>({order.OrderNumber})</b> has been cancelled. \nIf you did complete the payment, check your account balance after a while because we refunded the payment! \nThanks";
                    _emailSender.SendEmailAsync(order.ApplicationUser.Email, "Order Cancell Notification", message);
                }
                else if (order.PaymentType == PaymentTypes.PaymentOnline &&
                         order.PaymentStatus == PaymentStatus.STATUS_APPROVED)
                {
                    // code for online payment order to refund the money
                    //...
                    try
                    {
                        RefundPayment(order);
                        TempData["success"] = "Order is Canceled and Amount is Refunded!";

                        // sending email to customer
                        string message = $"Your order <b>({order.OrderNumber})</b> has been cancelled. \nIf you did complete the payment, check your account balance after a while because we refunded the payment! \nThanks";
                        _emailSender.SendEmailAsync(order.ApplicationUser.Email, "Order Cancell Notification", message);
                    }
                    catch (Exception)
                    {
                        TempData["error"] = "Something went wrong during Cancelling Order Or Refunding Amount!";
                    }
                }
            }

            return RedirectToAction("Details", new { orderId = orderId });
        }

        private bool RefundPayment(OrderHeader orderHeader)
        {
            var refundOptions = new RefundCreateOptions()
            {
                Reason = RefundReasons.RequestedByCustomer,
                PaymentIntent = orderHeader.PaymentIntentId
            };
            var refund = new RefundService().Create(refundOptions);
            _unitOfWork.OrderHeaderRepository.UpdateStatus(orderHeader.Id, OrderStatus.STATUS_CANCELLED,
                PaymentStatus.STATUS_REFUNDED);
            _unitOfWork.Save();
            return true;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ShipOrder(OrderDetailsVM orderDetailsVm)
        {
            var order = _unitOfWork.OrderHeaderRepository.GetT(o => o.Id == orderDetailsVm.OrderShipping.OrderId);
            if (order == null) return View("_404");

            order.TrackingNumber = orderDetailsVm.OrderShipping.TrackingNumber;
            order.Carrier = orderDetailsVm.OrderShipping.Carrier;

            if (order.OrderStatus == OrderStatus.STATUS_PROCESSING)
            {
                _unitOfWork.OrderHeaderRepository.UpdateStatus(order.Id, OrderStatus.STATUS_SHIPPED);

                if (order.PaymentType == PaymentTypes.CashOnDelivery)
                {
                    _unitOfWork.OrderHeaderRepository.Update(order);
                    _unitOfWork.Save();
                }
                else
                {
                    // online payment
                    if (order is { PaymentStatus: PaymentStatus.STATUS_APPROVED, PaymentIntentId: not null })
                    {
                        _unitOfWork.OrderHeaderRepository.Update(order);
                        _unitOfWork.Save();
                    }
                }

                TempData["success"] = "Order Shipped!";
                // sending email to customer
                string message = $"Your order <b>({order.OrderNumber})</b> has been shiped. Tracking No: {order.TrackingNumber} and Carrier: {order.Carrier}. \nThanks";
                _emailSender.SendEmailAsync(order.ApplicationUser.Email, "Order Shiping Notification", message);
            }

            return RedirectToAction("Details", new { orderId = order.Id });
        }

        /* This id is a OrderHeaderID */

        [HttpGet, ActionName("StartProcessing")]
        public IActionResult ApproveOrderAndStartProcessing(int orderId)
        {
            var order = _unitOfWork.OrderHeaderRepository.GetT(o => o.Id == orderId);
            if (order == null) return View("_404");

            if (order.OrderStatus != OrderStatus.STATUS_PROCESSING)
            {
                _unitOfWork.OrderHeaderRepository.UpdateStatus(orderId, OrderStatus.STATUS_PROCESSING);

                if (order.PaymentType == PaymentTypes.CashOnDelivery) _unitOfWork.Save();
                else
                {
                    // online payment
                    if (order is { PaymentStatus: PaymentStatus.STATUS_APPROVED, PaymentIntentId: not null })
                        _unitOfWork.Save();
                }
            }

            return RedirectToAction("Details", new { orderId = orderId });
        }
    }
}