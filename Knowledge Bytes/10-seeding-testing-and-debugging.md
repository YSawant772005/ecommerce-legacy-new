# Byte 10: Seeding, testing, and debugging

**Builds on:** Bytes 1–9

**File(s):** [backend/Ecommerce.Infrastructure/Data/DatabaseSeeder.cs](../backend/Ecommerce.Infrastructure/Data/DatabaseSeeder.cs), [backend/Ecommerce.Tests/TestWebApplicationFactory.cs](../backend/Ecommerce.Tests/TestWebApplicationFactory.cs), [backend/Ecommerce.Tests/AuthTests.cs](../backend/Ecommerce.Tests/AuthTests.cs), [backend/Ecommerce.Tests/CartOrderTests.cs](../backend/Ecommerce.Tests/CartOrderTests.cs), [frontend/tests/e2e/example.spec.ts](../frontend/tests/e2e/example.spec.ts)

**In plain terms:**

The backend seeds development data and uses an in-memory database for integration tests. The frontend currently has a small set of Playwright page-load tests.

**The code:**

```csharp
if (app.Environment.IsEnvironment("Testing"))
{
    db.Database.EnsureCreated();
}
else
{
    db.Database.Migrate();
}

await DatabaseSeeder.SeedAsync(db);
```

```csharp
[Fact]
public async Task Login_WithValidCredentials_ReturnsToken()
{
    var response = await _client.PostAsJsonAsync(
        "/api/auth/login",
        new { email = "admin@novakart.com", password = "Admin123!" });

    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
}
```

**What's happening:**

Normal startup applies migrations and seeds data. Tests use `WebApplicationFactory` and replace SQL Server with EF Core InMemory. Backend tests cover authentication, products, cart behavior, coupons, and pricing. Playwright tests check five basic pages.

**Connection to the project:**

When debugging a feature, first reproduce it with the narrowest relevant backend test, then inspect the frontend Network tab and browser console.

**Why it matters:**

Tests reveal whether a failure is in the UI, HTTP contract, controller, database, or business rules. The current frontend test suite does not yet cover full checkout or token-refresh workflows.

**Remember:**

Use tests and request traces to identify the failing layer instead of guessing from the visible page.

**Check yourself:**

Why does the test factory replace the production database registration?
