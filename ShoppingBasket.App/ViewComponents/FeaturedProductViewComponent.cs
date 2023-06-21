using Microsoft.AspNetCore.Mvc;
using ShoppingBasket.DataAccessLayer.Infrastructure.IRepository;

namespace ShoppingBasket.App.ViewComponents
{
    public class FeaturedProductViewComponent : ViewComponent
    {
        private readonly ILogger<FeaturedProductViewComponent> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public FeaturedProductViewComponent(ILogger<FeaturedProductViewComponent> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IViewComponentResult Invoke()
        {
            var featuredProducts = _unitOfWork.ProductRepository.GetAll(includeProperties: "Stock", predicate: p => p.IsFeatured == true);
            return View(featuredProducts);
        }
    }
}
