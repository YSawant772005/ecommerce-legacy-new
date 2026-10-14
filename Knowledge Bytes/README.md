# NovaKart Knowledge Bytes

This folder contains a progressive, repository-grounded guide to the NovaKart codebase.

## How to use this guide

Read the files in numerical order. Each file contains one or more small learning units and follows the requested format:

- **Builds on**
- **File(s)**
- **In plain terms**
- **The code**
- **What's happening**
- **Connection to the project**
- **Why it matters**
- **Remember**
- **Check yourself**

## Learning order

1. [01 - System overview](./01-system-overview.md)
2. [02 - Application startup](./02-application-startup.md)
3. [03 - Frontend routing and state](./03-frontend-routing-and-state.md)
4. [04 - API communication](./04-api-communication.md)
5. [05 - Backend controllers and persistence](./05-backend-controllers-and-persistence.md)
6. [06 - Authentication and authorization](./06-authentication-and-authorization.md)
7. [07 - Catalog workflow](./07-catalog-workflow.md)
8. [08 - Cart and pricing workflow](./08-cart-and-pricing-workflow.md)
9. [09 - Checkout and orders workflow](./09-checkout-and-orders-workflow.md)
10. [10 - Seeding, testing, and debugging](./10-seeding-testing-and-debugging.md)

## Important implementation notes

- The backend is an ASP.NET Core controller-based API, although the README calls it a Minimal API.
- Payment processing is mocked; no payment provider is configured.
- Cart and order pricing logic is duplicated in controllers as well as `PricingService`, so those paths should remain consistent when changed.
- Backend tests currently provide stronger workflow coverage than the five frontend page-load smoke tests.
- The migration documents contain older status descriptions; the source code is the implementation source of truth.
