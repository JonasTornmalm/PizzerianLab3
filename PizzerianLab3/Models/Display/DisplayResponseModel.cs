using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzerianLab3.Models
{
    public class DisplayResponseModel
    {
		public Guid OrderId { get; set; }
		public List<PizzaDisplayModel> Pizzas { get; set; } = new List<PizzaDisplayModel>();
		public List<SodaDisplayModel> Sodas { get; set; } = new List<SodaDisplayModel>();
		public double TotalPrice { get; set; }
	}
}
