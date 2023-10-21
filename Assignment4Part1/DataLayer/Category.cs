﻿using System;
using System.Collections.Generic;

namespace DataLayer;
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public IList<Product> Products { get; set; } = null!;

    }

