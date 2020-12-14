using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzerianLab3.Data;
using PizzerianLab3.Data.Entities;
using PizzerianLab3.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PizzerianLab3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PizzeriaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PizzeriaController(AppDbContext context)
        {
            _context = context;
        }
        // GET: api/<OrderController>
        [HttpGet]
        [SwaggerOperation(Summary = "Get menu")]
        public async Task<IActionResult> Get()
        {
            var menu = await _context.Menus
                .Select(x => new
                {
                    x.PizzaMenu,
                    x.SodaMenu,
                    x.IngredientMenu
                })
                .FirstOrDefaultAsync();

            var menuDisplayModel = new MenuDisplayModel();

            foreach (var pizza in menu.PizzaMenu)
            {
                var pizzaDisplayModel = new PizzaMenuDisplayModel();

                pizzaDisplayModel.MenuNumber = pizza.MenuNumber;
                pizzaDisplayModel.Name = pizza.Name;
                pizzaDisplayModel.Price = pizza.Price;

                foreach (var ingredient in pizza.PizzaIngredients)
                {
                    var ingredientDisplayModel = new IngredientDisplayModel();
                    ingredientDisplayModel.Name = ingredient.Name;
                    pizzaDisplayModel.PizzaIngredients.Add(ingredientDisplayModel);
                }
                menuDisplayModel.Pizzas.Add(pizzaDisplayModel);
            }

            foreach (var soda in menu.SodaMenu)
            {
                var sodaDisplayModel = new SodaDisplayModel();
                sodaDisplayModel.MenuNumber = soda.MenuNumber;
                sodaDisplayModel.Name = soda.Name;
                sodaDisplayModel.Price = soda.Price;
                menuDisplayModel.Sodas.Add(sodaDisplayModel);
            }

            foreach (var extraIngredient in menu.IngredientMenu)
            {
                var extraIngredientDisplayModel = new ExtraIngredientDisplayModel();
                extraIngredientDisplayModel.MenuNumber = extraIngredient.MenuNumber;
                extraIngredientDisplayModel.Name = extraIngredient.Name;
                extraIngredientDisplayModel.Price = extraIngredient.Price;
                menuDisplayModel.ExtraIngredients.Add(extraIngredientDisplayModel);
            }
            
            return Ok(menuDisplayModel);
        }
    }
}
