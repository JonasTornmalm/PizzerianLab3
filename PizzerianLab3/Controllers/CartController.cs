using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzerianLab3.Data;
using PizzerianLab3.Data.Entities;
using PizzerianLab3.DTOs;
using PizzerianLab3.Models;
using PizzerianLab3.Models.Cart;
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
    public class CartController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly CartSingleton _cart;

        public CartController(AppDbContext context, CartSingleton cart)
        {
            _context = context;
            _cart = cart;
        }

        // GET: api/<ValuesController>

        // GET api/<ValuesController>/5
        [HttpGet]
        [SwaggerOperation(Summary = "Get current shopping cart")]
        public ActionResult GetCartContent()
        {
            if (_cart.Order.IsEmpty)
                return Ok("Your cart is empty");

            var viewCartContent = new DisplayResponseModel();

            double totalPrice = 0;
            foreach (var pizzaOrder in _cart.Order.PizzaOrders)
            {
                var pizzaDisplayModel = new PizzaDisplayModel();

                foreach (var ingredient in pizzaOrder.PizzaIngredients)
                {
                    var ingredientModel = new IngredientDisplayModel();
                    ingredientModel.Name = ingredient.Name;
                    pizzaDisplayModel.PizzaIngredients.Add(ingredientModel);
                }

                foreach (var extraIngredient in pizzaOrder.ExtraIngredients)
                {
                    var extraIngredientModel = new ExtraIngredientDisplayModel();
                    extraIngredientModel.Name = extraIngredient.Name;
                    extraIngredientModel.MenuNumber = extraIngredient.MenuNumber;
                    extraIngredientModel.Price = extraIngredient.Price;
                    pizzaDisplayModel.ExtraIngredients.Add(extraIngredientModel);
                }

                pizzaDisplayModel.PizzaId = pizzaOrder.Id;
                pizzaDisplayModel.MenuNumber = pizzaOrder.MenuNumber;
                pizzaDisplayModel.Name = pizzaOrder.Name;
                pizzaDisplayModel.Price = pizzaOrder.Price;
                totalPrice += pizzaOrder.Price;

                viewCartContent.Pizzas.Add(pizzaDisplayModel);
            }
            foreach (var sodaOrder in _cart.Order.SodaOrders)
            {
                var sodaDisplayModel = new SodaDisplayModel();

                sodaDisplayModel.Name = sodaOrder.Name;
                sodaDisplayModel.MenuNumber = sodaOrder.MenuNumber;
                sodaDisplayModel.Price = sodaOrder.Price;
                totalPrice += sodaOrder.Price;

                viewCartContent.Sodas.Add(sodaDisplayModel);
            }

            viewCartContent.TotalPrice = totalPrice;

            return Ok(viewCartContent);
        }

        // POST api/<ValuesController>
        [HttpPost]
        [SwaggerOperation(Summary = "Place item to shopping cart")]
        public async Task<IActionResult> Post([FromBody] AddItemToCartDTO request)
        {
            if (!ModelState.IsValid)
                BadRequest();

            var menu = await _context.Menus.FirstOrDefaultAsync();

            var pizzaRecipe = menu.PizzaMenu.Where(x => x.MenuNumber == request.PizzaMenuNumber).FirstOrDefault();
            var sodaOption = menu.SodaMenu.Where(x => x.MenuNumber == request.SodaMenuNumber).FirstOrDefault();

            if (pizzaRecipe == null && sodaOption == null)
                return BadRequest("No items seleceted.");
            else
            {
                if (pizzaRecipe != null)
                {
                    var pizzaToBake = new Pizza()
                    {
                        Id = Guid.NewGuid(),
                        MenuNumber = pizzaRecipe.MenuNumber,
                        Name = pizzaRecipe.Name,
                        Price = pizzaRecipe.Price
                    };

                    foreach (var ingredient in pizzaRecipe.PizzaIngredients)
                    {
                        var freshIngredient = new Ingredient()
                        {
                            Name = ingredient.Name,
                            IngredientOption = ingredient.IngredientOption,
                            MenuNumber = ingredient.MenuNumber,
                            Price = ingredient.Price
                        };
                        pizzaToBake.PizzaIngredients.Add(freshIngredient);
                    }

                    _cart.Order.PizzaOrders.Add(pizzaToBake);
                }

                if (sodaOption != null)
                {
                    var sodaToAdd = new Soda()
                    {
                        Id = Guid.NewGuid(),
                        MenuNumber = sodaOption.MenuNumber,
                        Name = sodaOption.Name,
                        Price = sodaOption.Price
                    };

                    _cart.Order.SodaOrders.Add(sodaToAdd);
                }
            }

            return Ok(new 
            {
                pizza = (pizzaRecipe == null) ? string.Empty : pizzaRecipe.Name, 
                soda = (sodaOption == null) ? string.Empty : sodaOption.Name 
            });
        }

        // PUT api/<ValuesController>/5
        [HttpPut]
        [SwaggerOperation(Summary = "Update shopping cart")]
        public async Task<IActionResult> Put([FromBody] UpdateCartDTO request)
        {
            if (!ModelState.IsValid)
                BadRequest("Bad req");

            var pizzaToModify = _cart.Order.PizzaOrders.Where(x => x.Id == request.PizzaId).FirstOrDefault();

            if (pizzaToModify == null)
                return BadRequest("Could not find pizza id");

            if (request.ExtraIngredients.Where(x => x.MenuNumber > 0 && x.MenuNumber < 11).Any())
            {
                double priceForExtraIngredients = 0;
                var menu = await _context.Menus.FirstOrDefaultAsync();

                foreach (var ingredient in request.ExtraIngredients)
                {
                    var ingredientOption = menu.IngredientMenu.Where(x => x.MenuNumber == ingredient.MenuNumber).FirstOrDefault();
                    var extraIngridient = new Ingredient()
                    {
                        Name = ingredientOption.Name,
                        MenuNumber = ingredientOption.MenuNumber,
                        IngredientOption = ingredientOption.IngredientOption,
                        Price = ingredientOption.Price
                    };
                    priceForExtraIngredients += extraIngridient.Price;
                    pizzaToModify.ExtraIngredients.Add(extraIngridient);
                    pizzaToModify.Price = pizzaToModify.Price + priceForExtraIngredients;
                }
            }

            return Ok(request);
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Remove items from shopping cart")]
        public void Delete(int id)
        {

        }
    }
}
