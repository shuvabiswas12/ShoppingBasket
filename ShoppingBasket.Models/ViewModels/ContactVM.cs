using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ShoppingBasket.Models.ViewModels
{
    public class ContactVM
    {
        [ValidateNever] public Contact Contact { get; set; } = null!;
    }
}