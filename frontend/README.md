# NovaKart Frontend - Vue 3 SPA

Modern Vue 3 Single Page Application for NovaKart e-commerce platform.

## Tech Stack

- Vue 3 with Composition API
- TypeScript
- Vite
- Pinia (state management)
- Vue Router
- Axios

## Prerequisites

- Node.js 18+
- npm or yarn

## Setup

1. Install dependencies:
```bash
cd frontend
npm install
```

2. Run development server:
```bash
npm run dev
```

The app will be available at `http://localhost:5173`

## Build for Production

```bash
npm run build
```

## Features Implemented

- ✅ Authentication (login, register, logout)
- ✅ Product catalog with pagination, filtering, sorting
- ✅ Product detail pages
- ✅ Shopping cart (add, update, remove, coupon)
- ✅ Checkout flow
- ✅ Order history
- ✅ Account management
- ✅ Admin dashboard (basic)
- ✅ JWT token management with auto-refresh
- ✅ Route guards (auth, admin, guest-only)

## API Integration

The frontend proxies requests to the backend API at `http://localhost:5000/api` via Vite proxy configuration.

## Note

TypeScript errors in the IDE are expected until `npm install` is run to install dependencies.
