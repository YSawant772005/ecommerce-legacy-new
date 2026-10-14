# Byte 3: Frontend routing and shared state

**Builds on:** Bytes 1–2

**File(s):** [frontend/src/router/index.ts](../frontend/src/router/index.ts), [frontend/src/stores/auth.ts](../frontend/src/stores/auth.ts), [frontend/src/stores/catalog.ts](../frontend/src/stores/catalog.ts), [frontend/src/stores/cart.ts](../frontend/src/stores/cart.ts), [frontend/src/App.vue](../frontend/src/App.vue)

**In plain terms:**

Vue Router selects pages from the URL. Pinia stores keep shared data such as the current user, products, and cart available to multiple components.

**The code:**

```ts
{
  path: '/checkout',
  name: 'checkout',
  meta: { requiresAuth: true },
  component: () => import('../views/Checkout.vue')
}
```

```ts
const itemCount = computed(() =>
  cart.value?.items.reduce((sum, item) => sum + item.qty, 0) || 0
)
```

**What's happening:**

Routes lazy-load views. Route metadata describes access requirements. Stores call API modules, update reactive state, and expose computed values such as cart item count.

**Connection to the project:**

`App.vue` displays navigation using auth, cart, and wishlist state. Product pages use the catalog and cart stores. Account pages use auth and orders stores.

**Why it matters:**

Without centralized state, the header, product pages, cart page, and account pages could disagree about the current user or cart.

**Remember:**

Views display state; stores coordinate state changes and API calls.

**Check yourself:**

Which route metadata property marks checkout as protected?
