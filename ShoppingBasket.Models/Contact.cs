using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasket.Models
{
    public class Contact
    {
        [Key] public int Id { get; set; }

        [Required] public string Name { get; set; } = null!;

        [Required] public string Email { get; set; } = null!;

        [Required] public string Message { get; set; } = null!;

        [Required] public DateTime CreatedAt { get; set; } = DateTime.Now!;

        [ForeignKey("ApplicationUser")] public string ApplicationUserId { get; set; } = null!;

        [ValidateNever] public ApplicationUser ApplicationUser { get; set; } = null!;
    }
}
