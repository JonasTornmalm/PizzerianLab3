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
                    x.Pizzas,
                    x.Sodas,
                    x.TotalPrice
                }).ToListAsync();

            var viewActiveOrders = new List<ResponseViewModel>();

            double totalPrice = 0;
            foreach (var order in orders)
            {
                var viewActiveOrderModel = new ResponseViewModel();
                foreach (var pizza in order.Pizzas)
                {
                    var viewPizza = new PizzaViewModel();

                    foreach (var ingredient in pizza.PizzaIngredients)
                    {
                        var viewIngredient = new IngredientViewModel();
                        viewIngredient.Name = ingredient.Name;
                        viewPizza.PizzaIngredients.Add(viewIngredient);
                    }

                    foreach (var extraIngredient in pizza.ExtraIngredients)
                    {
                        var viewExtraIngredient = new ExtraIngredientViewModel();
                        viewExtraIngredient.MenuNumber = extraIngredient.MenuNumber;
                        viewExtraIngredient.Name = extraIngredient.Name;
                        viewExtraIngredient.Price = extraIngredient.Price;
                        viewPizza.ExtraIngredients.Add(viewExtraIngredient);
                    }

                    viewPizza.PizzaId = pizza.Id;
                    viewPizza.MenuNumber = pizza.MenuNumber;
                    viewPizza.Name = pizza.Name;
                    viewPizza.Price = pizza.Price;
                    totalPrice += pizza.Price;
                    viewActiveOrderModel.Pizzas.Add(viewPizza);
                }
                foreach (var soda in order.Sodas)
                {
                    var viewSoda = new SodaViewModel();
                    viewSoda.MenuNumber = soda.MenuNumber;
                    viewSoda.Name = soda.Name;
                    viewSoda.Price = soda.Price;
                    totalPrice += soda.Price;
                    viewActiveOrderModel.Sodas.Add(viewSoda);
                }
                viewActiveOrderModel.OrderId = order.Id;
                viewActiveOrderModel.TotalPrice = totalPrice;
                viewActiveOrders.Add(viewActiveOrderModel);
                totalPrice = 0;
            }

            return Ok(viewActiveOrders);
        }

        // POST api/<OrderController>
        [HttpPost]
        [SwaggerOperation(Summary = "Place order with items from customer cart")]
        public async Task<IActionResult> Post()
        {
            if (!ModelState.IsValid)
                BadRequest("Bad request");

            var itemsInCart = _cart.Order;
            var order = new Order()
            {
                Id = Guid.NewGuid(),
                TotalPrice = itemsInCart.TotalPrice
            };

            foreach (var pizza in itemsInCart.Pizzas)
            {
                var bakingPizza = new Pizza()
                {
                    Id = Guid.NewGuid(),
                    Name = pizza.Name,
                    MenuNumber = pizza.MenuNumber,
                    Price = pizza.Price
                };

                foreach (var ingredient in pizza.PizzaIngredients)
                {
                    var freshIngredient = new Ingredient()
                    {
                        Id = Guid.NewGuid(),
                        Name = ingredient.Name,
                        MenuNumber = ingredient.MenuNumber,
                        IngredientOption = ingredient.IngredientOption,
                        Price = ingredient.Price
                    };
                    bakingPizza.PizzaIngredients.Add(freshIngredient);
                }

                foreach (var extraIngredient in pizza.ExtraIngredients)
                {
                    var freshExtraIngredients = new Ingredient()
                    {
                        Id = Guid.NewGuid(),
                        Name = extraIngredient.Name,
                        MenuNumber = extraIngredient.MenuNumber,
                        IngredientOption = extraIngredient.IngredientOption,
                        Price = extraIngredient.Price
                    };
                    bakingPizza.ExtraIngredients.Add(freshExtraIngredients);
                }

                order.Pizzas.Add(bakingPizza);
            }

            foreach (var soda in itemsInCart.Sodas)
            {
                var servingSoda = new Soda()
                {
                    Id = Guid.NewGuid(),
                    MenuNumber = soda.MenuNumber,
                    Name = soda.Name,
                    Price = soda.Price
                };
                order.Sodas.Add(servingSoda);
            }

            _context.Add(order);
            await _context.SaveChangesAsync();

            return Ok($"Your order has been placed {order.Id}");
        }

        // PUT api/<OrderController>/5
        [HttpPut]
        [SwaggerOperation(Summary = "Update placed order")]
        public async Task<IActionResult> Put([FromBody] UpdateOrderStatusDTO request)
        {
            if (!ModelState.IsValid)
                BadRequest("Bad request");

            if (!request.Completed)
                Ok("No changes has been made.");

            var order = await _context.Orders.Where(x => x.IsActive && x.Id == request.OrderId).FirstOrDefaultAsync();

            if (order == null)
                return BadRequest("There's no active order with that Id.");

            order.IsActive = false;
            _context.Update(order);
            await _context.SaveChangesAsync();

            return Ok($"Order {order.Id} completed");
        }

        // DELETE api/<OrderController>/5
        [HttpDelete]
        [SwaggerOperation(Summary = "Cancel placed order")]
        public async Task<IActionResult> Delete([FromBody] CancelOrderDTO request)
        {
            if (!ModelState.IsValid)
                BadRequest("Bad request");

            if (!string.IsNullOrWhiteSpace(request.OrderIds.FirstOrDefault()))
            {
                foreach (var orderId in request.OrderIds)
                {
                    var order = await _context.Orders.Where(x => x.IsActive && x.Id.ToString() == orderId).FirstOrDefaultAsync();

                    if (order != null)
                    {
                        order.IsActive = false;
                        _context.Update(order);
                        await _context.SaveChangesAsync();
                    }
                    else
                        return BadRequest("There's no active order with that Id.");
                }
            }

            return Ok(new { canceledOrders = request.OrderIds });
        }
    }
}
