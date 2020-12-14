using PizzerianLab3.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzerianLab3.Models
{
    public class PizzaDisplayModel
    {
        public PizzaDisplayModel()
        {
            PizzaIngredients = new List<IngredientDisplayModel>();
        }
        public int MenuNumber { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public ICollection<IngredientDisplayModel> PizzaIngredients { get; set; }
    }
}
