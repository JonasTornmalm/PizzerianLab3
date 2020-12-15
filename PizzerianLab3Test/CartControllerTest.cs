using Microsoft.VisualStudio.TestTools.UnitTesting;
using PizzerianLab3;
using PizzerianLab3.Controllers;
using PizzerianLab3.Data;
using PizzerianLab3.Data.Entities;
using System.Linq;

namespace PizzerianLab3Test
{
    [TestClass]
    public class CartControllerTest
    {
        [TestMethod]
        public void AddToCartContentTest()
        {
            var cart = new CartSingleton();

            cart.Order.Pizzas.Add(new Pizza());

            var pizzaCount = cart.Order.Pizzas.Count();
            var sodaCount = cart.Order.Sodas.Count();

            var expected = 1;

            var actual = pizzaCount + sodaCount;

            Assert.AreEqual(expected, actual);
        }
    }
}
