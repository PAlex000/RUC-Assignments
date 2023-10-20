using System;
using System.Collections.Generic;

namespace DataLayer;
    public class OrderDetails
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int OrderId { get; set; }

        //public Order Order { get; set; }
        //public Product Product { get; set; }
        public double UnitPrice { get; set; }
        public double Quantity { get; set; }
        public double Discount { get; set; }
    }

