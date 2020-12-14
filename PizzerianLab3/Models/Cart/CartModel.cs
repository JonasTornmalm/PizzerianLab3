using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzerianLab3.Models.Cart
{
    public class CartModel
    {
        public CartModel()
        {
            CartLines = new List<CartLineModel>();
        }
        public List<CartLineModel> CartLines { get; set; }

        public double TotalPrice
        {
            get
            {
                return CartLines.Sum(c => c.Pizza.PizzaToCart.Price * c.Quantity);
            }
        }

        public int TotalQuantity
        {
            get
            {
                return CartLines.Sum(c => c.Quantity);
            }
        }

    }
}
