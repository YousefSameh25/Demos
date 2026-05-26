using Microsoft.AspNetCore.Mvc;
using TryMongoDB.Application.Catalog.Products;

namespace TryMongoDB.Controllers;

[ApiController]
[Route("api/[controller]")]
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

    [HttpPost("{productId}/variants")]
    public async Task<IActionResult> AddVariant(
        string productId,
        CreateProductVariantRequest request,
        CancellationToken cancellationToken)
    {
        var result = await products.AddVariantAsync(productId, request, cancellationToken);

        return StatusCode(result.StatusCode, result);
    }
}
