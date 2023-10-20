using System.ComponentModel;
using Microsoft.EntityFrameworkCore;


namespace DataLayer;

public class DataService
{
    public IList<Category> GetCategories()
    {
        var db = new NorthwindContex();
    }
}

// public DeleteCategory
// GetGategory
// CreateCategory
// UpdateCategory
// Addproduct