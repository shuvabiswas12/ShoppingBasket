using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ShoppingBasket.Models;

public class ApplicationUser : IdentityUser
{
    [Required] public string Name { get; set; }

    [Required] public string Address { get; set; }

    [Required] public string City { get; set; }
}