using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ShoppingBasket.Models.ViewModels;

public class ProductVm
{
    public Product Product { get; set; }

    [ValidateNever] public IEnumerable<SelectListItem> Categories { get; set; }
    [ValidateNever] public Stock Stock { get; set; }
}