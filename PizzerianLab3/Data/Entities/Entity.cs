using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzerianLab3.Data.Entities
{
    public abstract class Entity
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
