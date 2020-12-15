using Microsoft.VisualStudio.TestTools.UnitTesting;
using PizzerianLab3;
using PizzerianLab3.Data.Entities;

namespace PizzerianLab3Test
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void OrderIsEmptyTest()
        {
            var orderNotEmpty = new Order();
            var orderEmpty = new Order();

            orderNotEmpty.Pizzas.Add(new Pizza());

            Assert.IsFalse(orderNotEmpty.IsEmpty);
            Assert.IsTrue(orderEmpty.IsEmpty);
        }

        [TestMethod]
        public void IngredientNameFromEnumTest()
        {
            var ingredient = new Ingredient();
            ingredient.IngredientOption = IngredientEnum.Artichoke;

            var expected = "Artichoke";

            var actual = ingredient.Name;

            Assert.AreEqual(expected, actual);
        }
    }
}
