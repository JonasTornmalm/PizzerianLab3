using System;
using System.ComponentModel.DataAnnotations;

namespace PizzerianLab3.Data.Entities
{
    public class Ingredient
    {
        public Guid Id { get; set; }
        public int MenuNumber { get; set; }
        private string IngredientName;
        public string Name
        {
            get { return IngredientOption.ToString(); }
            set { IngredientName = value; }
        }
        public IngredientEnum IngredientOption { get; set; }
        public double Price { get; set; }
    }
    public enum IngredientEnum
    {
        Ham = 1,
        Pineapple = 2,
        Mushrooms = 3,
        Onion = 4,
        KebabSauce = 5,
        Shrimps = 6,
        Clams = 7,
        Artichoke = 8,
        Kebab = 9,
        Coriander = 10,

        //Free
        Pepperoni = 11,
        Salad = 12,
        Tomato = 13,
        Cheese = 14,
        Tomatosauce = 15
    }
}