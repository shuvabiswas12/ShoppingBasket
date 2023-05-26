using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ShoppingBasket.Models;

public class ApplicationUser : IdentityUser
{
    [Required] public string Name { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? Gender { get; set; }

    public string? PostCode { get; set; }

    [DisplayName("Created Date")]
    public DateTime CreatedDate { get; set; } = DateTime.Now;
}