using System.ComponentModel;
using Microsoft.EntityFrameworkCore;

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

    public Product GetProduct(int id)
    {
        var db = new NorthwindContex();
        var product = db.Products.Include(x => x.Category).FirstOrDefault(x => x.Id == id);

        if (product != null && product.Category != null)
        {
            product.CategoryName = product.Category.Name;
        }

        return product;
    }

    //Error @ Don't know what pk to use. Must define a composite key ?
    public Order GetOrder(int id)
    {
        var db = new NorthwindContex();
        var order = db.Orders.Include(o => o.OrderDetails).ThenInclude(od => od.Product).ThenInclude(p => p.Category).FirstOrDefault(o => o.Id == id);
        return order;
    }

}