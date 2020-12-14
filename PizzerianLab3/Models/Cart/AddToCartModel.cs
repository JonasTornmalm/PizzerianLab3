using PizzerianLab3.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzerianLab3.Models.Cart
{
    public class AddToCartModel
    {
        public Pizza PizzaToCart { get; set; }
        public int Quantity { get; set; }
    }
}
