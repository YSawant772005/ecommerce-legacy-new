# Legacy Behavior Contract

**Source Repository:** https://github.com/YSawant772005/ecommerce-legacy.git  
**Analysis Date:** October 13, 2026

---

## Overview

This document captures the observable behavior, business rules, formulas, and data assumptions from the legacy NovaKart application. These behaviors must be preserved in the modernized implementation unless explicitly changed by the assignment requirements.

---

## 1. Pricing and Taxation

### 1.1 GST Calculation
**Formula:** `Tax = Subtotal × 0.18`  
**Rounding:** Round to 2 decimal places using standard rounding  
**Example:**
- Subtotal: ₹1000.00
- Tax: ₹180.00
- Total: ₹1180.00

**Legacy Implementation:** `Pricing.Compute` static method

### 1.2 Shipping Fee
**Formula:** 
- If `Subtotal ≥ ₹999.00`: Shipping = ₹0.00
- Else: Shipping = ₹49.00

**Legacy Implementation:** Hardcoded in `Pricing.Compute`

### 1.3 Total Calculation
**Formula:** `Total = Subtotal - Discount + Shipping + Tax`

**Legacy Implementation:** `Pricing.Compute` static method

### 1.4 Discount Calculation (Fixed Amount)
**Formula:** `Discount = Coupon.FixedAmount`

**Constraints:**
- Discount cannot exceed Subtotal
- If `Discount > Subtotal`, set `Discount = Subtotal`

**Legacy Implementation:** `Coupons.Validate` static method

### 1.5 Discount Calculation (Percentage)
**Formula:** 
- `Discount = Subtotal × (Coupon.PercentDiscount / 100)`
- Round to 2 decimal places
- If `Coupon.MaxDiscountAmount > 0` and `Discount > MaxDiscountAmount`, set `Discount = MaxDiscountAmount`
- If `Discount > Subtotal`, set `Discount = Subtotal`

**Example:**
- Subtotal: ₹1000.00
- PercentDiscount: 15%
- MaxDiscountAmount: ₹100.00
- Discount: ₹100.00 (capped)

**Legacy Implementation:** `Coupons.Validate` static method

### 1.6 Free Shipping Coupons
**Behavior:** If `Coupon.FreeShipping = true`, shipping fee is waived regardless of subtotal

**Legacy Implementation:** `Coupons.Validate` sets `CouponResult.FreeShipping`

---

## 2. Coupon Validation Rules

### 2.1 Coupon Code Validation
**Rules:**
- Code must match exactly (case-insensitive, trimmed, uppercase)
- Code must exist in database
- Coupon must be active (`IsActive = true`)

**Error Messages:**
- Empty code: "Enter a coupon code."
- Invalid code: "That coupon code is not valid."

**Legacy Implementation:** `Coupons.Validate`

### 2.2 Date Validation
**Rules:**
- Current date must be ≥ `Coupon.StartsOn`
- If `Coupon.EndsOn` is set, current date must be ≤ `Coupon.EndsOn`

**Error Messages:**
- Not started: "{Code} is not active yet. Check the validity window."
- Expired: "{Code} has expired."

**Legacy Implementation:** `Coupons.Validate`

### 2.3 Minimum Order Value
**Rule:** `Subtotal ≥ Coupon.MinOrderValue`

**Error Message:** "{Code} needs a cart value of ₹{MinOrderValue} or more."

**Legacy Implementation:** `Coupons.Validate`

### 2.4 Coupon Types
**Type 1: Fixed Amount**
- `FixedAmount` has value
- `PercentDiscount` is null
- Discount = `FixedAmount`

**Type 2: Percentage**
- `PercentDiscount` has value
- `FixedAmount` is null
- Discount = percentage of subtotal, capped by `MaxDiscountAmount`

**Type 3: Free Shipping**
- `FreeShipping = true`
- Waives shipping fee
- May also have `FixedAmount` or `PercentDiscount`

**Legacy Implementation:** `Coupon` entity with nullable fields

---

## 3. Cart Behavior

### 3.1 Add to Cart
**Rules:**
- Maximum quantity per item: 10
- If item already exists, increment quantity (max 10)
- If item doesn't exist, add with quantity 1
- Variant (color/size) is stored as string

**Legacy Implementation:** `SessionCart.Add`

### 3.2 Update Quantity
**Rules:**
- If quantity ≤ 0, remove item
- If quantity > 10, cap at 10
- Update item quantity

**Legacy Implementation:** `SessionCart.Update`

### 3.3 Remove from Cart
**Behavior:** Remove item from cart completely

**Legacy Implementation:** `SessionCart.Remove`

### 3.4 Clear Cart
**Behavior:** Remove all items and clear coupon code

**Legacy Implementation:** `SessionCart.Clear`

### 3.5 Cart Merge on Login
**Behavior:**
- Load persisted cart from database
- Merge session cart items into persisted cart
- For duplicate products, sum quantities (max 10)
- Use variant from persisted cart if exists
- Clear session cart after merge

**Legacy Implementation:** `CartStore.MergeCartOnLogin`

### 3.6 Guest Cart
**Behavior:** 
- Stored in session only
- No database persistence
- Lost when session expires
- Merged into persisted cart on login

**Legacy Implementation:** `SessionCart` with session state

### 3.7 Authenticated Cart
**Behavior:**
- Stored in database (`CartItem` table)
- Persists across sessions
- Loaded on login
- Merges with session cart on login

**Legacy Implementation:** `CartStore` with database

---

## 4. Order Lifecycle

### 4.1 Order Status Transitions
**Valid Transitions:**
- Placed → Packed
- Packed → Shipped
- Shipped → Delivered
- Placed → Cancelled
- Packed → Cancelled
- Shipped → Cancelled

**Invalid Transitions:**
- Delivered → any other status (terminal)
- Cancelled → any other status (terminal)

**Legacy Implementation:** `OrderStatus` enum, manual admin updates

### 4.2 Stock Decrement
**Behavior:** When order is placed, decrement stock for each order item:
`Product.Stock -= OrderItem.Quantity`

**Legacy Implementation:** `CheckoutController.Confirm`

### 4.3 Stock Restock
**Behavior:** When order is cancelled, restock stock for each order item:
`Product.Stock += OrderItem.Quantity`

**Legacy Implementation:** Admin status update with cancellation check

### 4.4 Order Number Generation
**Format:** Unique string (not specified in legacy, appears to be auto-generated)

**Legacy Implementation:** Generated in controller, stored in `Order.OrderNumber`

### 4.5 Guest Orders
**Behavior:**
- `Order.UserId` is null
- `Order.Email` is required
- Order is not associated with any user account
- Customer can view order if they have the order number (not implemented in legacy)

**Legacy Implementation:** Nullable `UserId` in `Order` entity

### 4.6 Order Totals Storage
**Behavior:** All monetary values are stored in database at time of order creation:
- Subtotal
- Discount
- Shipping
- Tax
- Total

**Rationale:** Prices may change over time, order should reflect prices at purchase time

**Legacy Implementation:** Stored in `Order` entity

---

## 5. Product Behavior

### 5.1 Discount Percentage Calculation
**Formula:** 
```
if (ComparePrice == null || ComparePrice <= Price || ComparePrice <= 0):
    DiscountPercent = 0
else:
    DiscountPercent = round((ComparePrice - Price) / ComparePrice * 100)
```

**Example:**
- Price: ₹800
- ComparePrice: ₹1000
- DiscountPercent: 20%

**Legacy Implementation:** `Product.DiscountPercent` computed property

### 5.2 Stock Status
**Rule:** `InStock = (Stock > 0)`

**Legacy Implementation:** `Product.InStock` computed property

### 5.3 Color Variants
**Format:** Comma-separated string
**Example:** "Red,Blue,Green"
**Parsing:** Split by comma to get array

**Legacy Implementation:** `Product.Colors` string, `Product.ColorList` computed property

### 5.4 Size Variants
**Format:** Comma-separated string
**Example:** "S,M,L,XL"
**Parsing:** Split by comma to get array

**Legacy Implementation:** `Product.Sizes` string, `Product.SizeList` computed property

### 5.5 Rating Calculation
**Formula:** 
```
NewRating = (OldRating × OldRatingCount + NewRating) / (OldRatingCount + 1)
Round to 1 decimal place
```

**Legacy Implementation:** `ProductsController.ReviewPost`

### 5.6 Verified Purchase Check
**Rule:** Review is verified if user has a delivered order containing the product:
```
OrderItems.Any(i => i.ProductId == ProductId && i.Order.UserId == UserId && i.Order.Status == Delivered)
```

**Legacy Implementation:** `ProductsController.ReviewPost`

---

## 6. Search and Filtering

### 6.1 Search Query
**Behavior:** Search across:
- Product name (case-insensitive contains)
- Product short description (case-insensitive contains)
- Brand name (case-insensitive contains)
- Category name (case-insensitive contains)

**Minimum Length:** 2 characters

**Legacy Implementation:** In-memory LINQ on `CatalogCache.Products`

### 6.2 Brand Filter
**Format:** Comma-separated brand slugs
**Example:** "nike,adidas"
**Behavior:** Products must have brand with slug in the list

**Legacy Implementation:** Split by comma, filter in-memory

### 6.3 Price Filter
**Behavior:**
- Min price: `Product.Price >= Min`
- Max price: `Product.Price <= Max`

**Legacy Implementation:** In-memory LINQ

### 6.4 Rating Filter
**Behavior:** `Product.Rating >= MinRating`

**Legacy Implementation:** In-memory LINQ

### 6.5 Sorting Options
**Popular:** `OrderByDescending(SoldCount)`
**Price Ascending:** `OrderBy(Price)`
**Price Descending:** `OrderByDescending(Price)`
**Rating:** `OrderByDescending(Rating).ThenByDescending(RatingCount)`
**New:** `OrderByDescending(CreatedOn)`
**Discount:** `OrderByDescending(ComparePrice - Price)` (nulls treated as 0)

**Legacy Implementation:** In-memory LINQ

### 6.6 Pagination
**Default Page Size:** 24
**Allowed Page Sizes:** 12, 24, 48
**Behavior:** `Skip((Page - 1) * PageSize).Take(PageSize)`

**Legacy Implementation:** In-memory LINQ

---

## 7. Authentication and Authorization

### 7.1 Password Hashing
**Algorithm:** Custom BCrypt-style implementation (not standard BCrypt)
**Components:**
- Password hash (stored in `User.PasswordHash`)
- Salt (stored in `User.PasswordSalt`)

**Verification:** Hash provided password with stored salt, compare to stored hash

**Legacy Implementation:** `PasswordHasher` static class

### 7.2 Session Management
**User Session Key:** `NK.User`
**Session Data:** `SessionUser` object with Id, Name, Email, Role

**Cart Session Key:** `NK.Cart`
**Session Data:** `List<CartLine>`

**Coupon Session Key:** `NK.Coupon`
**Session Data:** Coupon code string

**Recently Viewed Key:** `NK.Recently`
**Session Data:** `List<int>` of product IDs (max 10)

**Legacy Implementation:** ASP.NET Session state

### 7.3 Role-Based Authorization
**Roles:** "Admin", "Customer" (case-insensitive)
**Admin Check:** `string.Equals(Role, "Admin", StringComparison.OrdinalIgnoreCase)`

**Legacy Implementation:** `Auth.IsAdmin` static method

### 7.4 Login Flow
**Behavior:**
1. Validate email and password
2. Hash password, compare to stored hash
3. If valid, create session with user data
4. Merge session cart into persisted cart
5. Redirect to return URL or home

**Legacy Implementation:** `AccountController.Login`

### 7.5 Logout Flow
**Behavior:**
1. Clear user session
2. Clear cart session (optional, legacy keeps it)
3. Redirect to home

**Legacy Implementation:** `AccountController.Logout`

---

## 8. Address Validation

### 8.1 Phone Validation
**Regex:** `[0-9+\-\s]{8,15}`
**Rules:**
- 8-15 characters
- Only digits, plus, minus, space

**Legacy Implementation:** Data annotation on `CustomerAddress.Phone`

### 8.2 PIN Code Validation
**Regex:** `[0-9]{6}`
**Rules:** Exactly 6 digits

**Legacy Implementation:** Data annotation on `CustomerAddress.PostalCode`

---

## 9. Review Behavior

### 9.1 Review Approval
**Default:** All reviews are auto-approved (`IsApproved = true`)

**Legacy Implementation:** `ProductsController.ReviewPost`

### 9.2 Review Display
**Filter:** Only approved reviews are displayed (`IsApproved = true`)

**Legacy Implementation:** Product detail page query

### 9.3 Review Count
**Behavior:** `Product.ReviewCount` incremented on each review submission

**Legacy Implementation:** `ProductsController.ReviewPost`

---

## 10. Wishlist Behavior

### 10.1 Add to Wishlist
**Behavior:**
- If product already in wishlist, remove it (toggle)
- If product not in wishlist, add it
- Requires login

**Legacy Implementation:** `WishlistController` with toggle logic

### 10.2 Wishlist Persistence
**Behavior:** Stored in database (`WishlistItem` table)

**Legacy Implementation:** Database entity with user and product foreign keys

---

## 11. Seeding Behavior

### 11.1 Deterministic Random
**Seed:** 20261007
**Behavior:** All random generation uses this seed for reproducibility

**Legacy Implementation:** `new Random(20261007)` in `EcommerceSeeder`

### 11.2 Product Count
**Target:** 2,500 products
**Distribution:** Evenly distributed across 12 categories

**Legacy Implementation:** `TargetProductCount = 2500` in `EcommerceSeeder`

### 11.3 Category Count
**Count:** 12 categories
**Fields:** Name, Slug, Tagline, Description, ImageUrl, DisplayOrder, IsActive

**Legacy Implementation:** Hardcoded category definitions in `EcommerceSeeder`

### 11.4 Brand Generation
**Behavior:** Brands are generated from category definitions, unique across categories

**Legacy Implementation:** `SeedBrands` in `EcommerceSeeder`

### 11.5 Product Pricing
**Range:** Category-specific min/max prices
**Distribution:** Random within range

**Legacy Implementation:** Category `Min` and `Max` fields in `EcommerceSeeder`

### 11.6 Stock Levels
**Range:** Random (not specified in legacy, appears to be 0-100)

**Legacy Implementation:** Random generation in `EcommerceSeeder`

### 11.7 Ratings
**Range:** 1.0 to 5.0
**Precision:** 1 decimal place
**Distribution:** Random

**Legacy Implementation:** Random generation in `EcommerceSeeder`

### 11.8 Default Admin User
**Email:** admin@novakart.com
**Password:** Admin123!
**Role:** Admin

**Legacy Implementation:** Hardcoded in `EcommerceSeeder.SeedUsers`

### 11.9 Default Coupons
**Coupons:**
- SAVE10: 10% discount, max ₹100
- FLAT50: ₹50 fixed discount
- FREESHIP: Free shipping

**Legacy Implementation:** Hardcoded in `EcommerceSeeder` (not visible in extracted code, mentioned in knowledge bytes)

---

## 12. Catalog Cache Behavior

### 12.1 Cache Scope
**Scope:** Application-level (shared across all users)
**Storage:** HttpRuntime.Cache

**Legacy Implementation:** `CatalogCache` static class

### 12.2 Cached Data
- All products (with Category and Brand navigation)
- All categories
- All brands
- Price floor (minimum product price)
- Price ceiling (maximum product price)

**Legacy Implementation:** `CatalogCache.RefreshProducts` and `CatalogCache.RefreshCategories`

### 12.3 Cache Refresh
**Triggers:**
- Product created/updated/deleted via admin
- Category created/updated/deleted via admin

**Legacy Implementation:** Called in admin controller actions

### 12.4 Cache Invalidation
**Strategy:** Full cache refresh (not incremental)

**Legacy Implementation:** `CatalogCache.RefreshProducts` reloads all products

---

## 13. URL Routing

### 13.1 Storefront Routes
- `/` - Home page
- `/c/{slug}` - Category page
- `/products` - Product listing with search/filters
- `/product/{slug}` - Product detail page
- `/cart` - Cart page
- `/checkout` - Checkout flow
- `/checkout/success` - Order confirmation
- `/wishlist` - Wishlist page
- `/contact` - Contact form

### 13.2 Account Routes
- `/account/register` - Registration
- `/account/login` - Login
- `/account/logout` - Logout
- `/account/profile` - Profile management
- `/account/change-password` - Password change
- `/account/forgot-password` - Forgot password
- `/account/reset-password` - Password reset
- `/account/addresses` - Address management
- `/account/orders` - Order history
- `/account/orders/{id}` - Order details

### 13.3 Admin Routes
- `/admin` - Dashboard
- `/admin/products` - Product management
- `/admin/categories` - Category management
- `/admin/brands` - Brand management
- `/admin/orders` - Order management
- `/admin/customers` - Customer management
- `/admin/coupons` - Coupon management
- `/admin/messages` - Contact messages
- `/admin/reviews` - Review moderation

### 13.4 API Routes
- `/sitemap.xml` - Sitemap
- `/robots.txt` - Robots.txt

**Legacy Implementation:** `RouteConfig` in MVC

---

## 14. Error Handling

### 14.1 Validation Errors
**Display:** ModelState errors displayed in view
**Format:** Field-level error messages

**Legacy Implementation:** ASP.NET MVC validation

### 14.2 404 Not Found
**Behavior:** Return 404 status, display error view
**Triggers:** Product not found, category not found

**Legacy Implementation:** `HttpNotFound()` in controllers

### 14.3 401 Unauthorized (AJAX)
**Behavior:** Return 401 status with JSON response `{ ok: false, error: "login_required" }`

**Legacy Implementation:** `RequireLogin` attribute with AJAX detection

### 14.4 401 Unauthorized (Non-AJAX)
**Behavior:** Redirect to login page with return URL

**Legacy Implementation:** `RequireLogin` attribute with redirect

---

## 15. Data Assumptions

### 15.1 Currency
**Currency:** Indian Rupee (₹)
**Locale:** en-IN
**Symbol:** ₹

**Legacy Implementation:** Hardcoded in views and pricing

### 15.2 Date Format
**Format:** Not explicitly specified, appears to use server culture

**Legacy Implementation:** DateTime defaults in .NET

### 15.3 Image URLs
**Source:** External (Unsplash)
**Format:** Full URLs
**Storage:** String in database

**Legacy Implementation:** `Product.ImageUrl`, `Category.ImageUrl`

### 15.4 Email Format
**Validation:** Standard email validation
**Storage:** Lowercase (not enforced in legacy)

**Legacy Implementation:** EmailAddress data annotation

### 15.5 Password Requirements
**Minimum Length:** Not specified (appears to be 6+ characters based on seed data)
**Complexity:** Not specified
**Validation:** Only required field validation

**Legacy Implementation:** Required data annotation only

---

## 16. SEO Behavior

### 16.1 Meta Descriptions
**Priority:**
1. Product short description
2. Category description
3. Default description

**Legacy Implementation:** ViewBag.MetaDescription in controllers

### 16.2 Canonical URLs
**Product Pages:** `/product/{slug}`
**Category Pages:** `/c/{slug}`

**Legacy Implementation:** ViewBag.Canonical in controllers

### 16.3 Sitemap
**Included:**
- All products
- All categories
- Priority: Not specified (appears to be 0.8 for products, 0.9 for categories)
- LastMod: Product/category CreatedOn

**Legacy Implementation:** `SitemapController.Index`

### 16.4 Robots.txt
**Content:**
```
User-agent: *
Allow: /
Sitemap: /sitemap.xml
```

**Legacy Implementation:** Static file

---

## 17. Performance Assumptions

### 17.1 Catalog Cache Rationale
**Reason:** Avoid querying database for 2,500 products on every request
**Trade-off:** Cache must be manually refreshed on changes

**Legacy Implementation:** In-memory cache with manual refresh

### 17.2 In-Memory Filtering
**Reason:** Simpler implementation, acceptable for 2,500 products
**Trade-off:** Doesn't scale to larger catalogs

**Legacy Implementation:** LINQ on cached list

### 17.3 Session Cart Rationale
**Reason:** Fast cart operations for guests
**Trade-off:** Lost on session expiry

**Legacy Implementation:** Session state for guests, database for authenticated

---

## 18. Security Assumptions

### 18.1 Password Storage
**Method:** Custom hash with salt (not industry standard)
**Risk:** Weaker than BCrypt/Argon2
**Migration Action:** Re-hash on first login with BCrypt

### 18.2 Session Security
**Method:** In-proc session
**Risk:** Not scalable, lost on server restart
**Migration Action:** Replace with JWT tokens

### 18.3 CSRF Protection
**Method:** AntiForgeryToken on POST
**Status:** Implemented in legacy

**Migration Action:** Implement CSRF protection for API (or rely on same-site cookies)

### 18.4 SQL Injection
**Method:** EF6 parameterized queries
**Status:** Protected by ORM

**Migration Action:** Continue using EF Core parameterized queries

### 18.5 XSS Protection
**Method:** Razor encoding (automatic)
**Status:** Protected by framework

**Migration Action:** Vue template encoding (automatic)

---

## 19. Intentional Simplifications (Legacy)

### 19.1 Mock Payment
**Behavior:** No real payment gateway integration
**Reason:** Learning/teaching codebase

**Migration Action:** Keep mock payment

### 19.2 Mock Email
**Behavior:** Password reset token displayed on screen, not emailed
**Reason:** Learning/teaching codebase

**Migration Action:** Keep mock email (or document as limitation)

### 19.3 In-Memory Catalog
**Behavior:** Catalog cached in memory, not database queries
**Reason:** Performance for 2,500 products

**Migration Action:** Consider EF Core query caching or keep in-memory cache

### 19.4 No Order Status History
**Behavior:** Only current status stored, no history
**Reason:** Simplification

**Migration Action:** Keep current status only (or add history as enhancement)

### 19.5 No Product Image Upload
**Behavior:** Image URLs only, no file upload
**Reason:** Simplification

**Migration Action:** Keep URL-only approach (or add upload as enhancement)

---

## 20. Migration Constraints

### 20.1 Must Preserve
- All pricing formulas (GST, shipping, discounts)
- All coupon validation rules
- All stock management logic
- All order lifecycle rules
- All validation rules (phone, PIN code, etc.)
- All business rules (max 10 quantity, etc.)
- All data assumptions (currency, locale, etc.)

### 20.2 Must Change (Assignment Requirement)
- Session-based authentication → JWT-based authentication
- Session-based cart → API-based cart with database persistence
- MVC controllers → Minimal API endpoints
- Razor views → Vue components
- EF6 → EF Core 10
- .NET Framework 4.7 → .NET 10

### 20.3 May Change (With Justification)
- In-memory catalog cache → EF Core query caching (justify performance)
- Custom password hash → BCrypt (justify security)
- Mock email → Real email (justify if implemented)
- 2,500 products → 25 products minimum (justify if reduced)

### 20.4 Should Not Change
- Pricing constants (18% GST, ₹49 shipping, ₹999 threshold)
- Coupon validation logic
- Stock decrement/restock logic
- Order status transitions
- Validation rules and error messages
- URL structure (unless Vue router requires change)

---

## 21. Regression Test Requirements

### 21.1 Pricing Tests
- [ ] GST calculation: ₹1000 → ₹180 tax
- [ ] Shipping fee: ₹500 → ₹49, ₹1000 → ₹0
- [ ] Fixed discount: ₹100 coupon on ₹500 subtotal → ₹100 discount
- [ ] Percentage discount: 15% on ₹1000 → ₹150 discount
- [ ] Percentage cap: 15% on ₹1000 with ₹100 cap → ₹100 discount
- [ ] Free shipping coupon: ₹500 subtotal → ₹0 shipping
- [ ] Total calculation: Subtotal ₹1000, discount ₹100, shipping ₹49, tax ₹180 → ₹1129 total

### 21.2 Cart Tests
- [ ] Add to cart: quantity capped at 10
- [ ] Update quantity: 0 or negative removes item
- [ ] Remove item: item removed from cart
- [ ] Cart merge: session + persisted quantities summed (max 10)
- [ ] Clear cart: all items removed, coupon cleared

### 21.3 Order Tests
- [ ] Stock decrement: order placement reduces stock
- [ ] Stock restock: cancellation restores stock
- [ ] Guest order: UserId is null
- [ ] Order totals: stored at creation time

### 21.4 Coupon Tests
- [ ] Invalid code: error message
- [ ] Expired coupon: error message
- [ ] Not started coupon: error message
- [ ] Below minimum: error message
- [ ] Valid coupon: applied successfully

### 21.5 Product Tests
- [ ] Discount percent: ₹800/₹1000 → 20%
- [ ] Stock status: Stock > 0 → InStock = true
- [ ] Rating calculation: new review updates average
- [ ] Verified purchase: delivered order only

---

## 22. Open Questions

### 22.1 Order Number Format
**Question:** What is the format for order numbers?
**Legacy Behavior:** Not visible in extracted code
**Decision Needed:** Define format (e.g., NK-20261013-0001)

### 22.2 Catalog Cache Strategy
**Question:** Should we keep in-memory cache or use EF Core query caching?
**Legacy Behavior:** In-memory cache with manual refresh
**Decision Needed:** Evaluate performance, decide on strategy

### 22.3 Password Migration
**Question:** How to migrate custom password hashes to BCrypt?
**Legacy Behavior:** Custom hash algorithm
**Decision Needed:** Re-hash on first login or bulk migration

### 22.4 Product Count
**Question:** Should we seed all 2,500 products or minimum 25?
**Legacy Behavior:** 2,500 products
**Assignment Requirement:** Minimum 25
**Decision Needed:** Decide based on performance and demo needs

### 22.5 Recently Viewed Persistence
**Question:** Should recently viewed be server-side or client-side?
**Legacy Behavior:** Server-side session
**Migration Option:** Client-side localStorage
**Decision Needed:** Choose approach, justify

---

## 23. Success Criteria

The migration will be considered behaviorally complete when:

1. **All pricing formulas produce identical results** to legacy
2. **All coupon validation rules match legacy** exactly
3. **All stock management logic matches legacy** exactly
4. **All order lifecycle rules match legacy** exactly
5. **All validation rules match legacy** exactly
6. **All business rules match legacy** exactly
7. **All regression tests pass**
8. **Any intentional changes are documented** with justification
