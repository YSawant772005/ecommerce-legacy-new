<template>
  <div id="app">
    <div class="announcement">Free delivery on orders over ₹999 <span>•</span> Easy returns within 7 days <span>•</span> Shop the NovaKart edit</div>
    <nav class="navbar">
      <div class="container nav-container">
        <router-link to="/" class="logo"><span>NK</span> NovaKart</router-link>
        <div class="nav-links">
          <router-link to="/">Home</router-link>
          <router-link to="/products">Products</router-link>
          <router-link to="/contact">Contact</router-link>
          <router-link v-if="!authStore.isAuthenticated" to="/login">Login</router-link>
          <router-link v-if="!authStore.isAuthenticated" to="/register">Register</router-link>
          <router-link v-else to="/account">Account</router-link>
          <router-link v-if="authStore.isAuthenticated" to="/wishlist">Wishlist</router-link>
          <router-link v-if="authStore.isAdmin" to="/admin">Admin</router-link>
          <router-link v-if="authStore.isAuthenticated" to="/cart" class="cart-link">Cart <b>{{ cartStore.itemCount }}</b></router-link>
          <button v-if="authStore.isAuthenticated" @click="authStore.logout">Logout</button>
        </div>
      </div>
    </nav>
    <main class="main-content">
      <router-view />
    </main>
    <footer class="footer">
      <div class="container footer-content">
        <p>&copy; 2026 NovaKart. Curated goods for modern living.</p>
        <div><router-link to="/products">Shop</router-link><router-link to="/contact">Contact</router-link></div>
      </div>
    </footer>
  </div>
</template>

<script setup lang="ts">
import { useAuthStore } from './stores/auth'
import { useCartStore } from './stores/cart'
import { onMounted } from 'vue'

const authStore = useAuthStore()
const cartStore = useCartStore()

onMounted(() => {
  if (authStore.isAuthenticated) {
    cartStore.fetchCart()
  }
})
</script>

<style scoped>
:global(*) {
  box-sizing: border-box;
}

:global(body) {
  margin: 0;
  background: #fbfaff;
  color: #21174d;
  font-family: Inter, ui-sans-serif, system-ui, -apple-system, BlinkMacSystemFont, "Segoe UI", sans-serif;
}

:global(button), :global(input) {
  font: inherit;
}

.navbar {
  background: #24145f;
  padding: 1.1rem 0;
  color: white;
}

.announcement {
  padding: .45rem 1rem;
  background: #0fe1d1;
  color: #24145f;
  text-align: center;
  font-size: .65rem;
  font-weight: 800;
  letter-spacing: .03em;
}

.announcement span {
  padding: 0 .7rem;
  opacity: .6;
}

.container {
  width: min(1120px, calc(100% - 3rem));
  margin: 0 auto;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.nav-container {
  min-height: 42px;
}

.logo {
  font-size: 1.15rem;
  font-weight: 800;
  letter-spacing: -0.04em;
  color: white;
  text-decoration: none;
}

.logo span {
  display: inline-grid;
  place-items: center;
  width: 28px;
  height: 28px;
  margin-right: 0.4rem;
  border: 1px solid rgba(255,255,255,0.65);
  border-radius: 50%;
  font-size: 0.62rem;
  letter-spacing: 0;
}

.nav-links {
  display: flex;
  gap: 1.25rem;
  align-items: center;
  font-size: 0.78rem;
  font-weight: 600;
}

.announcement {
  font-size: .56rem;
}

.nav-links a {
  color: rgba(255,255,255,0.8);
  text-decoration: none;
}

.nav-links a:hover,
.nav-links a.router-link-active {
  color: #fff;
}

.cart-link b {
  display: inline-grid;
  place-items: center;
  min-width: 18px;
  height: 18px;
  padding: 0 4px;
  margin-left: 3px;
  border-radius: 20px;
  background: #0fe1d1;
  color: #24145f;
  font-size: 0.65rem;
}

.nav-links button {
  background: transparent;
  color: white;
  border: 1px solid rgba(255,255,255,0.55);
  padding: 0.45rem 0.8rem;
  cursor: pointer;
  border-radius: 999px;
  font-size: 0.75rem;
}

.main-content {
  min-height: calc(100vh - 120px);
  padding: 0 0 4rem;
}

.footer {
  background: #24145f;
  color: white;
  padding: 1.5rem 0;
  font-size: 0.76rem;
}

.footer-content div {
  display: flex;
  gap: 1rem;
}

.footer a {
  color: rgba(255,255,255,0.75);
  text-decoration: none;
}

@media (max-width: 760px) {
  .container {
    width: min(100% - 2rem, 1120px);
  }

  .nav-links {
    gap: 0.65rem;
    flex-wrap: wrap;
    justify-content: flex-end;
  }

  .nav-links a:nth-child(n+4) {
    display: none;
  }

  .footer-content {
    align-items: flex-start;
    gap: 1rem;
    flex-direction: column;
  }
}
</style>
