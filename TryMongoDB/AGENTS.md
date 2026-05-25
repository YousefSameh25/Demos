# Project Instructions

This project is being built primarily to learn MongoDB with ASP.NET Core.

When making changes:

- Teach the MongoDB idea behind the code, not only the C# implementation.
- Prefer clear, learning-friendly designs over premature abstractions.
- Explain MongoDB document modeling choices, including when to embed data, reference data, use flexible attributes, create indexes, and avoid relational patterns.
- Keep examples close to the current e-commerce catalog domain.
- Call out tradeoffs explicitly so the project helps build judgment, not just working code.
- For product documents, keep only common/catalog-wide fields as top-level properties. Product-level custom details belong in the product `Attributes` dictionary unless they become common, heavily queried, or need their own index.
- Treat product variants as the sellable items. Variant-specific values such as SKU, price, stock quantity, phone storage, clothing size, color, material, or screen size belong in separate product variant documents, with each variant having its own `Attributes`.
- Prefer a separate `productVariants` collection over embedded variants when reasoning from the Amazon-like model, because variants have their own lifecycle, indexing needs, inventory updates, and pricing behavior. Only suggest embedded variants when deliberately simplifying for a small learning scenario.
- When suggesting catalog, product, variant, pricing, inventory, search, or ordering models, reason from an Amazon-like marketplace/e-commerce system. Explain when the Amazon-scale design should be simplified for this MongoDB learning project and when it should influence the model.
