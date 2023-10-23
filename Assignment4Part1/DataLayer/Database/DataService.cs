using System.ComponentModel;
using System.Reflection;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;


namespace DataLayer.Database;

public class DataService
{
    public List<Category> GetCategories()
    {
        var db = new NorthwindContext();
        return db.Categories.ToList();
    }

    public Category? GetCategory(int categoryId)
    {
        var db = new NorthwindContext();
        return db.Categories.FirstOrDefault(x => x.Id == categoryId);
    }

    public bool DeleteCategory(Category category)
    {
        var db = new NorthwindContext();
        return DeleteCategory(category.Id);
    }

    public bool DeleteCategory(int categoryId)
    {
        var db = new NorthwindContext();
        var category = db.Categories.FirstOrDefault(x => x.Id == categoryId);
        if (category != null)
        {
            db.Categories.Remove(category);
            return db.SaveChanges() > 0;
        }
        return false;
    }

    public Category CreateCategory(string name, string description)
    {
        var db = new NorthwindContext();
        var id = db.Categories.Max(x => x.Id) + 1;
        var category = new Category
        {
            Id = id,
            Name = name,
            Description = description
        };

        db.Add(category);
        db.SaveChanges();
        return category;
    }

    public bool UpdateCategory(int Id, string name, string description)
    {
        var db = new NorthwindContext();
        var category = db.Categories.FirstOrDefault(x => x.Id == Id);
        if (category != null)
        {
            category.Name = name;
            category.Description = description;
            db.SaveChanges();
            Console.WriteLine("Succesfully updated");
            return true;
        }

        Console.WriteLine("Update failed");
        return false;

    }

    // need to find an alternative way

    /* public Product GetProduct(int ProductId) 
    {
        var db = new NorthwindContext();
        Product product = db.Products.FirstOrDefault(x => x.Id == ProductId);
        Category category = db.Categories.FirstOrDefault(x => x.Id == product.CategoryId);
    }
    
    } */

    //GetCategoriesByName
    public List<Category> GetCategoriesByName(string name)
    {
        var db = new NorthwindContext();
        return db.Categories.Where(x => x.Name.Contains(name)).ToList();
    }

    public List<Product> GetProductByName(string productName)
    {
        var db = new NorthwindContext();
        return db.Products.Where(x => x.Name.ToLower().Contains(productName.ToLower())
        .Select(x => new AssemblyProductAttribute(x)).ToList());
    }

    //GetProductByName

    //GetProductByCategory

    public Order GetOrder(int orderId)
    {
        var db = new NorthwindContext();
        return db.Orders
            .Include(o => o.OrderDetails)
            .ThenInclude(od => od.Product)
            .ThenInclude(p => p.Category)
            .FirstOrDefault(x => x.Id == orderId);


    }

    public static List<Order> GetOrders()
    {
        var db = new NorthwindContext();
        return db.Orders.ToList();
    }

    public List<OrderDetails> GetOrderDetailsByOrderId (int orderId)
    {
        var db = new NorthwindContext();
        return db.OrderDetails
            .Include(x => x.Product)
            .Where(x => x.OrderId == orderId)
            .ToList();
    }

    public List<OrderDetails> GetOrderDetailsByProductId (int productId)
    {
        var db = new NorthwindContext();
        return db.OrderDetails
            .Include(x => x.Order)
            .Where(x => x.ProductId == productId)
            .OrderBy(x => x.OrderId)
            .ToList();
    }
}