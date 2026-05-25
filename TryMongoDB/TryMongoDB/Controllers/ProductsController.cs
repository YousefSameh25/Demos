using Microsoft.AspNetCore.Mvc;
using TryMongoDB.Application.Catalog.Products;

namespace TryMongoDB.Controllers;

[ApiController]
[Route("api/products")]
public sealed class ProductsController(IProductService products) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateProductRequest request,
        CancellationToken cancellationToken)
    {
        var result = await products.CreateAsync(request, cancellationToken);

        return StatusCode(result.StatusCode, result);
    }
}
