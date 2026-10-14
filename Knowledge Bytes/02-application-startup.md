# Byte 2: Application startup and configuration

**Builds on:** Byte 1

**File(s):** [backend/Ecommerce.Api/Program.cs](../backend/Ecommerce.Api/Program.cs), [backend/Ecommerce.Api/appsettings.json](../backend/Ecommerce.Api/appsettings.json), [frontend/vite.config.ts](../frontend/vite.config.ts)

**In plain terms:**

Startup code registers dependencies, configures middleware, connects the database, and defines how frontend development requests reach the API.

**The code:**

```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sql => sql.CommandTimeout(180)));

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options => { ... });
```

```ts
server: {
  host: '0.0.0.0',
  port: 5173,
  proxy: {
    '/api': {
      target: 'http://localhost:5000',
      changeOrigin: true,
    },
  },
}
```

**What's happening:**

ASP.NET Core reads the SQL Server connection string and JWT settings, then registers controllers, EF Core, authentication, CORS, Swagger, validation, and logging. Vite serves the Vue app on port 5173 and proxies `/api` calls to port 5000.

**Connection to the project:**

Controllers receive services registered in `Program.cs`. Frontend API modules use the proxy without hardcoding the backend host.

**Why it matters:**

Wrong ports, origins, connection strings, or JWT settings can make an otherwise correct feature appear broken.

**Remember:**

`Program.cs` wires the backend; `vite.config.ts` wires local frontend-to-backend communication.

**Check yourself:**

What target receives a frontend request beginning with `/api` during development?
