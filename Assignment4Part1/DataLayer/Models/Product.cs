using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models;
public class Product
{
    public int CategoryId { get; set; }

    public Category Category { get; set; }
    public int Id { get; set; }
    public string Name { get; set; } 
    public List<OrderDetails> OrderDetails { get; set; }

    public int UnitPrice { get; set; }
    public int QuantityPerUnit { get; set; }
    public int UnitsInStock { get; set; }
}

