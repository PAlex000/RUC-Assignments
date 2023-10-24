using DataLayer;
using Microsoft.AspNetCore.Mvc;
using WebServer.Models;

namespace WebServer.Controllers { 

[Route("api/product")]
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

        [HttpGet("{id}", Name = nameof(GetProduct))]
        public IActionResult GetProduct(int id)
        {
            var product = _dataService.GetProduct(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpGet("category/{id}")]
        public IActionResult GetProductsByCategory(int id)
        {
            var result = _dataService.GetProductByCategory(id);

            if (result.Count == 0)
            {
                return NotFound();

            }

            return Ok(result);
        }

        [HttpGet("name/{name}")]
        public IActionResult GetProductByName(string name)
        {
            var result = _dataService.GetProductByName(name);

            if (result.Count == 0)
            {
                return NotFound();

            }

            return Ok(result);
        }


    }
}

