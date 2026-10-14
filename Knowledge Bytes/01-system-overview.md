# Byte 1: System overview

**Builds on:** None — starting point

**File(s):** [README.md](../README.md), [frontend/src/main.ts](../frontend/src/main.ts), [backend/Ecommerce.Api/Program.cs](../backend/Ecommerce.Api/Program.cs)

**In plain terms:**

NovaKart is a Vue storefront that communicates with a .NET 10 API. The API stores catalog, account, cart, order, coupon, review, and administrative data through Entity Framework Core.

**The code:**

```ts
const app = createApp(App)
app.use(createPinia())
app.use(router)
app.mount('#app')
```

```csharp
builder.Services.AddControllers();
...
app.MapControllers();
app.Run();
```

**What's happening:**

The browser loads the Vue application. Vue renders pages and sends `/api` requests. ASP.NET Core receives those requests, controllers execute application behavior, and EF Core communicates with SQL Server LocalDB.

**Connection to the project:**

The frontend entry point is `frontend/src/main.ts`. The backend composition root is `backend/Ecommerce.Api/Program.cs`.

**Why it matters:**

These are the two starting points for almost every investigation. If either application fails to start, all higher-level features fail.

**Remember:**

The frontend owns the user interface; the backend owns trusted business operations and persistence.

**Check yourself:**

Which file would you inspect first if the browser shows a blank application?
