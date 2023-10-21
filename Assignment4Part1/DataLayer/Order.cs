using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataLayer;

public class Order
{
    [Key]
    public int Id { get; set; }
    public DateTime? Date { get; set; } = new DateTime();
    public DateTime? Required { get; set; } = new DateTime();
    public bool Shipped { get; set; }
    public string Freight { get; set; } = null!;
    public string ShipName { get; set; } = null!;
    public string ShipCity { get; set; } = null!;
    public List<OrderDetails> OrderDetails { get; set; } = null!;

}