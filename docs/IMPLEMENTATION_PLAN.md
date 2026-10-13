# Implementation Plan

**Source Repository:** https://github.com/YSawant772005/ecommerce-legacy.git  
**Destination Repository:** https://github.com/YSawant772005/ecommerce-legacy-new.git  
**Analysis Date:** October 13, 2026

---

## Overview

This document outlines the phased implementation plan for migrating NovaKart from ASP.NET MVC 5 to Vue 3 + .NET 10 Minimal API. The plan follows the mandatory workflow specified in the assignment, with clear stages, deliverables, and acceptance criteria.

---

## Stage A: Repository Setup and Audit

**Status:** 🟡 In Progress  
**Estimated Duration:** 1-2 days

### Objectives
- Clone source repository
- Configure destination remote
- Conduct comprehensive legacy codebase audit
- Create audit documentation

### Tasks
- [x] Clone source repository (via GitHub API due to git limitations)
- [ ] Configure destination remote (ecommerce-legacy-new)
- [ ] Create feature branch `migration/main`
- [ ] Inspect entire legacy codebase
- [x] Create `docs/LEGACY_FEATURE_INVENTORY.md`
- [x] Create `docs/MIGRATION_MATRIX.md`
- [ ] Create `docs/LEGACY_BEHAVIOR_CONTRACT.md`
- [ ] Verify destination repository is empty before push

### Deliverables
- Local working copy of legacy repository
- Feature inventory document
- Migration matrix document
- Behavior contract document
- Configured Git remotes

### Acceptance Criteria
- All documentation files created
- No changes pushed to source repository
- Destination repository ready for initial commit
- Feature branch created and checked out

### Risks
- Git clone failure due to network or permissions
- **Mitigation:** Use GitHub API to download files, manual Git setup

---

## Stage B: Architecture and Data Design

**Status:** 🔴 Not Started  
**Estimated Duration:** 2-3 days

### Objectives
- Define domain model for EF Core 10
- Design API contracts and DTOs
- Plan database migration strategy
- Design frontend architecture

### Tasks
- [ ] Define entity classes matching legacy schema
- [ ] Design DbContext with EF Core 10 conventions
- [ ] Create DTO classes for all API endpoints
- [ ] Design API endpoint structure (groups, routes)
- [ ] Design Pinia store structure
- [ ] Design Vue component hierarchy
- [ ] Design Vue Router configuration
- [ ] Plan authentication flow (JWT + refresh tokens)
- [ ] Plan database initialization and seeding strategy
- [ ] Create API specification document

### Deliverables
- Domain entity design document
- API contract specification
- Database schema design
- Frontend architecture diagram
- Authentication flow diagram

### Acceptance Criteria
- All entities mapped from legacy to new schema
- All API endpoints defined with request/response DTOs
- Database migration strategy documented
- Frontend architecture approved

### Risks
- EF Core 10 differences from EF6
- **Mitigation:** Research EF Core 10 documentation, test mappings early
- JWT complexity vs session simplicity
- **Mitigation:** Follow established JWT patterns, use secure libraries

---

## Stage C: Backend Foundation

**Status:** 🔴 Not Started  
**Estimated Duration:** 3-4 days

### Objectives
- Create .NET 10 solution structure
- Implement EF Core model and DbContext
- Create and apply database migrations
- Implement seeding logic
- Set up health checks and OpenAPI

### Tasks
- [ ] Create solution structure:
  - `Ecommerce.Api`
  - `Ecommerce.Core`
  - `Ecommerce.Infrastructure`
  - `Ecommerce.Tests`
- [ ] Configure .NET 10 Minimal API project
- [ ] Add EF Core 10 packages
- [ ] Implement entity classes in Core
- [ ] Implement DbContext in Infrastructure
- [ ] Configure entity relationships and constraints
- [ ] Create initial EF Core migration
- [ ] Implement database seeder (port legacy seeder)
- [ ] Configure connection string (SQL Server Express/LocalDB)
- [ ] Add health check endpoint
- [ ] Configure Swagger/OpenAPI
- [ ] Configure CORS policy
- [ ] Add Serilog configuration
- [ ] Configure ProblemDetails middleware
- [ ] Add FluentValidation package
- [ ] Create base API response classes

### Deliverables
- .NET 10 solution with 4 projects
- EF Core DbContext with all entities
- Database migration files
- Working seeder with 25+ products, 5+ categories
- Health check endpoint responding
- Swagger UI accessible at `/swagger`
- Serilog logging configured

### Acceptance Criteria
- Solution builds without errors
- Migration creates all 15+ tables
- Seeder runs successfully without exceptions
- Health endpoint returns 200 OK
- Swagger UI displays API documentation
- Fresh database can be initialized and seeded

### Risks
- EF Core 10 not available (may need .NET 8 or preview)
- **Mitigation:** Use latest stable .NET version, document version used
- Migration conflicts with legacy schema
- **Mitigation:** Test migration on empty database, verify all tables created
- Seeder performance with 2,500 products
- **Mitigation:** Optimize seeder, reduce to 25 for development if needed

---

## Stage D: Authentication and Catalog APIs

**Status:** 🔴 Not Started  
**Estimated Duration:** 4-5 days

### Objectives
- Implement JWT-based authentication
- Implement catalog (products, categories) APIs
- Implement search and filtering
- Implement admin authorization

### Tasks
- [ ] Implement password hashing service (BCrypt or Identity)
- [ ] Implement JWT token generation service
- [ ] Implement refresh token service with rotation
- [ ] Create authentication endpoints:
  - `POST /api/auth/register`
  - `POST /api/auth/login`
  - `POST /api/auth/refresh`
  - `POST /api/auth/logout`
  - `GET /api/auth/me`
- [ ] Implement role-based authorization middleware
- [ ] Create catalog endpoints:
  - `GET /api/categories`
  - `GET /api/products` (with pagination, search, filters, sort)
  - `GET /api/products/{id}`
  - `GET /api/products/suggest`
- [ ] Implement search logic (EF Core queries)
- [ ] Implement filtering logic (brand, price, rating)
- [ ] Implement sorting logic (popular, price, rating, new, discount)
- [ ] Implement pagination metadata
- [ ] Add FluentValidation for auth DTOs
- [ ] Add unit tests for authentication services
- [ ] Add integration tests for catalog endpoints

### Deliverables
- Working authentication flow with JWT
- Secure refresh token mechanism
- Catalog API with full search/filter/sort
- Authorization middleware protecting admin routes
- Unit tests for auth services
- Integration tests for catalog APIs

### Acceptance Criteria
- Registration creates user with hashed password
- Login returns JWT access token and refresh token
- Refresh token rotation works correctly
- Catalog API returns correct products with filters
- Search returns relevant results
- Pagination works correctly
- Admin endpoints reject non-admin requests
- All tests pass

### Risks
- JWT token expiry handling complexity
- **Mitigation:** Implement standard refresh pattern, test expiry scenarios
- Search performance with 2,500 products
- **Mitigation:** Add database indexes, consider query optimization
- Authorization bypass vulnerabilities
- **Mitigation:** Server-side checks on all sensitive endpoints, test thoroughly

---

## Stage E: Cart, Checkout, and Order APIs

**Status:** 🔴 Not Started  
**Estimated Duration:** 5-6 days

### Objectives
- Implement cart management APIs
- Implement pricing and coupon logic
- Implement checkout flow APIs
- Implement order management APIs

### Tasks
- [ ] Implement pricing service (GST, shipping, discounts)
- [ ] Implement coupon validation service
- [ ] Create cart endpoints:
  - `GET /api/cart`
  - `POST /api/cart/items`
  - `PUT /api/cart/items/{productId}`
  - `DELETE /api/cart/items/{productId}`
  - `DELETE /api/cart`
  - `POST /api/cart/merge`
- [ ] Implement cart persistence for authenticated users
- [ ] Implement cart merge logic on login
- [ ] Create checkout endpoints:
  - `POST /api/orders`
- [ ] Implement order creation with transaction
- [ ] Implement stock decrement logic
- [ ] Implement order number generation
- [ ] Create order endpoints:
  - `GET /api/orders`
  - `GET /api/orders/{id}`
- [ ] Implement order status update endpoint (admin)
- [ ] Add FluentValidation for cart and order DTOs
- [ ] Add unit tests for pricing and coupon calculations
- [ ] Add integration tests for cart and checkout
- [ ] Add integration tests for order management

### Deliverables
- Cart API with full CRUD operations
- Cart merge on login
- Pricing service with correct GST/shipping/discount math
- Coupon validation service
- Order creation API with stock management
- Order status update API
- Unit tests for pricing calculations
- Integration tests for cart/checkout/orders

### Acceptance Criteria
- Cart operations work for guest and authenticated users
- Cart merge correctly combines session and persisted carts
- Pricing calculations match legacy (18% GST, ₹49 shipping, ₹999 threshold)
- Coupon validation matches legacy rules
- Order creation decrements stock correctly
- Order status updates restock on cancellation
- All pricing edge cases tested (max discount, percentage caps, etc.)
- All tests pass

### Risks
- Pricing calculation discrepancies with legacy
- **Mitigation:** Port legacy Pricing.Compute exactly, add regression tests
- Cart merge logic complexity
- **Mitigation:** Test merge scenarios thoroughly (empty cart, duplicates, etc.)
- Stock race conditions
- **Mitigation:** Use database transactions, test concurrent order placement

---

## Stage F: Account, Wishlist, and Reviews APIs

**Status:** 🔴 Not Started  
**Estimated Duration:** 3-4 days

### Objectives
- Implement customer account APIs
- Implement wishlist APIs
- Implement review APIs
- Implement address management APIs

### Tasks
- [ ] Create account endpoints:
  - `GET /api/account/profile`
  - `PUT /api/account/profile`
  - `POST /api/account/password`
- [ ] Create address endpoints:
  - `GET /api/account/addresses`
  - `POST /api/account/addresses`
  - `PUT /api/account/addresses/{id}`
  - `DELETE /api/account/addresses/{id}`
- [ ] Implement password reset flow:
  - `POST /api/auth/forgot-password`
  - `POST /api/auth/reset-password`
- [ ] Create wishlist endpoints:
  - `GET /api/wishlist`
  - `POST /api/wishlist/items`
  - `DELETE /api/wishlist/items/{productId}`
- [ ] Create review endpoints:
  - `GET /api/products/{id}/reviews`
  - `POST /api/products/{id}/reviews`
- [ ] Implement verified purchase check
- [ ] Implement rating recalculation on review submission
- [ ] Add resource ownership checks (users can only access their own data)
- [ ] Add FluentValidation for account DTOs
- [ ] Add integration tests for account APIs
- [ ] Add integration tests for wishlist and reviews

### Deliverables
- Account profile management API
- Address management API
- Password reset API
- Wishlist API
- Review API with verified purchase
- Resource ownership authorization
- Integration tests for all endpoints

### Acceptance Criteria
- Profile updates persist correctly
- Address CRUD works with validation
- Password reset flow works (mock email)
- Wishlist operations work correctly
- Review submission updates product ratings
- Verified purchase check works correctly
- Users cannot access other users' data
- All tests pass

### Risks
- Resource ownership bypass
- **Mitigation:** Add ownership checks in service layer, test with different users
- Password reset token security
- **Mitigation:** Use secure random tokens, implement expiry (even if not in legacy)
- Rating calculation errors
- **Mitigation:** Port legacy rating math exactly, add unit tests

---

## Stage G: Admin APIs

**Status:** 🔴 Not Started  
**Estimated Duration:** 4-5 days

### Objectives
- Implement admin dashboard API
- Implement admin product management
- Implement admin order management
- Implement admin category/brand management
- Implement admin coupon management
- Implement admin customer/message/review management

### Tasks
- [ ] Create admin endpoints group with authorization
- [ ] Create dashboard endpoint:
  - `GET /api/admin/dashboard`
- [ ] Create product management endpoints:
  - `GET /api/admin/products`
  - `POST /api/admin/products`
  - `PUT /api/admin/products/{id}`
  - `DELETE /api/admin/products/{id}`
- [ ] Create category management endpoints:
  - `GET /api/admin/categories`
  - `POST /api/admin/categories`
  - `PUT /api/admin/categories/{id}`
  - `DELETE /api/admin/categories/{id}`
- [ ] Create brand management endpoints:
  - `GET /api/admin/brands`
  - `POST /api/admin/brands`
  - `PUT /api/admin/brands/{id}`
  - `DELETE /api/admin/brands/{id}`
- [ ] Create order management endpoints:
  - `GET /api/admin/orders`
  - `GET /api/admin/orders/{id}`
  - `PUT /api/admin/orders/{id}/status`
- [ ] Create customer management endpoints:
  - `GET /api/admin/customers`
- [ ] Create coupon management endpoints:
  - `GET /api/admin/coupons`
  - `POST /api/admin/coupons`
  - `PUT /api/admin/coupons/{id}`
  - `DELETE /api/admin/coupons/{id}`
- [ ] Create message management endpoints:
  - `GET /api/admin/messages`
  - `DELETE /api/admin/messages/{id}`
- [ ] Create review moderation endpoints:
  - `GET /api/admin/reviews`
  - `PUT /api/admin/reviews/{id}/approve`
  - `PUT /api/admin/reviews/{id}/hide`
  - `DELETE /api/admin/reviews/{id}`
- [ ] Implement catalog cache refresh on product/category changes
- [ ] Add admin role authorization to all admin endpoints
- [ ] Add integration tests for admin APIs

### Deliverables
- Admin dashboard API with metrics
- Full admin product management
- Full admin category/brand management
- Full admin order management
- Admin customer management
- Admin coupon management
- Admin message management
- Admin review moderation
- Admin authorization on all endpoints
- Integration tests for admin APIs

### Acceptance Criteria
- Dashboard metrics calculate correctly
- Product CRUD works with cache refresh
- Category/brand CRUD works
- Order status updates work with stock restock
- Customer list shows spend metrics
- Coupon CRUD works
- Message and review management works
- Non-admin users cannot access admin endpoints
- All tests pass

### Risks
- Admin authorization bypass
- **Mitigation:** Test admin endpoints with regular user accounts
- Cache refresh failures
- **Mitigation:** Implement cache refresh as background job or immediate action
- Stock management errors in admin
- **Mitigation:** Test stock updates thoroughly, use transactions

---

## Stage H: Contact, Newsletter, and SEO APIs

**Status:** 🔴 Not Started  
**Estimated Duration:** 1-2 days

### Objectives
- Implement contact form API
- Implement newsletter subscription API
- Implement sitemap generation
- Implement robots.txt

### Tasks
- [ ] Create contact endpoint:
  - `POST /api/contact`
- [ ] Create newsletter endpoints:
  - `POST /api/newsletter/subscribe`
  - `GET /api/admin/newsletter`
  - `DELETE /api/admin/newsletter/{id}`
- [ ] Implement duplicate email prevention
- [ ] Create sitemap endpoint:
  - `GET /sitemap.xml`
- [ ] Serve static robots.txt
- [ ] Add integration tests for contact and newsletter

### Deliverables
- Contact form API
- Newsletter subscription API
- Newsletter admin management
- Dynamic sitemap.xml
- Static robots.txt
- Integration tests

### Acceptance Criteria
- Contact messages persist correctly
- Newsletter subscriptions prevent duplicates
- Sitemap generates valid XML
- Robots.txt is accessible
- All tests pass

### Risks
- Sitemap performance with 2,500 products
- **Mitigation:** Cache sitemap or generate periodically
- Email validation complexity
- **Mitigation:** Use standard email validation regex

---

## Stage I: Vue 3 SPA Foundation

**Status:** 🔴 Not Started  
**Estimated Duration:** 3-4 days

### Objectives
- Set up Vue 3 + Vite project
- Configure TypeScript
- Set up routing and state management
- Set up API client and interceptors
- Configure build and deployment

### Tasks
- [ ] Create Vue 3 project with Vite
- [ ] Configure TypeScript
- [ ] Install and configure Tailwind CSS or Bootstrap 5
- [ ] Set up Vue Router with route configuration
- [ ] Set up Pinia stores:
  - Auth store
  - Cart store
  - Catalog store
  - Orders store
  - UI store
- [ ] Configure Axios with base URL and interceptors
- [ ] Implement JWT attachment in Axios interceptor
- [ ] Implement 401 handling with token refresh
- [ ] Create API module structure (organized by feature)
- [ ] Set up route guards:
  - Guest-only guard (login/register)
  - Authenticated guard (account pages)
  - Admin guard (admin pages)
- [ ] Create shared layouts:
  - Main layout (storefront)
  - Admin layout
  - Auth layout
- [ ] Configure environment variables
- [ ] Set up Vitest
- [ ] Set up Vue Test Utils
- [ ] Create base components (Button, Input, Card, etc.)

### Deliverables
- Vue 3 SPA with TypeScript
- Configured routing with guards
- Pinia stores for state management
- Axios client with JWT handling
- API module structure
- Shared layouts
- Base component library
- Vitest configuration

### Acceptance Criteria
- Vue app compiles without errors
- Vite dev server starts successfully
- Router navigation works
- Pinia stores persist state
- Axios interceptors attach JWT correctly
- 401 handling triggers token refresh
- Route guards protect routes correctly
- TypeScript compilation succeeds

### Risks
- Vue 3 + TypeScript learning curve
- **Mitigation:** Follow Vue 3 documentation, use TypeScript strict mode
- Axios interceptor complexity
- **Mitigation:** Test refresh flow thoroughly, handle edge cases
- Route guard bypass
- **Mitigation:** Test guards with authenticated/unauthenticated states

---

## Stage J: Storefront Views Implementation

**Status:** 🔴 Not Started  
**Estimated Duration:** 5-6 days

### Objectives
- Implement home page
- Implement catalog/product listing pages
- Implement product detail pages
- Implement cart page
- Implement checkout flow
- Implement account pages

### Tasks
- [ ] Implement home page:
  - Category display
  - Featured products
  - Deals section
  - New arrivals
- [ ] Implement catalog page:
  - Product grid
  - Filters (category, brand, price, rating)
  - Sorting
  - Pagination/infinite scroll
  - Search functionality
- [ ] Implement product detail page:
  - Image gallery
  - Product information
  - Variant selection (color/size)
  - Add to cart
  - Reviews display
  - Review submission form
  - Related products
- [ ] Implement cart page:
  - Cart items list
  - Quantity controls
  - Remove items
  - Move to wishlist
  - Coupon input
  - Totals display
- [ ] Implement checkout flow:
  - Address step
  - Payment step
  - Review step
  - Confirmation page
- [ ] Implement auth pages:
  - Login page
  - Register page
  - Forgot password page
  - Reset password page
- [ ] Implement account pages:
  - Profile page
  - Order history page
  - Order details page
  - Address management page
- [ ] Implement wishlist page
- [ ] Implement contact page
- [ ] Add loading states
- [ ] Add error states
- [ ] Add success notifications
- [ ] Implement responsive design
- [ ] Write Vitest tests for stores and components
- [ ] Write Playwright/Cypress E2E tests for critical flows

### Deliverables
- Complete storefront with all pages
- Working checkout flow
- Working account management
- Responsive design
- Loading and error states
- Unit tests for stores and components
- E2E tests for critical user flows

### Acceptance Criteria
- All pages render correctly
- Navigation works smoothly
- Forms validate correctly
- API calls integrate with stores
- Loading states display during API calls
- Errors display user-friendly messages
- Checkout flow completes successfully
- Account features work correctly
- Responsive design works on mobile
- Unit tests pass
- E2E tests pass

### Risks
- Vue component complexity
- **Mitigation:** Break down into smaller components, use composition API
- State synchronization issues
- **Mitigation:** Clear state ownership, use Pinia effectively
- E2E test flakiness
- **Mitigation:** Use stable selectors, wait for elements, retry logic

---

## Stage K: Admin Views Implementation

**Status:** 🔴 Not Started  
**Estimated Duration:** 4-5 days

### Objectives
- Implement admin dashboard
- Implement admin product management
- Implement admin order management
- Implement admin category/brand management
- Implement admin coupon management
- Implement admin customer/message/review management

### Tasks
- [ ] Implement admin dashboard:
  - Metrics display
  - Low stock alerts
  - Recent orders
  - Orders by status
- [ ] Implement admin product list:
  - Table with search/filter
  - Pagination
- [ ] Implement admin product form:
  - Create/edit form
  - Validation
  - Image URL input
- [ ] Implement admin category management:
  - Category list
  - Create/edit forms
- [ ] Implement admin brand management:
  - Brand list
  - Create/edit forms
- [ ] Implement admin order management:
  - Order list with search/filter
  - Order details view
  - Status update form
- [ ] Implement admin customer list:
  - Customer table
  - Spend metrics
- [ ] Implement admin coupon management:
  - Coupon list
  - Create/edit forms
- [ ] Implement admin message management:
  - Message list
  - Message details
- [ ] Implement admin review moderation:
  - Review list
  - Approve/hide/delete actions
- [ ] Add admin layout with navigation
- [ ] Add admin route guard
- [ ] Write E2E tests for admin flows

### Deliverables
- Complete admin panel
- All admin management screens
- Admin navigation
- Admin authorization
- E2E tests for admin flows

### Acceptance Criteria
- Dashboard displays correct metrics
- All CRUD operations work
- Forms validate correctly
- Status updates work
- Admin guard protects routes
- E2E tests pass

### Risks
- Admin UI complexity
- **Mitigation:** Reuse components, consistent design patterns
- Authorization bypass in UI
- **Mitigation:** Server-side authorization is primary defense, UI is secondary

---

## Stage L: Testing and Verification

**Status:** 🔴 Not Started  
**Estimated Duration:** 3-4 days

### Objectives
- Run all unit tests
- Run all integration tests
- Run all E2E tests
- Verify feature parity with legacy
- Fix all failing tests
- Achieve coverage targets

### Tasks
- [ ] Run backend unit tests (target 70% coverage for Core)
- [ ] Run backend integration tests
- [ ] Run frontend unit tests (target 60% coverage for stores/components)
- [ ] Run frontend E2E tests
- [ ] Verify all mandatory assignment features work:
  - Authentication (register, login, logout, refresh)
  - Catalog (products, categories, search, filters)
  - Cart (add, update, remove, merge)
  - Checkout (complete flow)
  - Orders (history, details)
  - Account (profile, addresses)
  - Admin (dashboard, products, categories, orders)
- [ ] Verify additional legacy features work:
  - Wishlist
  - Reviews
  - Coupons
  - Contact form
  - Newsletter
  - Customer management
  - Message management
  - Review moderation
- [ ] Verify pricing calculations match legacy
- [ ] Verify coupon rules match legacy
- [ ] Verify stock management matches legacy
- [ ] Verify SEO features (sitemap, robots.txt)
- [ ] Fix any failing tests
- [ ] Add missing tests if coverage targets not met
- [ ] Manual verification of UI/UX

### Deliverables
- All tests passing
- Coverage targets met
- Feature parity verified
- Test report generated

### Acceptance Criteria
- Backend unit tests pass with 70%+ coverage
- Backend integration tests pass
- Frontend unit tests pass with 60%+ coverage
- Frontend E2E tests pass
- All mandatory features verified working
- Additional legacy features verified working
- No critical bugs remaining

### Risks
- Coverage targets not met
- **Mitigation:** Add tests for uncovered paths, refactor for testability
- Feature parity gaps discovered
- **Mitigation:** Implement missing features, document intentional omissions
- Test flakiness
- **Mitigation:** Stabilize tests, retry logic, better selectors

---

## Stage M: Documentation and Delivery

**Status:** 🔴 Not Started  
**Estimated Duration:** 2-3 days

### Objectives
- Update README with setup instructions
- Create API documentation
- Create demo checklist
- Write reflection document
- Commit and push to destination repository
- Verify pushed repository

### Tasks
- [ ] Update README.md:
  - Prerequisites
  - Database setup
  - Backend startup instructions
  - Frontend startup instructions
  - Sample credentials (admin@novakart.com / Admin123!)
  - API documentation link
  - Troubleshooting guide
- [ ] Create API documentation (OpenAPI/Swagger)
  - Document all endpoints
  - Document authentication flow
  - Document error responses
- [ ] Create demo checklist:
  - 5-10 minute demonstration plan
  - Storefront flow (browse, cart, checkout)
  - Account flow (login, orders)
  - Admin flow (dashboard, products, orders)
- [ ] Write reflection document (1-2 pages):
  - MVC-to-SPA migration challenges
  - Session-based cart to API cart migration
  - SPA/API trade-offs
  - Lessons learned
- [ ] Update migration matrix with final status
- [ ] Update feature inventory with final status
- [ ] Commit all changes with sensible messages
- [ ] Push to destination repository (ecommerce-legacy-new)
- [ ] Verify pushed files and commit history
- [ ] Verify destination repository is not source repository
- [ ] Tag release (optional)

### Deliverables
- Complete README.md
- API documentation
- Demo checklist
- Reflection document
- Updated migration matrix
- Updated feature inventory
- Committed and pushed repository
- Verified destination repository

### Acceptance Criteria
- README is complete and accurate
- Fresh developer can follow README to run application
- API documentation is comprehensive
- Demo checklist covers all major features
- Reflection document is thoughtful
- All changes committed with clear messages
- Repository pushed to correct destination
- Source repository unchanged
- Destination repository contains all modernized code

### Risks
- README incomplete or inaccurate
- **Mitigation:** Follow README from scratch to verify
- Push to wrong repository
- **Mitigation:** Double-check remote URLs before push
- Missing files in push
- **Mitigation:** Verify pushed files match local files

---

## Overall Timeline

**Total Estimated Duration:** 35-45 days

| Stage | Duration | Dependencies |
|-------|----------|--------------|
| Stage A | 1-2 days | None |
| Stage B | 2-3 days | Stage A |
| Stage C | 3-4 days | Stage B |
| Stage D | 4-5 days | Stage C |
| Stage E | 5-6 days | Stage D |
| Stage F | 3-4 days | Stage D |
| Stage G | 4-5 days | Stage D |
| Stage H | 1-2 days | Stage C |
| Stage I | 3-4 days | Stage D, H |
| Stage J | 5-6 days | Stage I |
| Stage K | 4-5 days | Stage I |
| Stage L | 3-4 days | Stage J, K |
| Stage M | 2-3 days | Stage L |

**Critical Path:** A → B → C → D → E → I → J → L → M

---

## Risk Summary

| Risk | Impact | Probability | Mitigation |
|------|--------|-------------|------------|
| EF Core 10 not available | High | Medium | Use latest stable .NET version |
| JWT complexity | Medium | High | Follow established patterns, test thoroughly |
| Pricing calculation discrepancies | High | Medium | Port legacy code exactly, add regression tests |
| State synchronization issues | Medium | High | Clear state ownership, test extensively |
| Test coverage targets not met | Medium | Medium | Add tests incrementally, refactor for testability |
| Feature parity gaps | High | Medium | Continuous verification against legacy |
| Performance issues with 2,500 products | Medium | Low | Add indexes, optimize queries, cache results |
| Security vulnerabilities | High | Low | Security review, penetration testing |

---

## Success Criteria

The migration will be considered successful when:

1. **Technical Requirements Met**
   - Backend: .NET 10 Minimal API with EF Core 10
   - Frontend: Vue 3 SPA with TypeScript, Pinia, Vue Router
   - Database: SQL Server Express/LocalDB with migrations
   - Authentication: JWT with secure refresh tokens
   - Testing: Unit, integration, and E2E tests passing

2. **Functional Requirements Met**
   - All mandatory assignment features implemented
   - All additional legacy features preserved or documented
   - Pricing calculations match legacy exactly
   - Coupon rules match legacy exactly
   - Stock management matches legacy exactly

3. **Quality Requirements Met**
   - Backend unit test coverage ≥ 70% for Core
   - Frontend unit test coverage ≥ 60% for stores/components
   - All integration tests pass
   - All E2E tests pass
   - No critical bugs remaining

4. **Documentation Requirements Met**
   - Complete README with setup instructions
   - API documentation
   - Demo checklist
   - Reflection document
   - Migration matrix updated
   - Feature inventory updated

5. **Delivery Requirements Met**
   - Code pushed to correct destination repository
   - Source repository unchanged
   - Fresh developer can run application from README
   - Demo can be completed in 5-10 minutes

---

## Next Steps

1. Complete Stage A (Repository Setup and Audit)
2. Begin Stage B (Architecture and Data Design)
3. Proceed through stages sequentially
4. Report progress at end of each stage
5. Address blockers immediately
6. Maintain documentation throughout
