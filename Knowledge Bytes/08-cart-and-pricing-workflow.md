# Byte 8: Cart and pricing workflow

**Builds on:** Bytes 5–7

**File(s):** [frontend/src/stores/cart.ts](../frontend/src/stores/cart.ts), [backend/Ecommerce.Api/Controllers/CartController.cs](../backend/Ecommerce.Api/Controllers/CartController.cs), [backend/Ecommerce.Infrastructure/Services/PricingService.cs](../backend/Ecommerce.Infrastructure/Services/PricingService.cs)

**In plain terms:**

Authenticated cart items are stored in the database. The server calculates quantity limits, stock checks, coupon discounts, shipping, tax, and totals.

**The code:**

```csharp
var requestedQty = Math.Min(10, Math.Max(1, request.Qty));
var targetQty = Math.Min(10, currentQty + requestedQty);
```

```csharp
var tax = Math.Round(subtotal * 0.18m, 2);
var total = subtotal - discount + shipping + tax;
```

**What's happening:**

Adding an item checks the product and available stock, then caps the quantity at ten. Shipping is ₹49 below ₹999 and free at or above ₹999. Coupons can provide fixed discounts, percentage discounts, or free shipping.

**Connection to the project:**

The cart store reloads the complete cart after every mutation, so the UI displays server-calculated totals.

**Why it matters:**

Pricing must not be trusted from browser-submitted values. The backend must calculate the amount used for an order.

**Remember:**

The browser displays cart totals; the server determines them.

**Check yourself:**

What happens if the requested cart quantity is zero?
