using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzerianLab3.Data;
using PizzerianLab3.Data.Entities;
using PizzerianLab3.DTOs;
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
        private readonly CartSingleton _cart;

        public OrderController(AppDbContext context, CartSingleton cart)
        {
            _context = context;
            _cart = cart;
        }

        // GET api/<OrderController>/5
        [HttpGet]
        [SwaggerOperation(Summary = "Get all orders in progress")]
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
                    x.TotalPrice
                }).ToListAsync();

            var viewActiveOrders = new List<DisplayResponseModel>();

            foreach (var order in orders)
            {
                var viewActiveOrderModel = new DisplayResponseModel();
                foreach (var pizza in order.PizzaOrders)
                {
                    var viewPizza = new PizzaDisplayModel();

                    foreach (var ingredient in pizza.PizzaIngredients)
                    {
                        var viewIngredient = new IngredientDisplayModel();
                        viewIngredient.Name = ingredient.Name;
                        viewPizza.PizzaIngredients.Add(viewIngredient);
                    }

                    foreach (var extraIngredient in pizza.ExtraIngredients)
                    {
                        var viewExtraIngredient = new ExtraIngredientDisplayModel();
                        viewExtraIngredient.MenuNumber = extraIngredient.MenuNumber;
                        viewExtraIngredient.Name = extraIngredient.Name;
                        viewExtraIngredient.Price = extraIngredient.Price;
                        viewPizza.ExtraIngredients.Add(viewExtraIngredient);
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
                viewActiveOrderModel.OrderId = order.Id;
                viewActiveOrders.Add(viewActiveOrderModel);
            }

            return Ok(viewActiveOrders);
        }

        // POST api/<OrderController>
        [HttpPost]
        [SwaggerOperation(Summary = "Place order with items from customer cart")]
        public async Task<IActionResult> Post()
        {
            if (!ModelState.IsValid)
                return BadRequest("A validation error occurred. Please check that all fields have been entered correctly.");

            var itemsInCart = _cart.Order;
            var order = new Order()
            {
                Id = Guid.NewGuid(),
                TotalPrice = itemsInCart.TotalPrice
            };

            foreach (var pizza in itemsInCart.PizzaOrders)
                order.PizzaOrders.Add(pizza);

            foreach (var soda in itemsInCart.SodaOrders)
                order.SodaOrders.Add(soda);

            _context.Add(order);
            await _context.SaveChangesAsync();
            
            return Ok("Your order has been placed.");
        }

        // PUT api/<OrderController>/5
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update placed order")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateOrderStatusDTO request)
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
        [SwaggerOperation(Summary = "Delete placed order")]
        public void Delete(int id)
        {
        }
    }
}
