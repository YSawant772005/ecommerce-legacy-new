# Byte 9: Checkout and order creation

**Builds on:** Byte 8

**File(s):** [frontend/src/views/Checkout.vue](../frontend/src/views/Checkout.vue), [frontend/src/stores/orders.ts](../frontend/src/stores/orders.ts), [backend/Ecommerce.Api/Controllers/OrdersController.cs](../backend/Ecommerce.Api/Controllers/OrdersController.cs), [backend/Ecommerce.Core/Entities/OrderItem.cs](../backend/Ecommerce.Core/Entities/OrderItem.cs)

**In plain terms:**

Checkout sends customer and address information to the backend. The backend turns the current cart into a durable order.

**The code:**

```ts
await ordersStore.createOrder({
  ...form.value,
  couponCode: cartStore.cart?.couponCode
})
router.push('/account/orders')
```

```csharp
order.Items.Add(new OrderItem
{
    ProductId = cartItem.ProductId,
    ProductName = cartItem.Product.Name,
    UnitPrice = cartItem.Product.Price,
    Quantity = cartItem.Qty
});

cartItem.Product.Stock -= cartItem.Qty;
```

**What's happening:**

The API calculates the order totals, copies purchase-time product information into order items, decreases stock, removes the cart items, clears the coupon, and returns the created order. The payment reference is currently mocked.

**Connection to the project:**

After a successful request, the checkout view redirects to order history, which uses `GET /api/orders`.

**Why it matters:**

An order must preserve what was purchased even if the product name or price changes later.

**Remember:**

Order items are purchase snapshots, not just live product references.

**Check yourself:**

Does the current repository integrate a real payment provider?
