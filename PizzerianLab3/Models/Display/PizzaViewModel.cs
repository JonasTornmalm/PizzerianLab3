using PizzerianLab3.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzerianLab3.Models
{
    public class PizzaViewModel
    {
        public PizzaViewModel()
        {
            PizzaIngredients = new List<IngredientViewModel>();
            ExtraIngredients = new List<ExtraIngredientViewModel>();
        }
        public Guid PizzaId { get; set; }
        public int MenuNumber { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public ICollection<IngredientViewModel> PizzaIngredients { get; set; }
        public ICollection<ExtraIngredientViewModel> ExtraIngredients { get; set; }
    }
}
