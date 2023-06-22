using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingBasket.DataAccessLayer.Infrastructure.IRepository;
using ShoppingBasket.Models;
using ShoppingBasket.Models.ViewModels;
using System.Security.Claims;

namespace ShoppingBasket.App.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ContactController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ContactController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var claimIdentity = User.Identity as ClaimsIdentity;
            var user = _unitOfWork.ApplicationUserRepository.GetT(u => u.Email == claimIdentity!.Name);
            var contactVm = new ContactVM();
            if (user != null)
            {
                contactVm.Contact = new Contact()
                {
                    Name = user.Name,
                    Email = user.Email,
                };
            }

            return View(contactVm);
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "User")]
        public IActionResult SendMessage(ContactVM contactVm)
        {
            if (ModelState.IsValid)
            {
                var claimIdentity = User.Identity as ClaimsIdentity;
                var claims = claimIdentity!.FindFirst(ClaimTypes.NameIdentifier);

                contactVm.Contact.ApplicationUserId = claims!.Value;

                _unitOfWork.ContactRepository.Add(contactVm.Contact);
                _unitOfWork.Save();
                TempData["success"] = "Message sent successfully!";
            }
            else
                TempData["error"] = "Message not sent!";

            return RedirectToAction("Index", "Contact");
        }
    }
}