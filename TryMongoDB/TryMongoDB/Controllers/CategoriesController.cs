using Microsoft.AspNetCore.Mvc;
using TryMongoDB.Application.Catalog.Categories;

namespace TryMongoDB.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class CategoriesController(ICategoryService categories) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var result = await categories.CreateAsync(request, cancellationToken);

        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken)
    {
        var result = await categories.GetByIdAsync(id, cancellationToken);

        return StatusCode(result.StatusCode, result);
    }

    [HttpGet]
    public async Task<IActionResult> List(CancellationToken cancellationToken)
    {
        var result = await categories.ListAsync(cancellationToken);

        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        string id,
        UpdateCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var result = await categories.UpdateAsync(id, request, cancellationToken);

        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
    {
        var result = await categories.DeleteAsync(id, cancellationToken);

        return StatusCode(result.StatusCode, result);
    }
}
