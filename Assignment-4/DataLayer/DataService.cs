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
            //db.Remove(category);
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
    public Product? GetProduct(int productId)
    {
        var db = new NorthwindContex();
        Product product = db.Products.First(x => x.Id == productId);
        return product;
    }
}
