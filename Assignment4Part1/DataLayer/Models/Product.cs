﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models;
public class Product
{
    public int CategoryId { get; set; }

    public Category Category { get; set; } = null!;
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public List<OrderDetails> OrderDetails { get; set; } = null!;

    public int UnitPrice { get; set; }
    public int QuantityPerUnit { get; set; }
    public int UnitsInStock { get; set; }
}

