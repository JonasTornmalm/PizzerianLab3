﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzerianLab3.Data.Entities
{
    public class Soda : Entity
    {
        public int MenuNumber { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
    }
}
