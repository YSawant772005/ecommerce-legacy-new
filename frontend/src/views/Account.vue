<template>
  <div class="account">
    <h1>My Account</h1>
    <div class="account-info">
      <h2>Welcome, {{ authStore.user?.fullName }}</h2>
      <p><strong>Email:</strong> {{ authStore.user?.email }}</p>
      <p><strong>Phone:</strong> {{ authStore.user?.phone || 'Not provided' }}</p>
      <p><strong>Role:</strong> {{ authStore.user?.role }}</p>
    </div>
    <div class="account-links">
      <router-link to="/account/profile" class="btn">Edit Profile</router-link>
      <router-link to="/account/addresses" class="btn">Manage Addresses</router-link>
      <router-link to="/account/orders" class="btn">View Orders</router-link>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useAuthStore } from '../stores/auth'

const authStore = useAuthStore()
</script>

<style scoped>
.account-links {
  display: flex;
  gap: 1rem;
  flex-wrap: wrap;
}
</style>

<style scoped>
.account {
  padding: 2rem;
  max-width: 800px;
  margin: 0 auto;
}

.account-info {
  background: #f5f5f5;
  padding: 2rem;
  border-radius: 8px;
  margin-bottom: 2rem;
}

.account-info p {
  margin: 0.5rem 0;
}

.btn {
  display: inline-block;
  background: #667eea;
  color: white;
  padding: 0.75rem 2rem;
  text-decoration: none;
  border-radius: 4px;
}
</style>
