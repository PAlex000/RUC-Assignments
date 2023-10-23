using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer
{
    public interface IDataService
    {
        IList<Category> GetCategories();
        Category? GetCategory(int categoryId);
        Category CreateCategory(string name, string description);
        bool DeleteCategory(Category category);
        bool DeleteCategory(int categoryId);
        bool UpdateCategory(int id, string name, string description);
        ProductWithCategoryName GetProduct(int productId);
        List<ProductWithCategoryName> GetProductByCategory(int categoryId);
        List<ProductAndCategoryNames> GetProductByName(string productName);
        IList<Category> GetCategoriesByName(string name);
        Order GetOrder(int orderId);
        List<Order> GetOrders();
        List<OrderDetails> GetOrderDetailsByOrderId(int orderId);
        List<OrderDetails> GetOrderDetailsByProductId(int productId);
    }
}
