using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzerianLab3.Models.Cart
{
    public class CartLineModel
    {
        public AddToCartModel Pizza { get; set; }
        public int Quantity { get; set; }
    }
}
