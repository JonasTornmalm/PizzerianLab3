using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzerianLab3.DTOs
{
    public class UpdateOrderStatusDTO
    {
        public bool CompleteOrder { get; set; }
        public bool CancelOrder { get; set; }
    }
}
