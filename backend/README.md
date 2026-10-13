# NovaKart Backend - Modernized E-commerce API

Modern .NET 10 Minimal API backend for NovaKart e-commerce platform.

## Tech Stack

- .NET 10.0
- Entity Framework Core 10.0
- SQL Server LocalDB
- JWT Authentication with Refresh Tokens
- Serilog for logging
- Swagger/OpenAPI

## Prerequisites

- .NET 10.0 SDK
- SQL Server LocalDB (included with Visual Studio)

## Setup

1. Restore dependencies:
```bash
cd backend
dotnet restore
```

2. Update connection string in `Ecommerce.Api/appsettings.json` if needed:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=NovaKartDb;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

3. Run the application:
```bash
cd Ecommerce.Api
dotnet run
```

The API will be available at `https://localhost:5001` or `http://localhost:5000`

## Database

The database is migrated and seeded automatically on first run with:
- 6 categories
- 6 brands
- 360 products
- 2 users (admin + customer)
- 3 coupons

## Default Credentials

**Admin:**
- Email: admin@novakart.com
- Password: Admin123!

**Customer:**
- Email: john@example.com
- Password: Customer123!

## API Documentation

Swagger UI is available at `/swagger` when running in development mode.

## Key Endpoints

### Authentication
- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - Login
- `POST /api/auth/refresh` - Refresh access token
- `POST /api/auth/logout` - Logout
- `GET /api/auth/me` - Get current user

### Products
- `GET /api/products/categories` - Get all categories
- `GET /api/products` - Get products with pagination, filtering, sorting
- `GET /api/products/{id}` - Get product details

### Cart
- `GET /api/cart` - Get user's cart
- `POST /api/cart/items` - Add item to cart
- `PUT /api/cart/items/{productId}` - Update cart item
- `DELETE /api/cart/items/{productId}` - Remove item from cart
- `DELETE /api/cart` - Clear cart
- `POST /api/cart/coupon` - Apply coupon

### Orders
- `POST /api/orders` - Create order
- `GET /api/orders` - Get user's orders
- `GET /api/orders/{id}` - Get order details

## Pricing Rules

- GST: 18%
- Shipping: ₹49 (free above ₹999)
- Coupons: Fixed amount, percentage with cap, free shipping

## Security

- JWT access tokens (30 minute expiry)
- Refresh tokens (7 day expiry)
- Password hashing with PBKDF2
- Role-based authorization (Admin/Customer)
