using System;
using System.Collections.Generic;

public class Order
{
    public int Id { get; set; } = 0;
    public DateTime Date { get; set; } = new DateTime();
    public DateTime Required { get; set; } = new DateTime();
    public List<OrderDetails> OrderDetails { get; set; } = null;
    public string ShipName { get; set; } = null;
    public string ShipCity { get; set; } = null;
}