using System.ComponentModel;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;


namespace DataLayer.Database;

public class DataService
{
    public IList<Category> GetCategories()
    {
        var db = new NorthwindContext();
    }
}

// public DeleteCategory
// GetGategory
// CreateCategory
// UpdateCategory
// Addproduct