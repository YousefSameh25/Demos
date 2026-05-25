namespace TryMongoDB.Domain.Categories;

public sealed class Category
{
    private Category(
        string id,
        string name,
        string slug,
        string description,
        bool isActive,
        DateTime createdAtUtc,
        DateTime updatedAtUtc)
    {
        Id = id;
        Name = name;
        Slug = slug;
        Description = description;
        IsActive = isActive;
        CreatedAtUtc = createdAtUtc;
        UpdatedAtUtc = updatedAtUtc;
    }

    public string Id { get; }
    public string Name { get; private set; }
    public string Slug { get; }
    public string Description { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAtUtc { get; }
    public DateTime UpdatedAtUtc { get; private set; }

    public static Category Create(string name, string slug, string description, DateTime utcNow)
    {
        Validate(name, slug, description);

        return new Category(
            Guid.NewGuid().ToString("N"),
            name.Trim(),
            slug.Trim().ToLowerInvariant(),
            description.Trim(),
            true,
            utcNow,
            utcNow);
    }

    public static Category Rehydrate(
        string id,
        string name,
        string slug,
        string description,
        bool isActive,
        DateTime createdAtUtc,
        DateTime updatedAtUtc)
    {
        Validate(name, slug, description);

        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("Category id is required.", nameof(id));
        }

        return new Category(
            id.Trim(),
            name.Trim(),
            slug.Trim().ToLowerInvariant(),
            description.Trim(),
            isActive,
            createdAtUtc,
            updatedAtUtc);
    }

    public void UpdateDetails(string name, string description, bool isActive, DateTime utcNow)
    {
        Validate(name, Slug, description);

        Name = name.Trim();
        Description = description.Trim();
        IsActive = isActive;
        UpdatedAtUtc = utcNow;
    }

    private static void Validate(string name, string slug, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Category name is required.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(slug))
        {
            throw new ArgumentException("Category slug is required.", nameof(slug));
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("Category description is required.", nameof(description));
        }
    }
}
