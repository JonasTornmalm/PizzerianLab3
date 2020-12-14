using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzerianLab3.Data.Entities
{
    public class Order : Entity
    {
        public Order()
        {
            PizzaOrders = new List<Pizza>();
            SodaOrders = new List<Soda>();
            IngredientOrder = new List<Ingredient>();
        }
        public virtual ICollection<Pizza> PizzaOrders { get; set; }
        public virtual ICollection<Soda> SodaOrders { get; set; }
        public virtual ICollection<Ingredient> IngredientOrder { get; set; }
        public double TotalPrice { get; set; }
    }
}
