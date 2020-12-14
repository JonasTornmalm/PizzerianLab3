using PizzerianLab3.Data.Entities;
using System.Collections.Generic;

namespace PizzerianLab3
{
    public class OrderStoreSingleton 
    {
        public Dictionary<int, Order> Orders;

        public OrderStoreSingleton()
        {
            Orders = new Dictionary<int, Order>();
        }
    }
}
