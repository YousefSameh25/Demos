# Clean Architecture Work Order for Product Catalog MVP

## Summary

Implement the product catalog MVP by converting the current single Web API project into a Clean Architecture solution. Build from the center outward: `Domain`, then `Application`, then `Infrastructure`, then `API`.

## Work Order

1. Create the Clean Architecture project structure.
   - Add `TryMongoDB.Domain`, `TryMongoDB.Application`, and `TryMongoDB.Infrastructure`.
   - Keep the existing `TryMongoDB` project as the API layer.
   - Reference direction must remain inward: API and Infrastructure depend on Application; Application depends on Domain.

2. Build the Domain layer first.
   - Add `Product` and `Category` domain entities.
   - Add core rules for required names, valid price, valid stock quantity, SKU identity, active state, and timestamps.
   - Keep this layer free from ASP.NET Core, MongoDB, persistence attributes, DTOs, and configuration.

3. Build the Application layer.
   - Add product and category use cases or application services.
   - Add request/response DTOs for catalog operations.
   - Add repository interfaces such as `IProductRepository` and `ICategoryRepository`.
   - Add product list filtering by category, active status, price range, and search text.
   - Define application-level results for success, validation failure, not found, and duplicate conflicts.

4. Build the Infrastructure layer.
   - Add MongoDB persistence implementation.
   - Add infrastructure entity/document models separate from domain entities and API DTOs.
   - Configure infrastructure entities using fluent configuration rather than attributes.
   - Implement repository interfaces from the Application layer.
   - Configure indexes for unique product SKU and unique category slug.

5. Build the API layer last.
   - Remove the WeatherForecast sample.
   - Add `ProductsController` and `CategoriesController`.
   - Keep controllers thin; they should call Application services only.
   - Register Application and Infrastructure services in `Program.cs`.
   - Add MongoDB configuration to `appsettings.json`.

## API Surface

- `GET /api/products`
- `GET /api/products/{id}`
- `POST /api/products`
- `PUT /api/products/{id}`
- `DELETE /api/products/{id}`
- `GET /api/categories`
- `GET /api/categories/{id}`
- `POST /api/categories`
- `PUT /api/categories/{id}`
- `DELETE /api/categories/{id}`

## Response Rules

- Return `200 OK` for successful reads and updates.
- Return `201 Created` for successful creates.
- Return `204 NoContent` for successful deletes.
- Return `400 BadRequest` for validation failures.
- Return `404 NotFound` for missing records.
- Return `409 Conflict` for duplicate SKU or category slug.

## Test Plan

- Run `dotnet build TryMongoDB.sln` after project wiring and after feature completion.
- Add tests under a future `TryMongoDB.Tests` project.
- Cover domain rules, application use cases, repository behavior, and API response codes.
- Manually verify through Swagger: create category, create product, list/filter products, update product, delete product, and validate error responses.

## Assumptions

- Keep `.NET 8`.
- Keep controller-based Web API.
- Keep MongoDB persistence isolated in `Infrastructure`.
- Do not add cart, orders, payment, authentication, or UI in this MVP.
