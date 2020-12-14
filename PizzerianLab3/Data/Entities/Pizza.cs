using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzerianLab3.Data.Entities
{
    public class Pizza
    {
        public Pizza()
        {
            PizzaIngredients = new List<Ingredient>();
            ExtraIngredients = new List<Ingredient>();
        }
        public Guid Id { get; set; }
        public int MenuNumber { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public virtual ICollection<Ingredient> PizzaIngredients { get; set; }
        public virtual ICollection<Ingredient> ExtraIngredients { get; set; }
    }
}
