using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models;
public class OrderDetails
{
    public int ProductId { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; }
    public Product Product { get; set; }
    public int Quantity { get; set; }
    public int Discount { get; set; }
    public int UnitPrice { get; set; }
}

