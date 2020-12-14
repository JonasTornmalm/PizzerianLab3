using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzerianLab3.DTOs
{
    public class DeleteFromCartDTO
    {
        public List<string> PizzasIds { get; set; }
        public List<string> SodasIds { get; set; }
    }
}
