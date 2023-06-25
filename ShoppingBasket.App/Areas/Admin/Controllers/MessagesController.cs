using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingBasket.DataAccessLayer.Infrastructure.IRepository;

namespace ShoppingBasket.App.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "Admin")]
    public class MessagesController : Controller
    {
		private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<MessagesController> _logger;

		public MessagesController(ILogger<MessagesController> logger, IUnitOfWork unitOfWork)
		{
            _logger = logger;
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index()
        {
            var contacts = _unitOfWork.ContactRepository.GetAll();
            return View(contacts);
        }

        public IActionResult Preview(int messageId)
        {
            var messageToPreview = _unitOfWork.ContactRepository.GetT(predicate: c => c.Id == messageId);
            return Ok(new { data = messageToPreview });
        }
    }
}
