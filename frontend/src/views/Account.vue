<template>
  <div class="account-page">
    <div class="container">
      <header class="account-header">
        <div class="account-eyebrow">
          <span class="eyebrow-dot"></span>
          <span>MEMBERSHIP PORTAL</span>
        </div>
        <h1 class="account-title">
          Welcome back,<br />
          <span class="name-gradient">{{ authStore.user?.fullName || 'Member' }}</span>
        </h1>
      </header>

      <div class="account-layout-grid">
        <!-- Sidebar Navigation -->
        <aside class="account-nav-rail glass-panel">
          <router-link to="/account" class="nav-rail-item active">
            <span class="item-icon">✦</span>
            <span>Account Overview</span>
            <span class="rail-arrow">→</span>
          </router-link>
          <router-link to="/account/orders" class="nav-rail-item">
            <span class="item-icon">📦</span>
            <span>Order History</span>
            <span class="rail-arrow">→</span>
          </router-link>
          <router-link to="/account/profile" class="nav-rail-item">
            <span class="item-icon">👤</span>
            <span>Personal Profile</span>
            <span class="rail-arrow">→</span>
          </router-link>
          <router-link to="/account/addresses" class="nav-rail-item">
            <span class="item-icon">📍</span>
            <span>Saved Addresses</span>
            <span class="rail-arrow">→</span>
          </router-link>
          <router-link to="/wishlist" class="nav-rail-item">
            <span class="item-icon">♡</span>
            <span>Wishlist Vault</span>
            <span class="rail-arrow">→</span>
          </router-link>
          <router-link v-if="authStore.isAdmin" to="/admin" class="nav-rail-item admin-item">
            <span class="item-icon">⚡</span>
            <span>Admin Control Room</span>
            <span class="rail-arrow">→</span>
          </router-link>
        </aside>

        <!-- Main Account Overview Content -->
        <section class="account-main-content">
          <!-- Member Profile Banner Card -->
          <div class="profile-hero-card glass-panel">
            <div class="avatar-large">
              {{ authStore.user?.fullName?.charAt(0) || 'U' }}
            </div>
            <div class="profile-meta-info">
              <span class="member-status-pill">
                <span class="pill-dot"></span> Verified Member
              </span>
              <h2>{{ authStore.user?.fullName }}</h2>
              <p class="member-email">{{ authStore.user?.email }}</p>
              <span v-if="authStore.user?.role" class="member-role-badge">Role: {{ authStore.user?.role }}</span>
            </div>
          </div>

          <!-- Fast Shortcut Cards Grid -->
          <div class="shortcut-cards-grid">
            <router-link to="/account/orders" class="shortcut-tile glass-card">
              <span class="shortcut-number">01</span>
              <div class="shortcut-text">
                <h3>Order History</h3>
                <p>Track, manage, and review previous purchases.</p>
              </div>
              <span class="shortcut-link-text">Explore orders →</span>
            </router-link>

            <router-link to="/account/addresses" class="shortcut-tile glass-card">
              <span class="shortcut-number">02</span>
              <div class="shortcut-text">
                <h3>Saved Addresses</h3>
                <p>Manage shipping locations for one-click checkout.</p>
              </div>
              <span class="shortcut-link-text">Manage addresses →</span>
            </router-link>

            <router-link to="/account/profile" class="shortcut-tile glass-card">
              <span class="shortcut-number">03</span>
              <div class="shortcut-text">
                <h3>Profile & Security</h3>
                <p>Update personal credentials and password security.</p>
              </div>
              <span class="shortcut-link-text">Edit settings →</span>
            </router-link>
          </div>
        </section>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useAuthStore } from '../stores/auth'

const authStore = useAuthStore()
</script>

<style scoped>
.account-page {
  min-height: 100vh;
  padding: 3.5rem 0 6rem;
  background-color: var(--bg-main);
  color: var(--text-primary);
}

.account-header {
  margin-bottom: 3rem;
}

.account-eyebrow {
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  font-size: 0.75rem;
  font-weight: 800;
  letter-spacing: 0.15em;
  color: var(--accent-lime);
  margin-bottom: 0.8rem;
}

.eyebrow-dot {
  width: 6px;
  height: 6px;
  border-radius: 50%;
  background: var(--accent-lime);
}

.account-title {
  font-family: var(--font-display);
  font-size: clamp(2.8rem, 5.5vw, 4.4rem);
  font-weight: 900;
  line-height: 0.95;
  letter-spacing: -0.04em;
  color: #ffffff;
}

.name-gradient {
  background: linear-gradient(135deg, #ffffff 40%, #a855f7 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
}

/* Layout */
.account-layout-grid {
  display: grid;
  grid-template-columns: 260px 1fr;
  gap: 3.5rem;
  align-items: flex-start;
}

/* Navigation Rail */
.account-nav-rail {
  padding: 1.25rem;
  border-radius: var(--radius-xl);
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.nav-rail-item {
  display: flex;
  align-items: center;
  gap: 0.85rem;
  padding: 0.85rem 1.1rem;
  border-radius: var(--radius-md);
  color: var(--text-secondary);
  text-decoration: none;
  font-size: 0.88rem;
  font-weight: 600;
  transition: all var(--transition-fast);
}

.nav-rail-item:hover {
  background: rgba(255, 255, 255, 0.06);
  color: #ffffff;
  transform: translateX(3px);
}

.nav-rail-item.active {
  background: #ffffff;
  color: #0a0a0f;
  font-weight: 700;
}

.rail-arrow {
  margin-left: auto;
  opacity: 0.4;
  font-size: 0.82rem;
}

.nav-rail-item.active .rail-arrow {
  opacity: 1;
}

.admin-item {
  color: #fca5a5 !important;
  background: rgba(239, 68, 68, 0.1);
  border: 1px solid rgba(239, 68, 68, 0.2);
}

/* Profile Hero */
.profile-hero-card {
  display: flex;
  align-items: center;
  gap: 2rem;
  padding: 2.5rem;
  border-radius: var(--radius-xl);
  margin-bottom: 2rem;
}

.avatar-large {
  width: 76px;
  height: 76px;
  border-radius: 50%;
  background: linear-gradient(135deg, #a855f7, #22c55e);
  color: #ffffff;
  font-size: 2.2rem;
  font-weight: 850;
  display: grid;
  place-items: center;
  box-shadow: 0 0 25px rgba(168, 85, 247, 0.35);
  flex-shrink: 0;
}

.profile-meta-info {
  display: flex;
  flex-direction: column;
  gap: 0.35rem;
}

.member-status-pill {
  display: inline-flex;
  align-items: center;
  gap: 0.45rem;
  font-size: 0.72rem;
  font-weight: 800;
  color: var(--accent-lime);
  text-transform: uppercase;
  letter-spacing: 0.08em;
}

.pill-dot {
  width: 6px;
  height: 6px;
  border-radius: 50%;
  background: var(--accent-lime);
  box-shadow: 0 0 8px var(--accent-lime);
}

.profile-meta-info h2 {
  font-family: var(--font-display);
  font-size: 1.8rem;
  font-weight: 850;
  color: #ffffff;
  letter-spacing: -0.02em;
}

.member-email {
  color: var(--text-muted);
  font-size: 0.92rem;
  margin: 0;
}

.member-role-badge {
  font-size: 0.72rem;
  color: var(--text-secondary);
  font-weight: 700;
  text-transform: uppercase;
}

/* Shortcuts */
.shortcut-cards-grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 1.5rem;
}

.shortcut-tile {
  padding: 2rem;
  border-radius: var(--radius-lg);
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  min-height: 190px;
  text-decoration: none;
  color: inherit;
  transition: all var(--transition-base);
}

.shortcut-tile:hover {
  transform: translateY(-5px);
  border-color: rgba(255, 255, 255, 0.25);
}

.shortcut-number {
  font-size: 0.8rem;
  font-weight: 800;
  color: var(--accent-purple-bright);
}

.shortcut-text h3 {
  font-size: 1.15rem;
  font-weight: 750;
  color: #ffffff;
  margin-bottom: 0.35rem;
}

.shortcut-text p {
  font-size: 0.82rem;
  color: var(--text-muted);
  margin: 0;
  line-height: 1.5;
}

.shortcut-link-text {
  font-size: 0.82rem;
  font-weight: 700;
  color: var(--accent-lime);
}

@media (max-width: 992px) {
  .account-layout-grid {
    grid-template-columns: 1fr;
  }
  .shortcut-cards-grid {
    grid-template-columns: 1fr;
  }
}
</style>
