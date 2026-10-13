# Migration Matrix

**Source Repository:** https://github.com/YSawant772005/ecommerce-legacy.git  
**Destination Repository:** https://github.com/YSawant772005/ecommerce-legacy-new.git  
**Analysis Date:** October 13, 2026

---

## Overview

This matrix maps every legacy feature to its new implementation approach, tracking migration status and verification.

**Legend:**
- 🔴 Not Started
- 🟡 In Progress
- 🟢 Implemented
- 🔵 Tested
- ⚪ Blocked
- ➖ Intentionally Omitted (with justification)

---

## 1. Authentication & Authorization

| Feature | Legacy Implementation | New Implementation | Status | Verification |
|---------|---------------------|-------------------|--------|---------------|
| User Registration | `AccountController.Register` with custom password hashing | `POST /api/auth/register` with BCrypt/ASP.NET Identity | 🔴 Not Started | Unit test for password hashing, integration test for registration |
| User Login | `AccountController.Login` with session-based auth | `POST /api/auth/login` with JWT access token + refresh token | 🔴 Not Started | Integration test for login, JWT validation |
| User Logout | `AccountController.Logout` clearing session | `POST /api/auth/logout` with refresh token revocation | 🔴 Not Started | Integration test for logout, token invalidation |
| Token Refresh | Not applicable (session-based) | `POST /api/auth/refresh` with token rotation | 🔴 Not Started | Integration test for refresh flow, expiry handling |
| Current User | `Auth.Current` reading from session | `GET /api/auth/me` returning user profile | 🔴 Not Started | Integration test for profile endpoint |
| Role-Based Auth | `RequireAdmin` action filter checking session role | Server-side JWT role claims + endpoint authorization | 🔴 Not Started | Integration test for admin-only endpoints |
| Password Change | `AccountController.ChangePassword` with re-hash | `PUT /api/account/password` with hash update | 🔴 Not Started | Integration test for password change |
| Password Reset | `AccountController.ForgotPassword` with token display | `POST /api/auth/forgot-password` with token (mock email) | 🔴 Not Started | Integration test for reset flow |
| Session Guard | `RequireLogin` action filter | Vue route guards + 401 API response handling | 🔴 Not Started | E2E test for protected routes |

---

## 2. Product Catalog

| Feature | Legacy Implementation | New Implementation | Status | Verification |
|---------|---------------------|-------------------|--------|---------------|
| Home Page | `HomeController.Index` with featured/deals/new | Vue home page with API calls to catalog endpoints | 🔴 Not Started | E2E test for home page rendering |
| Category Pages | `ProductsController.Index` with category filter | `GET /api/products?categoryId={id}` with Vue catalog view | 🔴 Not Started | E2E test for category browsing |
| Product Search | In-memory LINQ on `CatalogCache.Products` | `GET /api/products?search={query}` with EF Core query | 🔴 Not Started | Integration test for search endpoint |
| Search Suggestions | `ProductsController.Suggest` returning top 6 | `GET /api/products/suggest?q={query}` endpoint | 🔴 Not Started | Integration test for suggestions |
| Product Details | `ProductsController.Details` with related products | `GET /api/products/{id}` with full product data | 🔴 Not Started | E2E test for product detail page |
| Product Filtering | In-memory LINQ filters (brand, price, rating) | EF Core `Where` clauses with query parameters | 🔴 Not Started | Integration test for filter combinations |
| Product Sorting | In-memory LINQ OrderBy/OrderByDescending | EF Core OrderBy with query parameter | 🔴 Not Started | Integration test for all sort options |
| Pagination | Skip/Take on in-memory list | EF Core Skip/Take with pagination metadata | 🔴 Not Started | Integration test for pagination |
| Infinite Scroll | AJAX `More` endpoint returning HTML partial | `GET /api/products?page={n}` returning JSON | 🔴 Not Started | E2E test for infinite scroll |
| Related Products | Query same category, exclude current, top 8 | API endpoint or client-side filtering | 🔴 Not Started | E2E test for related products display |
| Recently Viewed | Session `NK.Recently` with List<int> | LocalStorage + API endpoint for persistence | 🔴 Not Started | E2E test for recently viewed persistence |
| Catalog Cache | Static `CatalogCache` with HttpRuntime.Cache | EF Core query caching or Redis (optional) | 🔴 Not Started | Performance test for catalog queries |

---

## 3. Cart & Pricing

| Feature | Legacy Implementation | New Implementation | Status | Verification |
|---------|---------------------|-------------------|--------|---------------|
| Session Cart | `SessionCart` with List<CartLine> in session | Pinia cart store with API persistence | 🔴 Not Started | E2E test for cart operations |
| Persisted Cart | `CartItem` entity with `CartStore` | `GET /api/cart` and `POST /api/cart/items` endpoints | 🔴 Not Started | Integration test for cart CRUD |
| Cart Merge | `CartStore.MergeCartOnLogin` on login | `POST /api/cart/merge` endpoint | 🔴 Not Started | Integration test for cart merge logic |
| Add to Cart | `SessionCart.Add` with max 10 qty | `POST /api/cart/items` with validation | 🔴 Not Started | E2E test for add to cart |
| Update Quantity | `SessionCart.Update` with max 10 | `PUT /api/cart/items/{productId}` | 🔴 Not Started | E2E test for quantity update |
| Remove Item | `SessionCart.Remove` | `DELETE /api/cart/items/{productId}` | 🔴 Not Started | E2E test for remove from cart |
| Clear Cart | `SessionCart.Clear` | `DELETE /api/cart` | 🔴 Not Started | Integration test for cart clear |
| Pricing Engine | Static `Pricing.Compute` with hardcoded constants | Server-side pricing service with same constants | 🔴 Not Started | Unit test for GST calculation, shipping, totals |
| GST Calculation | 18% hardcoded | 18% in pricing service | 🔴 Not Started | Unit test for GST rounding |
| Shipping Fee | ₹49 hardcoded, free above ₹999 | Same logic in pricing service | 🔴 Not Started | Unit test for shipping threshold |
| Discount Calculation | `Coupons.Validate` with fixed/percentage | Same logic in coupon service | 🔴 Not Started | Unit test for discount calculations |
| Coupon Validation | Database lookup with date/active checks | Same validation in coupon service | 🔴 Not Started | Unit test for coupon validation rules |
| Coupon Storage | Session `NK.Coupon` | API endpoint with user cart association | 🔴 Not Started | Integration test for coupon application |
| Mini-Cart | Partial view rendered on every page | Vue component with Pinia store | 🔴 Not Started | E2E test for mini-cart display |

---

## 4. Checkout & Orders

| Feature | Legacy Implementation | New Implementation | Status | Verification |
|---------|---------------------|-------------------|--------|---------------|
| Checkout Wizard | Session-based wizard with 4 steps | Vue multi-step form with API validation | 🔴 Not Started | E2E test for complete checkout flow |
| Address Entry | Form with data annotations | Vue form with FluentValidation on API | 🔴 Not Started | E2E test for address form validation |
| Address Selection | Load saved addresses from database | `GET /api/account/addresses` endpoint | 🔴 Not Started | E2E test for address selection |
| Payment Selection | Form with UPI/Card/COD options | Vue form with mock payment | 🔴 Not Started | E2E test for payment selection |
| Order Review | Read-only display of wizard state | Vue component with API order preview | 🔴 Not Started | E2E test for order review step |
| Order Creation | `CheckoutController.Confirm` with transaction | `POST /api/orders` with EF Core transaction | 🔴 Not Started | Integration test for order creation |
| Stock Decrement | Product.Stock -= quantity on placement | Same logic in order service | 🔴 Not Started | Unit test for stock decrement |
| Order Number | Generated in controller | Generated in order service | 🔴 Not Started | Unit test for order number uniqueness |
| Guest Orders | UserId nullable in Order | Same nullable foreign key | 🔴 Not Started | Integration test for guest checkout |
| Order Confirmation | Success page with TempData | Vue success page with order ID | 🔴 Not Started | E2E test for order confirmation |
| Order Status | Enum: Placed, Packed, Shipped, Delivered, Cancelled | Same enum in domain model | 🔴 Not Started | Unit test for status transitions |
| Stock Restock | On cancellation, Product.Stock += quantity | Same logic in status update service | 🔴 Not Started | Unit test for stock restock |

---

## 5. Customer Account

| Feature | Legacy Implementation | New Implementation | Status | Verification |
|---------|---------------------|-------------------|--------|---------------|
| Profile View | `AccountController.Profile` | `GET /api/account/profile` | 🔴 Not Started | Integration test for profile endpoint |
| Profile Edit | Form with name/phone update | `PUT /api/account/profile` | 🔴 Not Started | E2E test for profile update |
| Address List | `AccountController.Addresses` | `GET /api/account/addresses` | 🔴 Not Started | E2E test for address list |
| Add Address | Form with validation | `POST /api/account/addresses` | 🔴 Not Started | E2E test for add address |
| Edit Address | Form with validation | `PUT /api/account/addresses/{id}` | 🔴 Not Started | E2E test for edit address |
| Delete Address | Delete action | `DELETE /api/account/addresses/{id}` | 🔴 Not Started | E2E test for delete address |
| Default Address | IsDefault flag | Same flag with API update | 🔴 Not Started | E2E test for default address |
| Order History | `AccountController.Orders` | `GET /api/orders` (filtered by user) | 🔴 Not Started | E2E test for order history |
| Order Details | `AccountController.OrderDetails` | `GET /api/orders/{id}` with ownership check | 🔴 Not Started | E2E test for order details |
| Printable Invoice | Print CSS on order details | Same print CSS in Vue component | 🔴 Not Started | Manual verification of print layout |

---

## 6. Wishlist

| Feature | Legacy Implementation | New Implementation | Status | Verification |
|---------|---------------------|-------------------|--------|---------------|
| Wishlist Page | `WishlistController.Index` | Vue page with `GET /api/wishlist` | 🔴 Not Started | E2E test for wishlist page |
| Add to Wishlist | AJAX endpoint with toggle | `POST /api/wishlist/items` | 🔴 Not Started | E2E test for add to wishlist |
| Remove from Wishlist | Delete action | `DELETE /api/wishlist/items/{productId}` | 🔴 Not Started | E2E test for remove from wishlist |
| Move to Wishlist | Cart remove + wishlist add | Combined API call or client-side | 🔴 Not Started | E2E test for move to wishlist |
| Wishlist Persistence | `WishlistItem` entity | Same entity with API endpoints | 🔴 Not Started | Integration test for wishlist CRUD |

---

## 7. Reviews

| Feature | Legacy Implementation | New Implementation | Status | Verification |
|---------|---------------------|-------------------|--------|---------------|
| Review Display | Product detail tab with approved filter | Vue component with API data | 🔴 Not Started | E2E test for review display |
| Review Submission | `ProductsController.ReviewPost` | `POST /api/products/{id}/reviews` | 🔴 Not Started | E2E test for review submission |
| Verified Purchase | Check OrderItems for delivered orders | Same logic in review service | 🔴 Not Started | Unit test for verified purchase check |
| Auto-Approval | All reviews set IsApproved=true | Same auto-approval (or configurable) | 🔴 Not Started | Unit test for approval logic |
| Rating Recalculation | Product.Rating updated on submission | Same calculation in review service | 🔴 Not Started | Unit test for rating aggregation |
| Review Moderation | Admin approve/hide/delete | Admin API endpoints with authorization | 🔴 Not Started | Integration test for moderation |

---

## 8. Admin Dashboard

| Feature | Legacy Implementation | New Implementation | Status | Verification |
|---------|---------------------|-------------------|--------|---------------|
| Dashboard Metrics | Aggregated LINQ queries | `GET /api/admin/dashboard` with aggregation | 🔴 Not Started | Integration test for dashboard metrics |
| Revenue Calculation | Sum(Order.Total) | Same aggregation in service | 🔴 Not Started | Unit test for revenue calculation |
| Monthly Revenue | Group by month | Same grouping in service | 🔴 Not Started | Unit test for monthly revenue |
| Low Stock Alert | Products.Where(p => p.Stock < 10) | Same query in service | 🔴 Not Started | Unit test for low stock query |
| Orders by Status | Group by Order.Status | Same grouping in service | 🔴 Not Started | Unit test for status grouping |
| Product List | `AdminController.Products` | `GET /api/admin/products` with pagination | 🔴 Not Started | E2E test for product list |
| Product Create | Form with slug/SKU generation | `POST /api/admin/products` | 🔴 Not Started | E2E test for product creation |
| Product Edit | Form with validation | `PUT /api/admin/products/{id}` | 🔴 Not Started | E2E test for product edit |
| Product Delete | Delete action | `DELETE /api/admin/products/{id}` | 🔴 Not Started | E2E test for product deletion |
| Category List | `AdminController.Categories` | `GET /api/admin/categories` | 🔴 Not Started | E2E test for category list |
| Category CRUD | Form with cascade delete | CRUD endpoints with cascade | 🔴 Not Started | E2E test for category CRUD |
| Brand List | `AdminController.Brands` | `GET /api/admin/brands` | 🔴 Not Started | E2E test for brand list |
| Brand CRUD | Form operations | CRUD endpoints | 🔴 Not Started | E2E test for brand CRUD |
| Order List | `AdminController.Orders` with search | `GET /api/admin/orders` with filters | 🔴 Not Started | E2E test for order list |
| Order Details | Detail view | `GET /api/admin/orders/{id}` | 🔴 Not Started | E2E test for order details |
| Order Status Update | Status change with stock restock | `PUT /api/admin/orders/{id}/status` | 🔴 Not Started | Integration test for status update |
| Customer List | `AdminController.Customers` with spend metrics | `GET /api/admin/customers` | 🔴 Not Started | E2E test for customer list |
| Coupon List | `AdminController.Coupons` | `GET /api/admin/coupons` | 🔴 Not Started | E2E test for coupon list |
| Coupon CRUD | Form operations | CRUD endpoints | 🔴 Not Started | E2E test for coupon CRUD |
| Message List | `AdminController.Messages` | `GET /api/admin/messages` | 🔴 Not Started | E2E test for message list |
| Message Delete | Delete action | `DELETE /api/admin/messages/{id}` | 🔴 Not Started | E2E test for message deletion |
| Review List | `AdminController.Reviews` | `GET /api/admin/reviews` | 🔴 Not Started | E2E test for review list |
| Review Moderation | Approve/hide/delete actions | Moderation endpoints | 🔴 Not Started | E2E test for review moderation |

---

## 9. Contact & Newsletter

| Feature | Legacy Implementation | New Implementation | Status | Verification |
|---------|---------------------|-------------------|--------|---------------|
| Contact Form | Form with database storage | Vue form with `POST /api/contact` | 🔴 Not Started | E2E test for contact form |
| Contact Messages | `ContactMessage` entity | Same entity with API | 🔴 Not Started | Integration test for message persistence |
| Newsletter Subscribe | Footer form with database | Vue form with `POST /api/newsletter/subscribe` | 🔴 Not Started | E2E test for newsletter subscription |
| Duplicate Prevention | Email uniqueness check | Same check in API | 🔴 Not Started | Unit test for duplicate prevention |
| Newsletter List | Admin view | `GET /api/admin/newsletter` | 🔴 Not Started | E2E test for newsletter admin |
| Unsubscribe | Not implemented | ➖ Omitted (out of scope for MVP) | ➖ Omitted | N/A |

---

## 10. SEO & Static Pages

| Feature | Legacy Implementation | New Implementation | Status | Verification |
|---------|---------------------|-------------------|--------|---------------|
| Robots.txt | Static file | Static file (copy from legacy) | 🔴 Not Started | Manual verification |
| Sitemap.xml | Dynamic XML from controller | API endpoint or static generation | 🔴 Not Started | Manual verification of XML format |
| Canonical URLs | ViewBag.Canonical in layout | Vue router meta or head management | 🔴 Not Started | Manual verification |
| Meta Descriptions | ViewBag.MetaDescription | Vue meta management | 🔴 Not Started | Manual verification |
| JSON-LD Structured Data | Script tag in product view | Vue component with JSON-LD | 🔴 Not Started | Manual verification of schema |
| Static Pages | Not implemented (except contact) | ➖ Omitted (no static pages in legacy) | ➖ Omitted | N/A |

---

## 11. Database Schema

| Entity | Legacy Schema | New Schema | Status | Verification |
|--------|--------------|-----------|--------|---------------|
| Users | User table with custom password hash | User table with BCrypt/Identity hash | 🔴 Not Started | EF Core migration test |
| Products | Product with 18,2 precision | Same precision in EF Core | 🔴 Not Started | Migration creates table |
| Categories | Category with DisplayOrder | Same structure | 🔴 Not Started | Migration creates table |
| Brands | Brand with Code | Same structure | 🔴 Not Started | Migration creates table |
| Orders | Order with 18,2 precision fields | Same precision | 🔴 Not Started | Migration creates table |
| OrderItems | OrderItem with 18,2 precision | Same precision | 🔴 Not Started | Migration creates table |
| CartItems | CartItem with Variant (120 char) | Same structure | 🔴 Not Started | Migration creates table |
| UserCarts | UserCart with CouponCode | Same structure | 🔴 Not Started | Migration creates table |
| WishlistItems | WishlistItem with AddedOn | Same structure | 🔴 Not Started | Migration creates table |
| Reviews | Review with IsApproved flag | Same structure | 🔴 Not Started | Migration creates table |
| Coupons | Coupon with 18,2 precision fields | Same precision | 🔴 Not Started | Migration creates table |
| CustomerAddresses | CustomerAddress with validation | Same structure | 🔴 Not Started | Migration creates table |
| ContactMessages | ContactMessage | Same structure | 🔴 Not Started | Migration creates table |
| NewsletterSubscribers | NewsletterSubscriber | Same structure | 🔴 Not Started | Migration creates table |
| PasswordResets | PasswordReset with token | Same structure | 🔴 Not Started | Migration creates table |
| RefreshTokens | Not applicable | New table for JWT refresh tokens | 🔴 Not Started | Migration creates table |

---

## 12. Infrastructure

| Component | Legacy Implementation | New Implementation | Status | Verification |
|-----------|---------------------|-------------------|--------|---------------|
| Database | SQL Server LocalDB | SQL Server Express or LocalDB | 🔴 Not Started | Connection string test |
| ORM | Entity Framework 6 | Entity Framework Core 10 | 🔴 Not Started | EF Core query test |
| Dependency Injection | None (manual instantiation) | Built-in .NET DI container | 🔴 Not Started | Service resolution test |
| Logging | None | Serilog with file/console sinks | 🔴 Not Started | Log output verification |
| API Documentation | None | Swagger UI / OpenAPI | 🔴 Not Started | Swagger UI accessible |
| Validation | Data annotations + ModelState | FluentValidation + ProblemDetails | 🔴 Not Started | Validation error test |
| Error Handling | Try/catch with error views | ProblemDetails middleware | 🔴 Not Started | Error response test |
| CORS | Not configured | Restrictive CORS policy | 🔴 Not Started | CORS header verification |
| Health Checks | Not implemented | `/health` endpoint | 🔴 Not Started | Health check test |
| Session State | In-proc session | JWT tokens (no server session) | 🔴 Not Started | N/A (architectural change) |
| Static Files | Content folder served by IIS | Vue static files served by API | 🔴 Not Started | Static file serving test |

---

## 13. Frontend Architecture

| Component | Legacy Implementation | New Implementation | Status | Verification |
|-----------|---------------------|-------------------|--------|---------------|
| Framework | ASP.NET MVC 5 (server-rendered) | Vue 3 SPA (client-rendered) | 🔴 Not Started | Vue app renders |
| Build Tool | MSBuild | Vite | 🔴 Not Started | Vite dev server starts |
| Language | C# (Razor) | TypeScript | 🔴 Not Started | TypeScript compiles |
| State Management | Session + ViewBag | Pinia stores | 🔴 Not Started | Pinia store test |
| Routing | ASP.NET Routing | Vue Router | 🔴 Not Started | E2E route navigation |
| Client-Side Validation | jQuery Validation (not used) | Vue form validation | 🔴 Not Started | Form validation test |
| HTTP Client | jQuery AJAX (not used, vanilla XHR) | Axios with interceptors | 🔴 Not Started | API call test |
| CSS Framework | Custom CSS (no framework) | Tailwind CSS or Bootstrap 5 | 🔴 Not Started | Styles render correctly |
| Component Library | None (custom Razor partials) | Custom Vue components | 🔴 Not Started | Component renders |
| Testing | None | Vitest + Vue Test Utils | 🔴 Not Started | Unit test passes |
| E2E Testing | None | Playwright or Cypress | 🔴 Not Started | E2E test passes |

---

## 14. Seeding & Data Migration

| Data | Legacy Seeder | New Seeder | Status | Verification |
|------|--------------|-----------|--------|---------------|
| Categories | 12 categories with deterministic data | Same 12 categories | 🔴 Not Started | Seeded categories match |
| Brands | Multiple brands per category | Same brands | 🔴 Not Started | Seeded brands match |
| Products | 2,500 products with random seed | Same 2,500 products (or scaled) | 🔴 Not Started | Product count >= 25 (assignment min) |
| Product Images | External URLs (Unsplash) | Same external URLs | 🔴 Not Started | Images load |
| Reviews | Random reviews per product | Same review distribution | 🔴 Not Started | Reviews seeded |
| Admin User | admin@novakart.com / Admin123! | Same credentials with BCrypt | 🔴 Not Started | Admin login works |
| Sample Customer | Customer with orders | Same customer | 🔴 Not Started | Customer login works |
| Sample Orders | Random orders with items | Same order distribution | 🔴 Not Started | Orders seeded |
| Coupons | Default coupons (SAVE10, FLAT50, etc.) | Same coupons | 🔴 Not Started | Coupons work |
| Password Resets | Not seeded | Not seeded | ➖ Omitted | N/A |

---

## 15. Security Migration

| Security Aspect | Legacy Implementation | New Implementation | Status | Verification |
|----------------|---------------------|-------------------|--------|---------------|
| Password Hashing | Custom BCrypt-style algorithm | ASP.NET Core Identity or BCrypt | 🔴 Not Started | Hash verification test |
| Session Management | In-proc session | JWT access tokens (short-lived) | 🔴 Not Started | Token expiry test |
| Refresh Tokens | Not applicable | Secure refresh tokens with rotation | 🔴 Not Started | Refresh flow test |
| Token Storage | Session | HttpOnly cookie for refresh, memory for access | 🔴 Not Started | Cookie security test |
| CSRF Protection | AntiForgeryToken | Same or CSRF cookie | 🔴 Not Started | CSRF test |
| XSS Prevention | Razor encoding (automatic) | Vue template encoding (automatic) | 🔴 Not Started | XSS test |
| SQL Injection | EF6 parameterized queries | EF Core parameterized queries | 🔴 Not Started | N/A (ORM protection) |
| Authorization | Role-based in controllers | Role-based in API endpoints | 🔴 Not Started | Authorization test |
| Resource Ownership | Manual checks in controllers | Manual checks in services | 🔴 Not Started | Ownership test |
| HTTPS | Not configured (dev) | Configured for production | 🔴 Not Started | HTTPS redirect test |
| Rate Limiting | Not implemented | Rate limiting on auth endpoints | 🔴 Not Started | Rate limit test |
| Secret Management | Web.config (not in repo) | User secrets / environment variables | 🔴 Not Started | Secret not in repo |

---

## 16. Intentional Omissions

| Feature | Reason for Omission | Impact |
|---------|-------------------|--------|
| Newsletter Unsubscribe | Not implemented in legacy | Low priority, can be added later |
| Email Sending | Mock in legacy (no real email) | Same mock approach acceptable |
| Real Payment Gateway | Mock in legacy | Same mock approach acceptable |
| Order Status History | Not tracked in legacy (only current state) | Can be added as enhancement |
| Product Image Upload | URL-only in legacy | Same URL-only approach |
| Export Functions | Not implemented in legacy | Can be added as enhancement |
| Remember Me | Not implemented in legacy | Can be added as enhancement |
| Advanced Search Filters | Basic filters only in legacy | Same basic filters preserved |
| Customer Groups/Segments | Not implemented in legacy | Can be added as enhancement |
| Multi-currency | INR-only in legacy | Same INR-only approach |

---

## Migration Progress Summary

- **Total Features:** 150+
- **Not Started:** 150+
- **In Progress:** 0
- **Implemented:** 0
- **Tested:** 0
- **Blocked:** 0
- **Intentionally Omitted:** 8

**Completion Percentage:** 0%

---

## Critical Path Features

These features must be implemented first to enable other features:

1. **Authentication** - Required for all protected features
2. **Product Catalog** - Required for storefront
3. **Cart API** - Required for checkout
4. **Order Creation** - Required for order management
5. **Admin Auth** - Required for all admin features

---

## Risk Areas

| Risk | Mitigation |
|------|------------|
| Session-to-JWT migration complexity | Implement robust refresh token flow with rotation |
| In-memory catalog cache removal | Use EF Core query caching or add Redis if needed |
| 2,500 product seeding time | Optimize seeder or reduce to minimum 25 for development |
| Custom password hash migration | Re-hash all passwords on first login with new algorithm |
| Vue SPA SEO | Implement SSR or pre-rendering if SEO critical (out of scope for MVP) |
| State synchronization (session vs API) | Design clear state ownership (client vs server) |
| Image hosting dependency | Document external image dependency, consider local hosting |

---

## Next Steps

1. **Stage A:** Complete repository setup and audit (current)
2. **Stage B:** Design domain model and API contracts
3. **Stage C:** Implement backend foundation (EF Core, migrations, seeding)
4. **Stage D:** Implement authentication and catalog APIs
5. **Stage E:** Implement cart, checkout, and order APIs
6. **Stage F:** Implement admin APIs
7. **Stage G:** Implement Vue SPA foundation
8. **Stage H:** Implement storefront views
9. **Stage I:** Implement admin views
10. **Stage J:** Testing and verification
