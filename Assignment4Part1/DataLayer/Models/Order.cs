using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataLayer.Models;

public class Order
{
    public int Id { get; set; }
    public DateTime Date { get; set; } = new DateTime();
    public DateTime Required { get; set; } = new DateTime();
    public bool Shipped { get; set; }
    public string ShipName { get; set; } 
    public string ShipCity { get; set; } 
    public List<OrderDetails> OrderDetails { get; set; } 

}