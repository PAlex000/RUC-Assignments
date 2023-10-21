using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer;
    public class Product
    {
        public int CategoryId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public IList<OrderDetails> OrderDetails { get; set; } = null!;

        [Column(TypeName = "decimal(6,2)")]
        public decimal UnitPrice { get; set; }
        public int QuantityPerUnit { get; set; }

        [Column(TypeName = "decimal(6,2)")]
        public decimal UnitInStick { get; set; }
    }

