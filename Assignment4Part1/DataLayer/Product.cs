using System;
using System.Collections.Generic;

namespace DataLayer;
    public class Product
    {
        public int CategoryId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public List<OrderDetails> OrderDetails { get; set; }
        public double UnitPrice { get; set; }
        public double QuantityPerUnit { get; set; }
        public double UnitInStick { get; set; }
    }

