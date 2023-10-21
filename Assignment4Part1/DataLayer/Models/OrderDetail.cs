using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models;
public class OrderDetails
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
    public Product Product { get; set; } = null!;
    public int Quantity { get; set; }

    [Column(TypeName = "decimal(6,2)")]
    public decimal Discount { get; set; }

    [Column(TypeName = "decimal(6,2)")]
    public decimal UnitPrice { get; set; }
}

