using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzerianLab3.Data.Entities
{
    public abstract class Entity
    {
        public Entity()
        {
            Created = Modified = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }

        public Guid Id { get; set; }
        public int Version { get; set; } = 1;
        public long Created { get; set; }
        public long Modified { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
