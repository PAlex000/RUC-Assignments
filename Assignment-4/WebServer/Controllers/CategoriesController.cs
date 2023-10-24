using DataLayer;
using Microsoft.AspNetCore.Mvc;
using WebServer.Models;

namespace WebServer.Controllers;

[Route("api/categories")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly IDataService _dataService;
    private readonly LinkGenerator _linkGenerator;

    public CategoriesController(IDataService dataService, LinkGenerator linkGenerator)
    {
        _dataService = dataService;
        _linkGenerator = linkGenerator;
    }

    [HttpGet]
    public IActionResult GetCetagories()
    {
        IEnumerable<CategoryModel> result = null;

        return Ok(_dataService.GetCategories()
                .Select(CreateCategoryModel));
    }
    
    [HttpGet("{id}", Name = nameof(GetCategory))]
    public IActionResult GetCategory(int id)
    {
        var category = _dataService.GetCategory(id);
        if(category == null)
        {
            return NotFound();
        }

        return Ok(CreateCategoryModel(category));
    }

    [HttpPost]
    public IActionResult CreateCategory(CreateCategoryModel model)
    {
        var category = new Category
        {
            Name = model.Name,
            Description = model.Description
        };

        _dataService.CreateCategory(model.Name, model.Description);

        return Ok(category);
    }


    [HttpPut("{id}")]
    public IActionResult UpdateCategory(int id)
    {

        var category = _dataService.DeleteCategory(id);

        if (!category)
        {
            return NotFound();
        }
        return Ok(category);
    }

    [HttpDelete]
    public IActionResult DeleteCategory(Category model)
    {

        var category = _dataService.UpdateCategory(model.Id, model.Name, model.Description);

        if (!category)
        {
            return NotFound();
        }
        return Ok(category);
    }


    private CategoryModel CreateCategoryModel(Category category)
    {
        return new CategoryModel
        {
            //Url = $"http://localhost:5001/api/categories/{category.Id}",
            Url = GetUrl(nameof(GetCategory), new { category.Id }),
            Name = category.Name,
            Description = category.Description
        };
    }



    private string? GetUrl(string name, object values)
    {
        return _linkGenerator.GetUriByName(HttpContext, name, values);
    }



}
