using TryMongoDB.Application.Common;
using TryMongoDB.Domain.Categories;

namespace TryMongoDB.Application.Catalog.Categories;

public sealed class CategoryService(ICategoryRepository categories) : ICategoryService
{
    public async Task<ApplicationResult<CategoryDto>> CreateAsync(
        CreateCategoryRequest request,
        CancellationToken cancellationToken)
    {
        if (await categories.ExistsBySlugAsync(request.Slug, cancellationToken))
        {
            return ApplicationResult<CategoryDto>.Failure(ApplicationErrorType.Conflict, "Category slug already exists.");
        }

        var category = Category.Create(request.Name, request.Slug, request.Description, DateTime.UtcNow);
        await categories.AddAsync(category, cancellationToken);

        return ApplicationResult<CategoryDto>.Success(category.ToDto());
    }

    public async Task<ApplicationResult<CategoryDto>> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        var category = await categories.GetByIdAsync(id, cancellationToken);

        return category is null
            ? ApplicationResult<CategoryDto>.Failure(ApplicationErrorType.NotFound, "Category was not found.")
            : ApplicationResult<CategoryDto>.Success(category.ToDto());
    }

    public async Task<IReadOnlyCollection<CategoryDto>> ListAsync(CancellationToken cancellationToken)
    {
        var result = await categories.ListAsync(cancellationToken);

        return result.Select(category => category.ToDto()).ToArray();
    }

    public async Task<ApplicationResult<CategoryDto>> UpdateAsync(
        string id,
        UpdateCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var category = await categories.GetByIdAsync(id, cancellationToken);
        if (category is null)
        {
            return ApplicationResult<CategoryDto>.Failure(ApplicationErrorType.NotFound, "Category was not found.");
        }

        category.UpdateDetails(request.Name, request.Description, request.IsActive, DateTime.UtcNow);
        await categories.UpdateAsync(category, cancellationToken);

        return ApplicationResult<CategoryDto>.Success(category.ToDto());
    }

    public async Task<ApplicationResult<bool>> DeleteAsync(string id, CancellationToken cancellationToken)
    {
        var category = await categories.GetByIdAsync(id, cancellationToken);
        if (category is null)
        {
            return ApplicationResult<bool>.Failure(ApplicationErrorType.NotFound, "Category was not found.");
        }

        await categories.DeleteAsync(id, cancellationToken);

        return ApplicationResult<bool>.Success(true);
    }
}
