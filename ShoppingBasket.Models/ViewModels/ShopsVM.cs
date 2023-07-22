using Microsoft.AspNetCore.Mvc.Rendering;
using ShoppingBasket.CommonHelper;
using ShoppingBasket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasket.Models.ViewModels
{
    public class ShopsVM
    {
        public IEnumerable<Product> products { get; set; } = null!;
        
        public int PageCount { get; set; } = 1;
        
        public int ProductsCount { get; set; } = 0;

        public string SortBy = FilterBy.DEFAULT;

        public IEnumerable<SelectListItem> Sorts = new List<SelectListItem>
        {
            new SelectListItem
            {
                Text = "Default",
                Value = FilterBy.DEFAULT
            },
            new SelectListItem
            {
                Text = "Low -> High",
                Value = FilterBy.PRICE_ASC
            },
            new SelectListItem
            {
                Text = "High -> Low",
                Value = FilterBy.PRICE_DESC
            },
            new SelectListItem
            {
                Text = "New",
                Value = FilterBy.NEW
            }
        };
    }
}
