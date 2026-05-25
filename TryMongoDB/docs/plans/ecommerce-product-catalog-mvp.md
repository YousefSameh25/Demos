# E-Commerce Product Catalog MVP Plan

## Summary

Build the first MVP as a MongoDB-backed ASP.NET Core Web API focused only on product catalog management. The API will expose product and category management endpoints and keep the existing controller-based Web API style.

## MVP Capabilities

- Replace the default WeatherForecast sample with a real Product Catalog API.
- Support product CRUD: create, read by id, list, update, and delete.
- Support category CRUD.
- Support product filtering by category, active status, price range, and text search by name or description.
- Store catalog data in MongoDB using the official `MongoDB.Driver` package.
- Keep the API documented through Swagger/OpenAPI.

## Key Implementation Changes

- Add MongoDB configuration in `appsettings.json`:
  - connection string key
  - database name
  - products collection name
  - categories collection name
- Add strongly typed options for MongoDB settings and register them in `Program.cs`.
- Add catalog feature structure:
  - MongoDB document models
  - request and response DTOs
  - catalog services for business and data access logic
  - thin API controllers
- Keep DTOs separate from persistence models.
- Use ProblemDetails-compatible API failures for not found, validation errors, and duplicate SKU/category slug conflicts.

## Product Model

- `id`
- `name`
- `description`
- `sku`
- `price`
- `currency`
- `categoryId`
- `imageUrl`
- `stockQuantity`
- `isActive`
- `createdAtUtc`
- `updatedAtUtc`

## Category Model

- `id`
- `name`
- `slug`
- `description`
- `isActive`
- `createdAtUtc`
- `updatedAtUtc`

## API Shape

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

## Test Plan

- Verify through Swagger:
  - create category
  - create product assigned to category
  - list products
  - filter products by category and active status
  - update price and stock
  - delete product
  - verify invalid IDs and missing records return correct errors
- Add automated API/integration tests when a test project is introduced.

## Assumptions

- Keep `.NET 8` because the current project targets `net8.0`.
- Keep controller-based Web API because the project already uses controllers.
- Do not add cart, orders, payment, authentication, or admin UI in this MVP.
- Store all future planning files under `docs/plans`.
