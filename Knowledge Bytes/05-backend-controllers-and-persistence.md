# Byte 5: Controllers, DTOs, and persistence

**Builds on:** Bytes 1–2

**File(s):** [backend/Ecommerce.Api/Controllers/ProductsController.cs](../backend/Ecommerce.Api/Controllers/ProductsController.cs), [backend/Ecommerce.Core/DTOs/ProductDTOs.cs](../backend/Ecommerce.Core/DTOs/ProductDTOs.cs), [backend/Ecommerce.Infrastructure/Data/AppDbContext.cs](../backend/Ecommerce.Infrastructure/Data/AppDbContext.cs)

**In plain terms:**

Controllers translate HTTP requests into database queries and return DTOs. A DTO is a deliberate request or response shape rather than the complete database entity.

**The code:**

```csharp
var products = await query
    .Skip((page - 1) * pageSize)
    .Take(pageSize)
    .Select(p => new ProductDto
    {
        Id = p.Id,
        Name = p.Name,
        Price = p.Price,
        InStock = p.InStock
    })
    .ToListAsync();
```

**What's happening:**

EF Core builds a query for the database, applies pagination, and projects only the fields needed by the API response.

**Connection to the project:**

`ProductsController` uses `AppDbContext.Products`, while the frontend receives the resulting `PagedResult<ProductDto>` through `productsApi`.

**Why it matters:**

DTO projection reduces accidental data exposure and keeps the API contract independent from the database model.

**Remember:**

Entities model persistence; DTOs model communication.

**Check yourself:**

Why should a controller usually avoid returning a full EF entity directly?
