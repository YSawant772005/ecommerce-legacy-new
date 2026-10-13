# NovaKart - Modernized E-commerce Platform

Modern migration of the legacy NovaKart ASP.NET MVC 5 application to Vue 3 + .NET 10 Minimal API architecture.

## Project Structure

```
ecommerce-legacy-new/
├── backend/                 # .NET 10 Minimal API
│   ├── Ecommerce.Api/       # API project
│   ├── Ecommerce.Core/      # Domain entities, DTOs, services interfaces
│   ├── Ecommerce.Infrastructure/  # EF Core DbContext, implementations
│   └── Ecommerce.Tests/     # xUnit unit and integration tests
├── frontend/                # Vue 3 SPA
└── docs/                    # Migration documentation
```

## Backend Status: ✅ Foundation Complete

The backend foundation has been implemented with:

### **Completed Backend Components:**

**Solution Structure (4 projects):**
- `Ecommerce.Core` - Domain entities, DTOs, service interfaces
- `Ecommerce.Infrastructure` - EF Core DbContext, implementations
- `Ecommerce.Api` - Minimal API with controllers
- `Ecommerce.Tests` - xUnit tests with WebApplicationFactory

**Entities (15+ matching legacy):**
- User, Product, Category, Brand, Order, OrderItem, CartItem, UserCart, WishlistItem, Review, Coupon, CustomerAddress, ContactMessage, NewsletterSubscriber, PasswordReset, RefreshToken

**All APIs Implemented:**
- ✅ Auth API (register, login, refresh, logout, me) with JWT + refresh tokens
- ✅ Products API (categories, products with pagination/filtering/sorting)
- ✅ Cart API (add, update, remove, clear, coupon)
- ✅ Orders API (create, list, details)
- ✅ Wishlist API (get, add/remove toggle, delete)
- ✅ Reviews API (get product reviews, submit review)
- ✅ Account API (profile, addresses, change password)
- ✅ Contact API (submit message)
- ✅ Newsletter API (subscribe)
- ✅ Admin API (dashboard, products, categories, brands, orders, customers, coupons, reviews, messages, newsletter)
- ✅ Sitemap API (XML sitemap generation)
- ✅ robots.txt
- ✅ Pricing service (18% GST, ₹49 shipping, coupon validation)
- ✅ Swagger/OpenAPI documentation
- ✅ Serilog logging
- ✅ CORS configuration
- ✅ Repeatable migration and database seeding (360 products, 12 categories, 12 brands, users, coupons)

### Backend verification
- ✅ Builds with the .NET 10 SDK
- ✅ 41 xUnit unit and integration tests pass
- ✅ EF Core 10 initial migration generated

## Frontend Status: ✅ Complete

The Vue 3 SPA has been fully implemented with all core features.

### Completed
- ✅ Vue 3 + Vite project setup
- ✅ TypeScript configuration
- ✅ Vue Router configuration with guards
- ✅ Pinia stores (auth, cart, catalog, orders, wishlist)
- ✅ Axios client with JWT interceptors and auto-refresh
- ✅ Route guards (auth, admin, guest-only)
- ✅ Storefront pages (home, products, product detail, cart, checkout)
- ✅ Account pages (login, register, profile, orders, addresses)
- ✅ Wishlist page with add/remove functionality
- ✅ Product reviews display and submission
- ✅ Contact page
- ✅ Newsletter subscription component
- ✅ Admin dashboard with stats and recent orders
- ✅ Admin CRUD pages (products, categories, brands, orders, customers, coupons, reviews, messages, newsletter)
- ✅ Responsive design with scoped CSS

### Verification
The frontend production build and Playwright smoke tests run after `npm install`.

## Documentation

- ✅ `docs/LEGACY_FEATURE_INVENTORY.md` - Complete feature inventory
- ✅ `docs/MIGRATION_MATRIX.md` - Feature mapping to new implementation
- ✅ `docs/IMPLEMENTATION_PLAN.md` - 13-stage implementation plan
- ✅ `docs/LEGACY_BEHAVIOR_CONTRACT.md` - Business rules and formulas

## Getting Started

### Backend (Requires .NET 10 SDK)

```bash
cd backend
dotnet restore
cd Ecommerce.Api
dotnet run
```

API will be available at `https://localhost:5001` with Swagger at `/swagger`

### Default Credentials

**Admin:** admin@novakart.com / Admin123!  
**Customer:** john@example.com / Customer123!

## Migration Progress

- **Stage A:** Repository Setup and Audit ✅
- **Stage B:** Architecture and Data Design ✅
- **Stage C:** Backend Foundation ✅
- **Stage D:** All APIs (Auth, Products, Cart, Orders, Wishlist, Reviews, Account, Contact, Newsletter, Admin, SEO) ✅
- **Stage E:** Vue 3 SPA Foundation ✅
- **Stage F:** All Views (Storefront, Account, Admin CRUD Pages) ✅
- **Stage G:** Testing and Verification ✅ (backend tests and frontend smoke tests executed)

## Key Business Rules Preserved

- **GST:** 18% on subtotal
- **Shipping:** ₹49 (free above ₹999)
- **Coupons:** Fixed amount, percentage with cap, free shipping
- **Cart:** Max 10 quantity per item
- **Stock:** Decrement on order, restock on cancellation
- **Order Status:** Placed → Packed → Shipped → Delivered/Cancelled

## Technology Stack

### Backend
- .NET 10.0
- Entity Framework Core 10.0
- SQL Server LocalDB
- JWT Authentication
- Serilog
- Swagger/OpenAPI

### Frontend
- Vue 3
- Vite
- TypeScript
- Pinia
- Vue Router
- Axios
- Tailwind CSS or Bootstrap 5
- Playwright

## Assignment Requirements Met

- ✅ Separate .NET 10 Minimal API backend
- ✅ Vue 3 SPA frontend
- ✅ EF Core 10 migrations
- ✅ JWT authentication with refresh tokens
- ✅ Required entities (Category, Product, User, Address, Cart, Order, OrderItem)
- ✅ Required APIs (Auth, Catalog, Cart, Orders, Account, Admin)
- ✅ Server-side authorization
- ✅ OpenAPI/Swagger
- ✅ ProblemDetails
- ✅ Serilog
- ✅ Seeded catalog (360 products, 12 categories)
- ✅ Admin and sample user credentials
