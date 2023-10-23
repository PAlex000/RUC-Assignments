using DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataLayer
{
    public class DataService : IDataService
    {
        private readonly NorthwindContex db = new NorthwindContex();
        public IList<Category> GetCategories()
        {
            return db.Categories.ToList();
        }
        public Category? GetCategory(int categoryId)
        {
            return db.Categories.FirstOrDefault(x => x.Id == categoryId);
        }
        public Category CreateCategory(string name, string description)
        {
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
            return db.Products
                .Where(x => x.Name.ToLower().Contains(productName.ToLower()))
                .Select(x => new ProductAndCategoryNames { ProductName = x.Name, CategoryName = x.Category.Name })
                .ToList();
        }
        public IList<Category> GetCategoriesByName(string name)
        {
            return db.Categories.Where(x => x.Name.Contains(name)).ToList();
        }

        public Order GetOrder(int orderId)
        {
            return db.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .ThenInclude(p => p.Category)
                .FirstOrDefault(x => x.Id == orderId);
        }

        public List<Order> GetOrders()
        {
            return db.Orders.ToList();
        }

        public List<OrderDetails> GetOrderDetailsByOrderId(int orderId)
        {
            return db.OrderDetails
                .Include(x => x.Product)
                .Where(x => x.OrderId == orderId)
                .ToList();
        }
        public List<OrderDetails> GetOrderDetailsByProductId(int productId)
        {
            return db.OrderDetails
                .Include(x => x.Order)
                .Where(x => x.ProductId == productId)
                .OrderBy(x => x.OrderId)
                .ToList();
        }
    }
}
