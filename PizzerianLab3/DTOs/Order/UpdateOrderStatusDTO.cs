using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzerianLab3.DTOs
{
    public class UpdateOrderStatusDTO
    {
        public Guid OrderId { get; set; }
        public bool Completed { get; set; }
    }
}
