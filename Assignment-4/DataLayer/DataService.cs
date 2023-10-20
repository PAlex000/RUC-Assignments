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
            return db.SaveChanges() > 0;
        }
        return false;
    }
    public bool UpdateCategory(int id, string name, string description)
    {
        var db = new NorthwindContex();
        var category = db.Categories.FirstOrDefault(x => x.Id == id);
        if (category != null)
        {
            category.Name = name;
            category.Description = description;
            db.Update(category);
            return db.SaveChanges() > 0;
        }
        return false;
    }
    public ProductWithCategoryName GetProduct(int productId)
    {
        var db = new NorthwindContex();
        Product product = db.Products.FirstOrDefault(x => x.Id == productId);
        Category category = db.Categories.FirstOrDefault(x => x.Id == product.CategoryId);
        ProductWithCategoryName result = new ProductWithCategoryName
        {
            Id = product.Id,
            Name = product.Name,
            UnitPrice = product.UnitPrice,
            QuantityPerUnit = product.QuantityPerUnit,
            UnitsInStock = product.UnitsInStock,
            CategoryName = category.Name
        };
        return result;
    }
    public List<ProductWithCategoryName> GetProductByCategory(int categoryId)
    {
        var db = new NorthwindContex();
        return db.Products
            .Where(x => x.CategoryId == categoryId)
            .Select(x => new ProductWithCategoryName
            {
                Id = x.Id,
                Name = x.Name,
                UnitPrice = x.UnitPrice,
                QuantityPerUnit = x.QuantityPerUnit,
                UnitsInStock = x.UnitsInStock,
                CategoryName = x.Category.Name
            })
            .ToList();
    }
    public List<ProductAndCategoryNames> GetProductByName(string productName)
    {
        var db = new NorthwindContex();

        return db.Products
            .Where(x => x.Name.ToLower().Contains(productName.ToLower()))
            .Select(x => new ProductAndCategoryNames { ProductName = x.Name, CategoryName = x.Category.Name })
            .ToList();
    }

    public Order GetOrder(int orderId)
    {
        var db = new NorthwindContex();
        return db.Orders
            .Include(o => o.OrderDetails)
            .ThenInclude(od => od.Product)
            .ThenInclude(p => p.Category)
            .FirstOrDefault(x => x.Id == orderId);
    }

    public List<Order> GetOrders()
    {
        var db = new NorthwindContex();
        return db.Orders.ToList();
    }
}
