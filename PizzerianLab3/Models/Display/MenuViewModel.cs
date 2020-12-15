using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzerianLab3.Models
{
    public class MenuViewModel
    {
        public List<PizzaMenuViewModel> Pizzas { get; set; } = new List<PizzaMenuViewModel>();
        public List<SodaViewModel> Sodas { get; set; } = new List<SodaViewModel>();
        public List<ExtraIngredientViewModel> ExtraIngredients { get; set; } = new List<ExtraIngredientViewModel>();
    }
}
