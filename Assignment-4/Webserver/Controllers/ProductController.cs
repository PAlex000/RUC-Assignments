using DataLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Xml.Linq;
using Webserver.Models;
using WebServer.Models;

namespace Webserver.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IDataService _dataService;
        private readonly LinkGenerator _linkGenerator;
        public ProductController(IDataService dataService, LinkGenerator linkGenerator)
        {
            _dataService = dataService;
            _linkGenerator = linkGenerator;
        }
        [HttpGet]
        public IActionResult GetProducts(string? name = null)
        {
            IEnumerable<ProductModel> result = null;
            if (name != null)
            {
                var products = _dataService.GetProductByName(name);
                return products.Count > 0 ? Ok(products) : NotFound(products);
            }
            else
                result = _dataService.GetProducts().Select(CreateProductModel);
            return Ok(result);
        }
        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            var result = _dataService.GetProduct(id);

            return result != null ? Ok(CreateProductModel(result)) : NotFound();
        }
        [HttpGet("category/{categoryId}")]
        public IActionResult ProductsCategory(int categoryId)
        {
            var result = _dataService.GetProductByCategory(categoryId);

            return result.Count() > 0 ? Ok(result) : NotFound(result);
        }
        private ProductModel CreateProductModel(Product product)
        {
            return new ProductModel
            {
                Url = $"http://localhost:5001/api/product/{product.Id}",
                Name = product.Name,
                UnitPrice = product.UnitPrice,
                QuantityPerUnit = product.QuantityPerUnit,
                UnitsInStock = product.UnitsInStock
            };
        }
        private ProductModel CreateProductModel(ProductWithCategoryName product)
        {
            return new ProductModel
            {
                Url = $"http://localhost:5001/api/product/{product.Id}",
                Name = product.Name,
                UnitPrice = product.UnitPrice,
                QuantityPerUnit = product.QuantityPerUnit,
                UnitsInStock = product.UnitsInStock,
                CategoryName = product.CategoryName
            };
        }
    }
}
