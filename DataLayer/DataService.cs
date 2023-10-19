using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DataLayer;

public class DataService
{

    public IList<Category> GetCategories()
    {
        var db = new NorthwindContex();
        return db.Categories.ToList();
    }

    public Category? GetCategory(int categoryId)
    {
        var db = new NorthwindContex();
        return db.Categories.FirstOrDefault(x => x.Id == categoryId);
        //return db.Categories.Find(categoryId);
    }

    public bool DeleteCategory(Category category)
    {
        return DeleteCategory(category.Id);
    }
    public bool DeleteCategory(int categoryId)
    {
        var db = new NorthwindContex();
        var category = db.Categories.FirstOrDefault(x => x.Id == categoryId);
        if (category != null)
        {
            db.Categories.Remove(category);
            //db.Remove(category);
            return db.SaveChanges() > 0;
        }
        return false;
    }

    public Category CreateCategory(string name, string description)
    {
        var db = new NorthwindContex();
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
        var db = new NorthwindContex();
        var category = db.Categories.FirstOrDefault(x => x.Id == Id);

        if (category != null)
        {
            category.Name = name;
            category.Description = description;
            db.SaveChanges();
            Console.WriteLine("update success");
            return true;
        }
        Console.WriteLine("update failed");
        return false;
    }

    //Bogey one. Needs to be re-done because it's not right.
    public Product GetProduct(int id)
    {
        var db = new NorthwindContex();
        var product = db.Products.Include(x => x.Category).FirstOrDefault(x => x.Id == id);

        //if (product != null && product.Category != null)
        //{
        //    product.CategoryName = product.Category.Name;
        //}

        return product;
    }

    public Order GetOrder(int id)
    {
        var db = new NorthwindContex();
        var order = db.Orders.Include(o => o.OrderDetails).ThenInclude(od => od.Product).ThenInclude(p => p.Category).FirstOrDefault(o => o.Id == id);
        return order;
    }

    public List<Order> GetOrders()
    {
        var db = new NorthwindContex();
        return db.Orders.ToList();
    }

    public List<OrderDetails> GetOrderDetailsByOrderId(int id)
    {
        var db = new NorthwindContex();
        var orderDetails = db.OrderDetails.Include(od => od.Product).Where(od => od.OrderId == id).ToList();
        return orderDetails;
    } 

    public List<OrderDetails> GetOrderDetailsByProductId(int id)
    {
        var db = new NorthwindContex();
        var orderDetails = db.OrderDetails.Include(od => od.Order).Where(od=>od.ProductId == id).OrderBy(od => od.OrderId).ToList();
        return orderDetails;
    }


}