# Legacy Feature Inventory

**Source Repository:** https://github.com/YSawant772005/ecommerce-legacy.git  
**Analysis Date:** October 13, 2026  
**Purpose:** Complete inventory of all features in the legacy NovaKart application to ensure preservation during migration.

---

## Executive Summary

The legacy NovaKart application is a monolithic ASP.NET MVC 5 e-commerce platform with approximately 2,500 products across 12 categories. It uses Entity Framework 6, SQL Server LocalDB, session-based authentication, and in-memory catalog caching. The application includes a full storefront, customer account management, and comprehensive admin dashboard.

**Total Features Identified:** 87  
**Status Legend:** 
- 🔴 Not Started
- 🟡 Partially Implemented  
- 🟢 Implemented
- 🔵 Tested
- ⚪ Blocked

---

## 1. Storefront & Product Catalog

### 1.1 Home Page
- **Status:** 🔴 Not Started
- **Entry Point:** `HomeController.Index` → `/`
- **Features:**
  - Category display with images
  - Featured products section
  - Deals section
  - New arrivals section
  - Recently viewed products
- **Dependencies:** `CatalogCache`, `StoreContext.Categories`, `StoreContext.Products`
- **Legacy Implementation:** Razor view with partials, session-based recently viewed tracking

### 1.2 Category Landing Pages
- **Status:** 🔴 Not Started
- **Entry Point:** `ProductsController.Index` → `/c/{slug}`
- **Features:**
  - Category-specific product listing
  - Category metadata (tagline, description, image)
  - SEO meta tags
  - Canonical URLs
- **Dependencies:** `StoreContext.Categories`, `CatalogCache`
- **Legacy Implementation:** Filtered catalog view with category filter applied

### 1.3 Product Search
- **Status:** 🔴 Not Started
- **Entry Point:** `ProductsController.Index` → `/products?q={query}`
- **Features:**
  - Full-text search across product name, description, brand, category
  - Search suggestions endpoint (`/products/suggest`)
  - Minimum 2-character requirement
  - Top 6 suggestion results
- **Dependencies:** `CatalogCache.Products` (in-memory search)
- **Legacy Implementation:** In-memory LINQ queries on cached product list

### 1.4 Product Listing with Filters
- **Status:** 🔴 Not Started
- **Entry Point:** `ProductsController.Index` with query parameters
- **Features:**
  - Pagination (12, 24, or 48 items per page)
  - Infinite scroll mode (`mode=scroll`)
  - Classic pagination mode (`mode=pages`)
  - Sorting options: popular, price-asc, price-desc, rating, new, discount
  - Brand filter (multi-select)
  - Price range filter (min/max)
  - Rating filter (minimum rating)
  - Category filter
  - AJAX "load more" endpoint (`/products/more`)
- **Dependencies:** `CatalogCache`, session state for recently viewed
- **Legacy Implementation:** In-memory filtering with LINQ, session-based recently viewed

### 1.5 Product Detail Pages
- **Status:** 🔴 Not Started
- **Entry Point:** `ProductsController.Details` → `/product/{slug}`
- **Features:**
  - Image gallery (up to 3 images)
  - Product specifications
  - Color variant selection
  - Size variant selection
  - Price with discount percentage calculation
  - Stock status (InStock property)
  - Related products (same category, max 8)
  - Product reviews (approved only, max 12)
  - Review submission (requires login)
  - Verified purchase badge
  - Recently viewed tracking
  - SEO meta tags and canonical URL
  - Tabbed interface (description/specs/reviews)
- **Dependencies:** `StoreContext.Products`, `StoreContext.Reviews`, session state
- **Legacy Implementation:** Razor view with AJAX review submission, session-based recently viewed

### 1.6 Recently Viewed Products
- **Status:** 🔴 Not Started
- **Entry Point:** Session key `NK.Recently`
- **Features:**
  - Stores up to 10 product IDs
  - Persists across session
  - Displayed on home and product pages
  - Maintains chronological order
- **Dependencies:** Session state
- **Legacy Implementation:** `List<int>` in session, loaded from database on each request

---

## 2. Cart, Coupons & Pricing

### 2.1 Session-Based Cart
- **Status:** 🔴 Not Started
- **Entry Point:** `SessionCart` class, session key `NK.Cart`
- **Features:**
  - Add products with quantity (max 10 per item)
  - Update quantity
  - Remove items
  - Clear cart
  - Variant storage (color/size)
  - Cart count badge
- **Dependencies:** Session state
- **Legacy Implementation:** `List<CartLine>` in session, no database persistence for guests

### 2.2 Persisted Cart (Authenticated Users)
- **Status:** 🔴 Not Started
- **Entry Point:** `CartStore` class, `CartItem` entity
- **Features:**
  - Database-backed cart for logged-in users
  - Automatic merge on login (session cart → persisted cart)
  - Cart persistence across sessions
  - Coupon code storage per user
- **Dependencies:** `StoreContext.CartItems`, `StoreContext.UserCarts`
- **Legacy Implementation:** `CartStore.MergeCartOnLogin` in `AccountController.Login`

### 2.3 Cart Page
- **Status:** 🔴 Not Started
- **Entry Point:** `CartController.Index` → `/cart`
- **Features:**
  - Display cart items with images
  - Quantity controls (AJAX)
  - Remove item (AJAX)
  - Move to wishlist
  - Coupon code input
  - Real-time totals calculation
  - Subtotal, discount, shipping, tax, total display
  - Continue shopping button
  - Proceed to checkout button
- **Dependencies:** `SessionCart`, `Coupons`, `Pricing`
- **Legacy Implementation:** Razor view with AJAX endpoints for updates

### 2.4 Coupon System
- **Status:** 🔴 Not Started
- **Entry Point:** `Coupons.Validate` static method
- **Features:**
  - Fixed amount discounts
  - Percentage discounts with maximum cap
  - Free shipping coupons
  - Minimum order value requirement
  - Start date validation
  - End date validation
  - Active/inactive status
  - Usage tracking (TotalUses counter)
  - Coupon code storage in cart
- **Dependencies:** `StoreContext.Coupons`
- **Legacy Implementation:** Static validation class with database lookup

### 2.5 Pricing Engine
- **Status:** 🔴 Not Started
- **Entry Point:** `Pricing.Compute` static method (in ViewModels)
- **Features:**
  - 18% GST calculation
  - ₹49 shipping fee
  - Free shipping above ₹999 threshold
  - Discount application (coupons)
  - Subtotal calculation
  - Tax rounding to 2 decimal places
  - Total calculation
- **Dependencies:** None (pure calculation)
- **Legacy Implementation:** Static method with hardcoded constants

### 2.6 Mini-Cart
- **Status:** 🔴 Not Started
- **Entry Point:** `_MiniCart` partial view
- **Features:**
  - Cart item count
  - Quick view of cart contents
  - AJAX updates on add/remove
  - Link to full cart page
- **Dependencies:** `SessionCart`
- **Legacy Implementation:** Partial rendered on every page via layout

---

## 3. Checkout & Orders

### 3.1 Multi-Step Checkout Wizard
- **Status:** 🔴 Not Started
- **Entry Point:** `CheckoutController` → `/checkout`
- **Features:**
  - Step 1: Address entry
  - Step 2: Payment method selection
  - Step 3: Review order
  - Step 4: Order confirmation
  - Wizard state in session
  - Navigation between steps
  - Validation at each step
- **Dependencies:** Session state, `StoreContext.CustomerAddresses`
- **Legacy Implementation:** Session-based wizard with action filters

### 3.2 Address Management (Checkout)
- **Status:** 🔴 Not Started
- **Entry Point:** Checkout step 1
- **Features:**
  - Address form (full name, phone, address line, city, state, postal code, country)
  - Phone validation (8-15 characters, digits/plus/minus/space)
  - PIN code validation (6 digits)
  - Save address option
  - Load saved addresses
  - Set default address
- **Dependencies:** `StoreContext.CustomerAddresses`
- **Legacy Implementation:** Form validation with data annotations

### 3.3 Payment Simulation
- **Status:** 🔴 Not Started
- **Entry Point:** Checkout step 2
- **Features:**
  - Payment method selection (UPI, Card, COD)
  - Mock payment processing
  - Payment reference generation
  - No real payment gateway integration
- **Dependencies:** None (mock)
- **Legacy Implementation:** Form selection with mock success

### 3.4 Order Review
- **Status:** 🔴 Not Started
- **Entry Point:** Checkout step 3
- **Features:**
  - Order summary
  - Itemized list with images
  - Shipping address display
  - Payment method display
  - Coupon applied display
  - Final totals
  - Place order button
- **Dependencies:** Session wizard state, `Pricing`
- **Legacy Implementation:** Read-only display of wizard data

### 3.5 Order Creation
- **Status:** 🔴 Not Started
- **Entry Point:** `CheckoutController.Confirm` POST
- **Features:**
  - Order number generation (unique)
  - Stock decrement
  - Order persistence
  - Order items creation
  - User association (if logged in)
  - Guest order support
  - Cart clear after placement
  - Session wizard clear
- **Dependencies:** `StoreContext.Orders`, `StoreContext.OrderItems`, `StoreContext.Products`
- **Legacy Implementation:** Transactional database write with stock update

### 3.6 Order Confirmation
- **Status:** 🔴 Not Started
- **Entry Point:** `CheckoutController.Success` → `/checkout/success`
- **Features:**
  - Order number display
  - Order summary
  - Continue shopping button
  - View order details link
- **Dependencies:** Session (order ID passed via TempData)
- **Legacy Implementation:** Simple success page with TempData

### 3.7 Order Lifecycle
- **Status:** 🔴 Not Started
- **Entry Point:** Order status enum and admin updates
- **Features:**
  - Status transitions: Placed → Packed → Shipped → Delivered
  - Cancellation support (Cancelled status)
  - Stock decrement on placement
  - Stock restock on cancellation
  - Status history not tracked (only current state)
- **Dependencies:** `StoreContext.Orders`, `StoreContext.Products`
- **Legacy Implementation:** Enum-based status with manual admin updates

---

## 4. Customer Account

### 4.1 Registration
- **Status:** 🔴 Not Started
- **Entry Point:** `AccountController.Register` GET/POST → `/account/register`
- **Features:**
  - Full name, email, password fields
  - Email validation
  - Password hashing (custom BCrypt implementation)
  - Password salt generation
  - Role assignment (Customer by default)
  - Duplicate email prevention
  - Auto-login after registration
- **Dependencies:** `PasswordHasher`, `StoreContext.Users`
- **Legacy Implementation:** Custom password hashing, session-based auth

### 4.2 Login
- **Status:** 🔴 Not Started
- **Entry Point:** `AccountController.Login` GET/POST → `/account/login`
- **Features:**
  - Email/password authentication
  - Password hash verification
  - Session creation
  - Cart merge on login
  - Remember me (not implemented)
  - Return URL support
  - Admin role detection
- **Dependencies:** `Auth`, `PasswordHasher`, `CartStore`, session state
- **Legacy Implementation:** Session-based authentication with custom auth filter

### 4.3 Logout
- **Status:** 🔴 Not Started
- **Entry Point:** `AccountController.Logout` → `/account/logout`
- **Features:**
  - Session clear
  - Auth clear
  - Redirect to home
- **Dependencies:** `Auth`, session state
- **Legacy Implementation:** Session abandonment

### 4.4 Profile Management
- **Status:** 🔴 Not Started
- **Entry Point:** `AccountController.Profile` GET/POST → `/account/profile`
- **Features:**
  - Edit full name
  - Edit phone
  - Email not editable
  - Password change (separate flow)
- **Dependencies:** `StoreContext.Users`
- **Legacy Implementation:** Simple form with model binding

### 4.5 Password Change
- **Status:** 🔴 Not Started
- **Entry Point:** `AccountController.ChangePassword` GET/POST → `/account/change-password`
- **Features:**
  - Current password verification
  - New password validation
  - Confirm password matching
  - Password re-hashing
  - Salt regeneration
- **Dependencies:** `PasswordHasher`, `StoreContext.Users`
- **Legacy Implementation:** Password verification and re-hash

### 4.6 Password Reset
- **Status:** 🔴 Not Started
- **Entry Point:** `AccountController.ForgotPassword` → `/account/forgot-password`
- **Features:**
  - Email input
  - Token generation
  - Token storage in database
  - Token display on screen (no email)
  - Reset link with token
  - Token expiration (not implemented)
  - Password reset form
- **Dependencies:** `StoreContext.PasswordResets`, `StoreContext.Users`
- **Legacy Implementation:** Token displayed on screen (mock email)

### 4.7 Address Management
- **Status:** 🔴 Not Started
- **Entry Point:** `AccountController.Addresses` → `/account/addresses`
- **Features:**
  - List saved addresses
  - Add new address
  - Edit address
  - Delete address
  - Set default address
  - Address validation
- **Dependencies:** `StoreContext.CustomerAddresses`
- **Legacy Implementation:** CRUD operations with validation

### 4.8 Order History
- **Status:** 🔴 Not Started
- **Entry Point:** `AccountController.Orders` → `/account/orders`
- **Features:**
  - List user's orders
  - Order number, date, status, total display
  - Pagination
  - Filter by status (not implemented)
- **Dependencies:** `StoreContext.Orders`
- **Legacy Implementation:** Simple list view

### 4.9 Order Details
- **Status:** 🔴 Not Started
- **Entry Point:** `AccountController.OrderDetails` → `/account/orders/{id}`
- **Features:**
  - Order information display
  - Order items with images
  - Shipping address
  - Payment method
  - Order status
  - Order totals
  - Printable invoice
- **Dependencies:** `StoreContext.Orders`, `StoreContext.OrderItems`
- **Legacy Implementation:** Detail view with print CSS

### 4.10 Printable Invoice
- **Status:** 🔴 Not Started
- **Entry Point:** Order details page with print stylesheet
- **Features:**
  - Invoice layout
  - Company branding
  - Order details
  - Tax breakdown
  - Print-friendly CSS
- **Dependencies:** None (CSS)
- **Legacy Implementation:** Print media query in CSS

---

## 5. Wishlist

### 5.1 Wishlist Page
- **Status:** 🔴 Not Started
- **Entry Point:** `WishlistController.Index` → `/wishlist`
- **Features:**
  - List wishlist items
  - Product display with images
  - Add to cart from wishlist
  - Remove from wishlist
  - Empty wishlist message
- **Dependencies:** `StoreContext.WishlistItems`, `WishlistStore`
- **Legacy Implementation:** Database-backed wishlist with session fallback

### 5.2 Add to Wishlist
- **Status:** 🔴 Not Started
- **Entry Point:** AJAX endpoint in `WishlistController`
- **Features:**
  - Add product to wishlist
  - Remove if already present (toggle)
  - AJAX response
  - Login requirement
- **Dependencies:** `StoreContext.WishlistItems`
- **Legacy Implementation:** AJAX POST with auth check

### 5.3 Move to Wishlist from Cart
- **Status:** 🔴 Not Started
- **Entry Point:** Cart page
- **Features:**
  - Remove from cart
  - Add to wishlist
  - AJAX operation
- **Dependencies:** `SessionCart`, `WishlistStore`
- **Legacy Implementation:** Combined cart remove + wishlist add

---

## 6. Reviews

### 6.1 Review Display
- **Status:** 🔴 Not Started
- **Entry Point:** Product detail page reviews tab
- **Features:**
  - Approved reviews only
  - Rating display (stars)
  - Review title and body
  - Author name
  - Verified purchase badge
  - Date display
  - Pagination (not implemented, max 12)
- **Dependencies:** `StoreContext.Reviews`
- **Legacy Implementation:** Simple list with IsApproved filter

### 6.2 Review Submission
- **Status:** 🔴 Not Started
- **Entry Point:** `ProductsController.ReviewPost` POST
- **Features:**
  - Rating selection (1-5 stars)
  - Title and body input
  - Login requirement
  - Verified purchase detection
  - Auto-approval (all reviews approved)
  - Product rating recalculation
  - Review count increment
- **Dependencies:** `StoreContext.Reviews`, `StoreContext.Products`, `StoreContext.OrderItems`
- **Legacy Implementation:** Form with verified purchase check

### 6.3 Review Moderation
- **Status:** 🔴 Not Started
- **Entry Point:** Admin panel
- **Features:**
  - Approve reviews
  - Hide reviews (soft delete via IsApproved)
  - Delete reviews (hard delete)
  - View all reviews
- **Dependencies:** `StoreContext.Reviews`
- **Legacy Implementation:** Admin CRUD operations

---

## 7. Contact & Newsletter

### 7.1 Contact Form
- **Status:** 🔴 Not Started
- **Entry Point:** Contact page
- **Features:**
  - Name, email, subject, message fields
  - Form validation
  - Message persistence
  - Admin inbox view
  - No email sending (mock)
- **Dependencies:** `StoreContext.ContactMessages`
- **Legacy Implementation:** Form with database storage

### 7.2 Newsletter Subscription
- **Status:** 🔴 Not Started
- **Entry Point:** Footer subscription form
- **Features:**
  - Email input
  - Subscription persistence
  - Duplicate prevention
  - Unsubscribe link (not implemented)
  - Admin management
- **Dependencies:** `StoreContext.NewsletterSubscribers`
- **Legacy Implementation:** Simple form with database storage

### 7.3 Newsletter Management
- **Status:** 🔴 Not Started
- **Entry Point:** Admin panel
- **Features:**
  - View subscribers
  - Delete subscribers
  - Export list (not implemented)
  - Send newsletter (not implemented)
- **Dependencies:** `StoreContext.NewsletterSubscribers`
- **Legacy Implementation:** Admin list view

---

## 8. Administration

### 8.1 Admin Authentication
- **Status:** 🔴 Not Started
- **Entry Point:** `RequireAdmin` attribute
- **Features:**
  - Role-based access control
  - Admin role check
  - Redirect non-admins to home
  - Redirect unauthenticated to login
- **Dependencies:** `Auth`, session state
- **Legacy Implementation:** Action filter attribute

### 8.2 Dashboard
- **Status:** 🔴 Not Started
- **Entry Point:** `AdminController.Index` → `/admin`
- **Features:**
  - Total revenue
  - Monthly revenue
  - Total orders
  - Total customers
  - Total products
  - Low stock alerts (products with stock < 10)
  - Orders by status summary
  - Recent orders list
- **Dependencies:** `StoreContext.Orders`, `StoreContext.Products`, `StoreContext.Users`
- **Legacy Implementation:** Aggregated queries with LINQ

### 8.3 Product Management
- **Status:** 🔴 Not Started
- **Entry Point:** `AdminController.Products` → `/admin/products`
- **Features:**
  - Product list with search/filter
  - Create product
  - Edit product
  - Delete product
  - Slug auto-generation
  - SKU auto-generation
  - Catalog cache refresh after changes
  - Image upload (not implemented, URL only)
- **Dependencies:** `StoreContext.Products`, `CatalogCache`
- **Legacy Implementation:** CRUD with cache refresh

### 8.4 Category Management
- **Status:** 🔴 Not Started
- **Entry Point:** `AdminController.Categories` → `/admin/categories`
- **Features:**
  - Category list
  - Create category
  - Edit category
  - Delete category (cascade to products)
  - Slug auto-generation
  - Display order
  - Active/inactive toggle
- **Dependencies:** `StoreContext.Categories`, `CatalogCache`
- **Legacy Implementation:** CRUD with cascade delete

### 8.5 Brand Management
- **Status:** 🔴 Not Started
- **Entry Point:** `AdminController.Brands` → `/admin/brands`
- **Features:**
  - Brand list
  - Create brand
  - Edit brand
  - Delete brand (no cascade on products)
  - Slug auto-generation
  - Code auto-generation
- **Dependencies:** `StoreContext.Brands`, `CatalogCache`
- **Legacy Implementation:** CRUD with cache refresh

### 8.6 Order Management
- **Status:** 🔴 Not Started
- **Entry Point:** `AdminController.Orders` → `/admin/orders`
- **Features:**
  - Order list with search/filter
  - Order details view
  - Status update (Placed → Packed → Shipped → Delivered/Cancelled)
  - Stock restock on cancellation
  - Order number search
  - Email search
  - Status filter
- **Dependencies:** `StoreContext.Orders`, `StoreContext.Products`
- **Legacy Implementation:** List view with status update action

### 8.7 Customer Management
- **Status:** 🔴 Not Started
- **Entry Point:** `AdminController.Customers` → `/admin/customers`
- **Features:**
  - Customer list
  - Customer details
  - Total spend calculation
  - Order count
  - Email search
  - Name search
- **Dependencies:** `StoreContext.Users`, `StoreContext.Orders`
- **Legacy Implementation:** List view with aggregated metrics

### 8.8 Coupon Management
- **Status:** 🔴 Not Started
- **Entry Point:** `AdminController.Coupons` → `/admin/coupons`
- **Features:**
  - Coupon list
  - Create coupon
  - Edit coupon
  - Delete coupon
  - Activate/deactivate
  - Usage tracking display
- **Dependencies:** `StoreContext.Coupons`
- **Legacy Implementation:** CRUD with validation

### 8.9 Contact Message Management
- **Status:** 🔴 Not Started
- **Entry Point:** `AdminController.Messages` → `/admin/messages`
- **Features:**
  - Message list
  - Message details
  - Delete message
  - Mark as read (not implemented)
- **Dependencies:** `StoreContext.ContactMessages`
- **Legacy Implementation:** Simple list and detail view

### 8.10 Review Moderation
- **Status:** 🔴 Not Started
- **Entry Point:** `AdminController.Reviews` → `/admin/reviews`
- **Features:**
  - Review list
  - Approve review
  - Hide review (set IsApproved = false)
  - Delete review
  - Filter by product
- **Dependencies:** `StoreContext.Reviews`
- **Legacy Implementation:** List with approve/hide/delete actions

---

## 9. SEO & Static Pages

### 9.1 Robots.txt
- **Status:** 🔴 Not Started
- **Entry Point:** `/robots.txt`
- **Features:**
  - Allow all bots
  - Sitemap reference
- **Dependencies:** None
- **Legacy Implementation:** Static file

### 9.2 Sitemap.xml
- **Status:** 🔴 Not Started
- **Entry Point:** `SitemapController.Index` → `/sitemap.xml`
- **Features:**
  - Dynamic sitemap generation
  - All products
  - All categories
  - Priority and lastmod
  - XML format
- **Dependencies:** `StoreContext.Products`, `StoreContext.Categories`
- **Legacy Implementation:** XML result from controller

### 9.3 Canonical URLs
- **Status:** 🔴 Not Started
- **Entry Point:** Product detail pages
- **Features:**
  - Canonical link tag
  - Prevent duplicate content issues
- **Dependencies:** None (HTML)
- **Legacy Implementation:** ViewBag.Canonical in layout

### 9.4 Meta Tags
- **Status:** 🔴 Not Started
- **Entry Point:** All pages
- **Features:**
  - Meta description
  - Open Graph tags (not implemented)
  - Twitter cards (not implemented)
- **Dependencies:** None (HTML)
- **Legacy Implementation:** ViewBag.MetaDescription in layout

### 9.5 JSON-LD Structured Data
- **Status:** 🔴 Not Started
- **Entry Point:** Product detail pages
- **Features:**
  - Product schema markup
  - Price, availability, rating
- **Dependencies:** None (HTML)
- **Legacy Implementation:** Script tag in product view

---

## 10. Infrastructure & Utilities

### 10.1 Catalog Cache
- **Status:** 🔴 Not Started
- **Entry Point:** `CatalogCache` static class
- **Features:**
  - In-memory product list
  - In-memory category list
  - In-memory brand list
  - Price floor/ceiling calculation
  - Cache refresh on product/category changes
  - Application-level singleton
- **Dependencies:** `StoreContext`, HTTP cache
- **Legacy Implementation:** Static class with HttpRuntime.Cache

### 10.2 Password Hashing
- **Status:** 🔴 Not Started
- **Entry Point:** `PasswordHasher` static class
- **Features:**
  - BCrypt-style hashing
  - Salt generation
  - Hash verification
  - Custom implementation (not BCrypt library)
- **Dependencies:** None
- **Legacy Implementation:** Custom hash algorithm

### 10.3 Session Management
- **Status:** 🔴 Not Started
- **Entry Point:** ASP.NET Session
- **Features:**
  - User authentication state
  - Cart state
  - Coupon state
  - Recently viewed products
  - Checkout wizard state
- **Dependencies:** ASP.NET Session
- **Legacy Implementation:** In-proc session state

### 10.4 Database Initialization
- **Status:** 🔴 Not Started
- **Entry Point:** `EcommerceInitializer`
- **Features:**
  - Database creation if not exists
  - Automatic migration (not implemented, uses DropCreateDatabaseIfModelChanges)
  - Seeder execution
  - Index creation
- **Dependencies:** EF6 Database Initializer
- **Legacy Implementation:** DropCreateDatabaseIfModelChanges strategy

### 10.5 Data Seeding
- **Status:** 🔴 Not Started
- **Entry Point:** `EcommerceSeeder`
- **Features:**
  - 12 categories
  - 2,500 products
  - Multiple brands per category
  - Product variants (colors, sizes)
  - Random pricing
  - Random stock levels
  - Random ratings
  - Sample reviews
  - Admin user (admin@novakart.com / Admin123!)
  - Sample customer
  - Sample orders
  - Default coupons
- **Dependencies:** `StoreContext`
- **Legacy Implementation:** Deterministic random with fixed seed (20261007)

---

## 11. Frontend Assets

### 11.1 CSS
- **Status:** 🔴 Not Started
- **Entry Point:** `Content/site.css`, `Content/admin.css`
- **Features:**
  - Responsive layout
  - Grid system
  - Product cards
  - Forms styling
  - Admin panel styling
  - Print styles
- **Dependencies:** None (custom CSS, no framework)
- **Legacy Implementation:** Custom CSS without Bootstrap or Tailwind

### 11.2 JavaScript
- **Status:** 🔴 Not Started
- **Entry Point:** `Scripts/site.js`, `Scripts/catalog.js`, `Scripts/cart.js`, `Scripts/pdp.js`, `Scripts/checkout.js`
- **Features:**
  - AJAX cart operations
  - Infinite scroll
  - Search suggestions
  - Variant selection
  - Form validation
  - AJAX review submission
- **Dependencies:** jQuery (not included, vanilla JS)
- **Legacy Implementation:** Vanilla JavaScript with XMLHttpRequest

### 11.3 Images
- **Status:** 🔴 Not Started
- **Entry Point:** `Content/images/`
- **Features:**
  - Category images
  - Product images
  - Generated via PowerShell script
  - External URLs (Unsplash)
- **Dependencies:** External image hosting
- **Legacy Implementation:** Static URLs to external images

---

## 12. Security

### 12.1 Authentication
- **Status:** 🔴 Not Started
- **Entry Point:** `Auth` class, session-based
- **Features:**
  - Session-based authentication
  - Role-based authorization
  - Login/logout
  - Auth filters
- **Dependencies:** Session state
- **Legacy Implementation:** Custom session-based auth (no ASP.NET Identity)

### 12.2 Authorization
- **Status:** 🔴 Not Started
- **Entry Point:** `RequireLogin`, `RequireAdmin` attributes
- **Features:**
  - Action-level authorization
  - AJAX-aware auth checks
  - Redirect for non-AJAX
  - 401 JSON for AJAX
- **Dependencies:** `Auth`
- **Legacy Implementation:** Custom action filters

### 12.3 Input Validation
- **Status:** 🔴 Not Started
- **Entry Point:** Data annotations, ModelState
- **Features:**
  - Required fields
  - String length limits
  - Email validation
  - Regular expressions (phone, PIN)
  - Server-side validation
  - Client-side validation (not implemented)
- **Dependencies:** ASP.NET MVC validation
- **Legacy Implementation:** Data annotations with ModelState

### 12.4 CSRF Protection
- **Status:** 🔴 Not Started
- **Entry Point:** `ValidateAntiForgeryToken` attribute
- **Features:**
  - Anti-forgery tokens on POST
  - Token validation
- **Dependencies:** ASP.NET MVC anti-forgery
- **Legacy Implementation:** Built-in MVC attribute

### 12.5 SQL Injection Prevention
- **Status:** 🔴 Not Started
- **Entry Point:** Entity Framework 6
- **Features:**
  - Parameterized queries via LINQ
  - No raw SQL
- **Dependencies:** EF6
- **Legacy Implementation:** EF6 LINQ queries

---

## 13. Configuration

### 13.1 Connection String
- **Status:** 🔴 Not Started
- **Entry Point:** Web.config
- **Features:**
  - SQL Server LocalDB
  - Database name: LegacyEcommerce
- **Dependencies:** SQL Server LocalDB
- **Legacy Implementation:** LocalDB connection string

### 13.2 Application Settings
- **Status:** 🔴 Not Started
- **Entry Point:** Web.config appSettings
- **Features:**
  - Culture setting (en-IN)
  - Timezone (not configured)
- **Dependencies:** None
- **Legacy Implementation:** Web.config configuration

---

## Summary Statistics

- **Total Entities:** 15 (User, Product, Category, Brand, Order, OrderItem, CartItem, UserCart, WishlistItem, Review, Coupon, CustomerAddress, ContactMessage, NewsletterSubscriber, PasswordReset)
- **Total Controllers:** 9 (Home, Products, Cart, Checkout, Account, Admin, Wishlist, Sitemap, Error)
- **Total Knowledge Bytes:** 20 documentation files
- **Database Tables:** 15
- **Seeded Products:** 2,500
- **Seeded Categories:** 12
- **Default Admin Credentials:** admin@novakart.com / Admin123!
- **Pricing Constants:** 18% GST, ₹49 shipping, ₹999 free shipping threshold

---

## Migration Priority

### Phase 1 (Critical Path)
1. Authentication & Authorization
2. Product Catalog & Search
3. Cart & Checkout
4. Order Management
5. Basic Admin (Products, Orders, Dashboard)

### Phase 2 (Customer Experience)
6. Customer Account
7. Wishlist
8. Reviews
9. Address Management

### Phase 3 (Marketing & Communication)
10. Coupons
11. Contact Form
12. Newsletter
13. SEO Features

### Phase 4 (Admin Completeness)
14. Full Admin Panel
15. Customer Management
16. Message Management
17. Review Moderation

### Phase 5 (Polish)
18. Recently Viewed
19. Static Pages
20. Image Management
