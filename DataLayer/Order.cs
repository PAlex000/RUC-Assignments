using System;
using System.Collections.Generic;

public class Order
{
    public int Id { get; set; }
    public DateTime Date { get; set; } = new DateTime();
    public DateTime Required { get; set; } = new DateTime();
    public List<OrderDetails> OrderDetails { get; set; }
    public string ShipName { get; set; }
    public string ShipCity { get; set; }
}