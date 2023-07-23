using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using ShoppingBasket.CommonHelper;
using ShoppingBasket.DataAccessLayer.Infrastructure.IRepository;
using ShoppingBasket.Models;
using ShoppingBasket.Models.ViewModels;
using Stripe.Checkout;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace ShoppingBasket.App.Areas.Customer.Controllers;

[Area("Customer")]
[Authorize(Roles = "User")]
public class CartController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CartController> _logger;
    private readonly IEmailSender _emailSender;

    public CartController(ILogger<CartController> logger, IUnitOfWork unitOfWork, IEmailSender emailSender)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _emailSender = emailSender;
    }

    // GET
    public IActionResult Index()
    {
        var claimIdentity = User.Identity as ClaimsIdentity;
        var claims = claimIdentity!.FindFirst(ClaimTypes.NameIdentifier);

        var carts = _unitOfWork.CartRepository.GetAll(includeProperties: "Product", predicate: c => c.ApplicationUserId == claims!.Value);
        double total = 0;
        foreach (var cart in carts)
        {
            total += (cart.Count * cart.Product!.Price);
        }

        var cartVm = new CartVM() { Carts = carts, Total = total };

        // adding cart count to sessions
        HttpContext.Session.SetInt32("carts", carts.ToList().Count);

        return View(cartVm);
    }

    [HttpPost]
    public IActionResult AddToCart(ProductDetailsVM? productDetailsVm, int productId)
    {
        var claimIdentity = User.Identity as ClaimsIdentity;
        var claims = claimIdentity!.FindFirst(ClaimTypes.NameIdentifier);

        var newCart = new Cart()
        {
            ProductId = productId,
            ApplicationUserId = claims!.Value,
        };

        if (productDetailsVm!.Cart != null)
        {
            newCart.Count = productDetailsVm.Cart!.Count;
        }

        _unitOfWork.CartRepository.Add(newCart);
        _unitOfWork.Save();
        // adding cart count to sessions
        HttpContext.Session.SetInt32("carts", _unitOfWork.CartRepository.GetAll(predicate: c => c.ApplicationUserId == claims!.Value).ToList().Count);

        var isApiResponse = HttpContext.Request.Headers["isApiResponse"].ToString().ToLower();
        if (isApiResponse == "true") return Ok(new { success = "The Product just added to cart!" });

        TempData["success"] = "The Product added to carts!";
        TempData["acknowledge"] = "Added to cart!";
        return RedirectToAction("Details", "Shops", new { id = productId });
    }

    [HttpGet]
    public IActionResult Checkout()
    {
        var claimIdentity = User.Identity as ClaimsIdentity;
        var claims = claimIdentity!.FindFirst(ClaimTypes.NameIdentifier);

        var orderHeader = new OrderHeader()
        {
            ApplicationUserId = claims!.Value
        };

        var checkoutVm = new CheckoutVM()
        {
            Carts = _unitOfWork.CartRepository.GetAll(includeProperties: "Product",
                c => c.ApplicationUserId == claims!.Value),
            OrderHeader = orderHeader
        };

        // counting total price value of all of selected products
        checkoutVm.OrderHeader.OrderTotal = checkoutVm.Carts.Sum(cart => (cart.Product!.Price * cart.Count));

        if (checkoutVm.OrderHeader.OrderTotal == 0) return RedirectToAction("Index");

        return View(checkoutVm);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Checkout(CheckoutVM checkoutVm)
    {
        var claimIdentity = User.Identity as ClaimsIdentity;
        var claims = claimIdentity!.FindFirst(ClaimTypes.NameIdentifier);

        if (ModelState.IsValid && checkoutVm.OrderHeader.ApplicationUserId == claims!.Value)
        {
            _unitOfWork.OrderHeaderRepository.Add(checkoutVm.OrderHeader);
            _unitOfWork.Save();

            checkoutVm.Carts = _unitOfWork.CartRepository.GetAll(includeProperties: "Product",
                c => c.ApplicationUserId == claims!.Value);

            if (SaveOrderDetails(checkoutVm) && RemoveCart(checkoutVm))
            {
                _unitOfWork.Save();
            }

            // Cash on delivery options...
            if (string.Equals(checkoutVm.OrderHeader.PaymentType.ToUpper(), PaymentTypes.CashOnDelivery.ToUpper()))
            {
                _unitOfWork.OrderHeaderRepository.UpdateStatus(checkoutVm.OrderHeader.Id, OrderStatus.STATUS_PENDING,
                    PaymentStatus.STATUS_DUE);
                _unitOfWork.Save();
                SendEmail(claimIdentity.Name, checkoutVm.OrderHeader.OrderNumber);
                TempData["success"] = "Your Order has been successful. There is an email regarding this order sent to you. Please check your mailbox.";
                return RedirectToAction("Details", "Orders", new { orderId = checkoutVm.OrderHeader.Id });
            }

            // Online Payment options...
            return PaymentByStripe(checkoutVm.OrderHeader.Id);
        }
        return View();
    }

    private void SendEmail(string? email, string orderNo, string? paymentIntentId = null)
    {
        if (string.IsNullOrEmpty(email))
        {
            return;
        }

        // for online payment's message
        if (paymentIntentId is not null)
        {
            _emailSender.SendEmailAsync(email, "Order Confimation",
            $"Your order has been placed. \nOrder Number is: <strong>{orderNo}</strong>. " +
            $"\nPayment Intent Id: <b>{paymentIntentId}<b>." +
            $"\n<i>Visit order deatil page for more information about the order.</i>. \n<p>Thanks</p>")
            .GetAwaiter();
            return;
        }

        // For cash on delivery
        _emailSender.SendEmailAsync(email, "Order Confimation", 
            $"Your order has been placed. Order Number is: <strong>{orderNo}</strong>. " +
            $"\n<i>Visit order deatil page for more information about the order.</i>. \n<p>Thanks</p>")
            .GetAwaiter();
    }

    public IActionResult PaymentByStripe(int orderHeaderId)
    {
        var domain = Request.Scheme + "://" + Request.Host.Value;

        var orderDetails = _unitOfWork.OrderDetailsRepository.GetAll(includeProperties: "Product", predicate: o => o.Id == orderHeaderId);

        if (orderDetails.Any())
        {
            var options = new SessionCreateOptions()
            {
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + "/Customer/Cart/PaymentSuccess?id=" + orderHeaderId,
                CancelUrl = domain + "/Customer/Cart/Index",
            };

            foreach (var orderDetail in orderDetails)
            {
                var lineItemOptions = new SessionLineItemOptions()
                {
                    PriceData = new SessionLineItemPriceDataOptions()
                    {
                        ProductData = new SessionLineItemPriceDataProductDataOptions()
                        {
                            Name = orderDetail.Product!.Name,
                        },
                        Currency = "USD",
                        UnitAmount = (long)(orderDetail.Product!.Price * 100),
                    },
                    Quantity = orderDetail.Count,
                };
                options.LineItems.Add(lineItemOptions);
            }

            var service = new SessionService();
            var session = service.Create(options);
            // updating payment status
            _unitOfWork.OrderHeaderRepository.PaymentStatus(orderHeaderId: orderHeaderId, sessionId: session.Id);
            _unitOfWork.OrderHeaderRepository.UpdateStatus(orderHeaderId, OrderStatus.STATUS_PENDING, PaymentStatus.STATUS_PENDING);
            _unitOfWork.Save();

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }
        return View("_404");
    }

    public IActionResult PaymentSuccess(int id)
    {
        // Updting payment status and order status for online payment system
        var orderHeader = _unitOfWork.OrderHeaderRepository.GetT(o => o.Id == id, includeProperties: "ApplicationUser");
        if (orderHeader != null && orderHeader.PaymentIntentId == null)
        {
            var service = new SessionService();
            var session = service.Get(orderHeader.SessionId);
            _unitOfWork.OrderHeaderRepository.PaymentStatus(orderHeaderId: orderHeader.Id, paymentIntentId: session.PaymentIntentId);
            _unitOfWork.OrderHeaderRepository.UpdateStatus(orderHeaderId: orderHeader.Id, OrderStatus.STATUS_APPROVED, PaymentStatus.STATUS_APPROVED);
            _unitOfWork.Save();

            SendEmail(orderHeader.Email, orderHeader.OrderNumber, orderHeader.PaymentIntentId);

            return View(orderHeader);
        }

        return RedirectToAction("Index", "Home");
    }

    private bool RemoveCart(CheckoutVM checkoutVm)
    {
        // remove the carts from db
        foreach (var cart in checkoutVm.Carts)
        {
            _unitOfWork.CartRepository.Delete(cart);
        }

        return true;
    }

    private bool SaveOrderDetails(CheckoutVM checkoutVm)
    {
        foreach (var cart in checkoutVm.Carts)
        {
            var orderDetail = new OrderDetail()
            {
                Count = cart.Count,
                Price = cart.Product!.Price,
                OrderHeaderId = checkoutVm.OrderHeader.Id,
                ProductId = cart.Product!.Id
            };
            _unitOfWork.OrderDetailsRepository.Add(orderDetail);
        }

        return true;
    }

    #region update increment or decrement product's count

    [HttpGet, ActionName("Increment")]
    public IActionResult IncrementProductCount(int cartId, int count)
    {
        var cart = _unitOfWork.CartRepository.GetT(c => c.Id == cartId, includeProperties: "Product");
        if (cart != null)
        {
            cart.Count += count;
            _unitOfWork.Save();
            return Ok(new { data = cart, success = "Increment successful!" });
        }

        return NotFound(new { error = "Cart not exists!" });
    }

    [HttpGet, ActionName("Decrement")]
    public IActionResult DecrementProductCount(int cartId, int count)
    {
        var cart = _unitOfWork.CartRepository.GetT(c => c.Id == cartId, includeProperties: "Product");
        if (cart != null)
        {
            cart.Count -= count;
            _unitOfWork.Save();
            return Ok(new { data = cart, success = "Decrement successful!" });
        }

        return NotFound(new { error = "Cart not exists!" });
    }

    [HttpDelete, ActionName("Delete")]
    public IActionResult DeleteCart(int cartId)
    {
        var cartToDelete = _unitOfWork.CartRepository.GetT(c => c.Id == cartId);
        if (cartToDelete != null)
        {
            _unitOfWork.CartRepository.Delete(cartToDelete);
            _unitOfWork.Save();
            return Ok(new { data = cartToDelete, success = "Cart has been deleted." });
        }

        return NotFound(new { error = "Cart could be deleted." });
    }

    #endregion update increment or decrement product's count
}