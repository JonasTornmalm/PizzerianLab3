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
            Pizzas = new List<Pizza>();
            Sodas = new List<Soda>();
        }
        public virtual ICollection<Pizza> Pizzas { get; set; }
        public virtual ICollection<Soda> Sodas { get; set; }
        
        public double TotalPrice { get; set; }
        public bool IsEmpty => !Pizzas.Any() && !Sodas.Any();
    }
}
