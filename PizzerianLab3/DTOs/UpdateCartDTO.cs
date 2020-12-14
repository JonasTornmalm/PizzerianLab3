using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzerianLab3.DTOs
{
    public class UpdateCartDTO
    {
        public Guid PizzaId { get; set; }
        public List<ExtraIngredientDTO> ExtraIngredients { get; set; }
    }
}
