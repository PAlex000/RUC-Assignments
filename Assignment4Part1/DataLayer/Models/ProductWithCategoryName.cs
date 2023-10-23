﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models;

public class ProductWithCategoryName
{
    public int Id { get; set; }
    public string CategoryName { get; set; } 
    public string Name { get; set; } 
    public int UnitPrice { get; set; }
    public int QuantityPerUnit { get; set; }
    public int UnitsInStock { get; set; }
}
