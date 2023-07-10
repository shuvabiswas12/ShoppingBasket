using ShoppingBasket.DataAccessLayer.Infrastructure.IRepository;
using System.Security.Claims;

namespace ShoppingBasket.App.Middlewares
{
    public class CookieMiddleware : IMiddleware
    {
        private readonly IUnitOfWork _unitOfWork;

        public CookieMiddleware(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var claimIdentity = context.User.Identity as ClaimsIdentity;
            var claims = claimIdentity!.FindFirst(ClaimTypes.NameIdentifier);

            context.Session.SetInt32("carts", _unitOfWork.CartRepository.GetAll(predicate: cart => cart.ApplicationUserId == claims!.Value).ToList().Count);

            await next(context);
        }
    }
}
