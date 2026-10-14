# Byte 4: API communication and token refresh

**Builds on:** Bytes 2–3

**File(s):** [frontend/src/api/client.ts](../frontend/src/api/client.ts), [frontend/src/api/products.ts](../frontend/src/api/products.ts), [frontend/src/api/auth.ts](../frontend/src/api/auth.ts)

**In plain terms:**

The shared Axios client is the single frontend HTTP boundary. Feature-specific modules use it to call product, authentication, cart, order, and account endpoints.

**The code:**

```ts
const api = axios.create({
  baseURL: '/api',
  timeout: 10000
})
```

```ts
if (error.response?.status === 401 && !originalRequest._retry) {
  originalRequest._retry = true
  // request a new access token, then retry the original request
}
```

**What's happening:**

The request interceptor adds the access token from local storage. If an API request returns 401, the response interceptor tries the refresh token, stores replacement tokens, and retries the original request.

**Connection to the project:**

`products.ts`, `auth.ts`, `cart.ts`, and `orders.ts` all reuse `client.ts`. This keeps the base URL and authentication behavior consistent.

**Why it matters:**

Changing the shared client affects every API request. A bad retry implementation can cause loops or unexpectedly log users out.

**Remember:**

API modules describe feature endpoints; the shared client describes cross-cutting HTTP behavior.

**Check yourself:**

What does the client do when token refresh also fails?
