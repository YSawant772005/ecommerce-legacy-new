<template>
  <div id="app">
    <nav class="navbar">
      <div class="container">
        <router-link to="/" class="logo">NovaKart</router-link>
        <div class="nav-links">
          <router-link to="/products">Products</router-link>
          <router-link to="/contact">Contact</router-link>
          <router-link v-if="!authStore.isAuthenticated" to="/login">Login</router-link>
          <router-link v-if="!authStore.isAuthenticated" to="/register">Register</router-link>
          <router-link v-else to="/account">Account</router-link>
          <router-link v-if="authStore.isAuthenticated" to="/wishlist">Wishlist</router-link>
          <router-link v-if="authStore.isAdmin" to="/admin">Admin</router-link>
          <router-link v-if="authStore.isAuthenticated" to="/cart">Cart ({{ cartStore.itemCount }})</router-link>
          <button v-if="authStore.isAuthenticated" @click="authStore.logout">Logout</button>
        </div>
      </div>
    </nav>
    <main class="main-content">
      <router-view />
    </main>
    <footer class="footer">
      <div class="container">
        <p>&copy; 2024 NovaKart. Modernized E-commerce Platform.</p>
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
.navbar {
  background: #1a1a1a;
  padding: 1rem 0;
  color: white;
}

.container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 1rem;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.logo {
  font-size: 1.5rem;
  font-weight: bold;
  color: white;
  text-decoration: none;
}

.nav-links {
  display: flex;
  gap: 1.5rem;
  align-items: center;
}

.nav-links a {
  color: white;
  text-decoration: none;
}

.nav-links button {
  background: #e63946;
  color: white;
  border: none;
  padding: 0.5rem 1rem;
  cursor: pointer;
  border-radius: 4px;
}

.main-content {
  min-height: calc(100vh - 120px);
  padding: 2rem 0;
}

.footer {
  background: #1a1a1a;
  color: white;
  padding: 2rem 0;
  text-align: center;
}
</style>
