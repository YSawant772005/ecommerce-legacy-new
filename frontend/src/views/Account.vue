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
  min-height: calc(100vh - 100px);
  padding: 4rem max(1.5rem, calc((100% - 800px) / 2));
  margin: 0 auto;
  color: #24145f;
  background: #fbfaff;
}

.account h1 { margin: 0 0 2rem; font-size: clamp(2.7rem, 5vw, 4.5rem); letter-spacing: -.08em; }
.account-info {
  background: #fff;
  padding: 2rem;
  border: 1px solid #e8e3f1;
  border-radius: 16px;
  margin-bottom: 2rem;
  box-shadow: 0 12px 30px rgba(36,20,95,.06);
}

.account-info p {
  margin: 0.5rem 0;
}

.btn {
  display: inline-block;
  background: #281568;
  color: white;
  padding: 0.75rem 2rem;
  text-decoration: none;
  border-radius: 999px;
}
</style>
