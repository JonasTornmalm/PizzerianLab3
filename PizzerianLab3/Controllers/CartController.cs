using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzerianLab3.Data;
using PizzerianLab3.Data.Entities;
using PizzerianLab3.Models;
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

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddToCartModel request)
        {
            if (!ModelState.IsValid)
                BadRequest();

            var pizzaRecipe = await _context.Pizzas.Where(x => x.Name == request.Pizza).FirstOrDefaultAsync();

            if (pizzaRecipe == null)
            {
                return BadRequest();
            }

            var pizzaToBake = new Pizza()
            {
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



            await _context.SaveChangesAsync();

            return Ok(request);
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
