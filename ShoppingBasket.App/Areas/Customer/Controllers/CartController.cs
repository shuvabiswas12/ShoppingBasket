using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingBasket.DataAccessLayer.Infrastructure.IRepository;
using ShoppingBasket.Models;
using ShoppingBasket.Models.ViewModels;
using System.Security.Claims;
using ShoppingBasket.CommonHelper;
using Stripe.Checkout;
using Exception = System.Exception;

namespace ShoppingBasket.App.Areas.Customer.Controllers;

[Area("Customer")]
[Authorize(Roles = "User")]
public class CartController : Controller
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ILogger<CartController> _logger;

	public CartController(ILogger<CartController> logger, IUnitOfWork unitOfWork)
	{
		_logger = logger;
		_unitOfWork = unitOfWork;
	}

	// GET
	public IActionResult Index()
	{
		var carts = _unitOfWork.CartRepository.GetAll(includeProperties: "Product");
		double total = 0;
		foreach (var cart in carts)
		{
			total += (cart.Count * cart.Product!.Price);
		}

		var cartVm = new CartVM() { Carts = carts, Total = total };
		return View(cartVm);
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
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

		TempData["success"] = "The Product added to carts!";

		return RedirectToAction("Index", "Cart");
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
					PaymentStatus.STATUS_PENDING);
				_unitOfWork.Save();
				return RedirectToAction("Index", "Shops");
			}

			// Online Payment options...
			return PaymentByStripe(checkoutVm);
		}

		return View();
	}

	private IActionResult PaymentByStripe(CheckoutVM checkoutVm)
	{
		var domain = "https://localhost:7147/";
		var options = new SessionCreateOptions()
		{
			LineItems = new List<SessionLineItemOptions>(),
			Mode = "payment",
			SuccessUrl = domain + "Customer/Cart/PaymentSuccess?id=" + checkoutVm.OrderHeader.Id,
			CancelUrl = domain + "Customer/Cart/Index",
		};

		foreach (var cart in checkoutVm.Carts)
		{
			var lineItemOptions = new SessionLineItemOptions()
			{
				PriceData = new SessionLineItemPriceDataOptions()
				{
					ProductData = new SessionLineItemPriceDataProductDataOptions()
					{
						Name = cart.Product!.Name,
					},
					Currency = "USD",
					UnitAmount = (long)(cart.Product!.Price * 100),
				},
				Quantity = cart.Count,
			};
			options.LineItems.Add(lineItemOptions);
		}

		var service = new SessionService();
		var session = service.Create(options);
		// updating payment status
		_unitOfWork.OrderHeaderRepository.PaymentStatus(orderHeaderId: checkoutVm.OrderHeader.Id, sessionId: session.Id);
		_unitOfWork.Save();

		Response.Headers.Add("Location", session.Url);
		return new StatusCodeResult(303);
	}

	public IActionResult PaymentSuccess(int id)
	{
		// Updting payment status and order status for online payment system
		var orderHeader = _unitOfWork.OrderHeaderRepository.GetT(o => o.Id == id);
		if (orderHeader != null && orderHeader.PaymentIntentId == null)
		{
			var service = new SessionService();
			var session = service.Get(orderHeader.SessionId);
			_unitOfWork.OrderHeaderRepository.PaymentStatus(orderHeaderId: orderHeader.Id, paymentIntentId: session.PaymentIntentId);
			_unitOfWork.OrderHeaderRepository.UpdateStatus(orderHeaderId: orderHeader.Id, OrderStatus.STATUS_APPROVED, PaymentStatus.STATUS_APPROVED);
			_unitOfWork.Save();

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

	#endregion
}