# Byte 7: Catalog request workflow

**Builds on:** Bytes 3–5

**File(s):** [frontend/src/views/Products.vue](../frontend/src/views/Products.vue), [frontend/src/stores/catalog.ts](../frontend/src/stores/catalog.ts), [frontend/src/api/products.ts](../frontend/src/api/products.ts), [backend/Ecommerce.Api/Controllers/ProductsController.cs](../backend/Ecommerce.Api/Controllers/ProductsController.cs)

**In plain terms:**

The catalog supports server-side pagination, category filtering, search, sorting, product details, and infinite scrolling.

**The code:**

```ts
const response = await productsApi.getProducts(filters)
products.value = response.data.items
totalCount.value = response.data.totalCount
totalPages.value = response.data.totalPages
```

```csharp
if (!string.IsNullOrWhiteSpace(search))
{
    var term = search.ToLower();
    query = query.Where(p =>
        p.Name.ToLower().Contains(term) ||
        (p.ShortDescription != null && p.ShortDescription.ToLower().Contains(term)));
}
```

**What's happening:**

The view gathers filter values. The catalog store calls the API. The controller builds an EF query, counts the matching records, applies ordering and pagination, and returns products with metadata.

**Connection to the project:**

The product list can replace existing items for normal pagination or append unique items for infinite scrolling.

**Why it matters:**

Server-side filtering prevents the browser from downloading the entire catalog and keeps query behavior consistent.

**Remember:**

The frontend selects catalog criteria; the backend executes the authoritative query.

**Check yourself:**

Why does the catalog response include `totalCount` and `totalPages`?
