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
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }

        // GET api/<OrderController>/5
        [HttpGet]
        [SwaggerOperation(Summary = "Get active orders")]
        public async Task<IActionResult> Get()
        {
            // All non-completed orders
            var orders = await _context.Orders
                .Where(x => x.IsActive)
                .Select(x => new 
                { 
                    x.Id,
                    x.PizzaOrders,
                    x.SodaOrders,
                    x.IngredientOrder,
                    x.TotalPrice
                }).ToListAsync();

            var viewActiveOrders = new List<ViewOrdersDisplayModel>();

            foreach (var order in orders)
            {
                var viewActiveOrderModel = new ViewOrdersDisplayModel();
                foreach (var pizza in order.PizzaOrders)
                {
                    var viewPizza = new PizzaDisplayModel();

                    foreach (var ingredient in pizza.PizzaIngredients)
                    {
                        var viewIngredient = new IngredientDisplayModel();
                        viewIngredient.Name = ingredient.Name;
                        viewPizza.PizzaIngredients.Add(viewIngredient);
                    }

                    viewPizza.MenuNumber = pizza.MenuNumber;
                    viewPizza.Name = pizza.Name;
                    viewPizza.Price = pizza.Price;
                    viewActiveOrderModel.Pizzas.Add(viewPizza);
                }
                foreach (var soda in order.SodaOrders)
                {
                    var viewSoda = new SodaDisplayModel();
                    viewSoda.MenuNumber = soda.MenuNumber;
                    viewSoda.Name = soda.Name;
                    viewSoda.Price = soda.Price;
                    viewActiveOrderModel.Sodas.Add(viewSoda);
                }
                foreach (var extraIngredient in order.IngredientOrder)
                {
                    var viewExtraIngredient = new ExtraIngredientDisplayModel();
                    viewExtraIngredient.MenuNumber = extraIngredient.MenuNumber;
                    viewExtraIngredient.Name = extraIngredient.Name;
                    viewActiveOrderModel.ExtraIngredients.Add(viewExtraIngredient);
                }
                viewActiveOrderModel.OrderId = order.Id;
                viewActiveOrders.Add(viewActiveOrderModel);
            }

            return Ok(viewActiveOrders);
        }

        // POST api/<OrderController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddToCartModel request)
        {
            if (!ModelState.IsValid)
                return BadRequest("A validation error occurred. Please check that all fields have been entered correctly.");

            

            
            return Ok(request);
        }

        // PUT api/<OrderController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdatePizzaOrderModel request)
        {
            if (!ModelState.IsValid)
                return BadRequest("A validation error occurred. Please check that all fields have been entered correctly.");
            
            //var order = await _context.Orders.Where(x => x.IsActive && x.Id == id).FirstOrDefaultAsync();
            //
            //order.PizzaOrders.Where(x => x.Id == )

            return Ok();
        }

        // DELETE api/<OrderController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
