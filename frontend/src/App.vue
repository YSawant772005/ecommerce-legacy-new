<template>
  <div id="app" class="nova-app">
    <!-- Announcement Ticker -->
    <aside class="announcement-bar" aria-label="Store Announcements">
      <div class="ticker-track">
        <div class="ticker-content">
          <span>⚡ FREE EXPRESS DELIVERY ON ORDERS OVER ₹999</span>
          <span class="ticker-dot">•</span>
          <span>✦ 30-DAY HASSLE-FREE RETURNS</span>
          <span class="ticker-dot">•</span>
          <span>★ OVER 12,000+ VERIFIED 5-STAR REVIEWS</span>
          <span class="ticker-dot">•</span>
          <span>⚡ 100% SECURE 256-BIT ENCRYPTED CHECKOUT</span>
          <span class="ticker-dot">•</span>
          <span>✦ CURATED DESIGN OBJECTS & NEXT-GEN ESSENTIALS</span>
          <span class="ticker-dot">•</span>
        </div>
        <div class="ticker-content" aria-hidden="true">
          <span>⚡ FREE EXPRESS DELIVERY ON ORDERS OVER ₹999</span>
          <span class="ticker-dot">•</span>
          <span>✦ 30-DAY HASSLE-FREE RETURNS</span>
          <span class="ticker-dot">•</span>
          <span>★ OVER 12,000+ VERIFIED 5-STAR REVIEWS</span>
          <span class="ticker-dot">•</span>
          <span>⚡ 100% SECURE 256-BIT ENCRYPTED CHECKOUT</span>
          <span class="ticker-dot">•</span>
          <span>✦ CURATED DESIGN OBJECTS & NEXT-GEN ESSENTIALS</span>
          <span class="ticker-dot">•</span>
        </div>
      </div>
    </aside>

    <!-- Sticky Glass Navigation Header -->
    <header class="navbar-wrapper" :class="{ 'scrolled': isScrolled }">
      <div class="container nav-container">
        <!-- Logo -->
        <router-link to="/" class="brand-logo" aria-label="Nova Home">
          <span class="logo-mark">
            <svg width="22" height="22" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
              <circle cx="12" cy="12" r="10" stroke="currentColor" stroke-width="2" stroke-opacity="0.8"/>
              <path d="M12 6L14.5 9.5L18.5 12L14.5 14.5L12 18L9.5 14.5L5.5 12L9.5 9.5L12 6Z" fill="currentColor"/>
            </svg>
          </span>
          <span class="logo-text">Nova<span class="logo-accent">.</span></span>
        </router-link>

        <!-- Desktop Navigation Links -->
        <nav class="desktop-nav" aria-label="Main Navigation">
          <router-link to="/" class="nav-item">Home</router-link>
          <router-link to="/products" class="nav-item">Shop</router-link>
          <router-link to="/products?sort=popular" class="nav-item">Trending</router-link>
          <router-link to="/contact" class="nav-item">Contact</router-link>
          <router-link v-if="authStore.isAdmin" to="/admin" class="nav-item admin-pill">
            <span class="admin-indicator"></span> Admin
          </router-link>
        </nav>

        <!-- Right Utility Controls -->
        <div class="nav-actions">
          <!-- Quick Search -->
          <router-link to="/products" class="search-trigger" aria-label="Search Collection">
            <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
              <circle cx="11" cy="11" r="8"></circle>
              <line x1="21" y1="21" x2="16.65" y2="16.65"></line>
            </svg>
            <span class="search-hint">Search collection...</span>
          </router-link>

          <!-- Wishlist -->
          <router-link v-if="authStore.isAuthenticated" to="/wishlist" class="action-btn" aria-label="Wishlist">
            <svg width="19" height="19" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
              <path d="M20.84 4.61a5.5 5.5 0 0 0-7.78 0L12 5.67l-1.06-1.06a5.5 5.5 0 0 0-7.78 7.78l1.06 1.06L12 21.23l7.78-7.78 1.06-1.06a5.5 5.5 0 0 0 0-7.78z"></path>
            </svg>
            <span v-if="wishlistStore.items.length > 0" class="action-badge wishlist-badge">{{ wishlistStore.items.length }}</span>
          </router-link>

          <!-- Account Dropdown Trigger / Auth buttons -->
          <div class="account-control" v-if="authStore.isAuthenticated">
            <router-link to="/account" class="action-btn user-btn" aria-label="Account">
              <svg width="19" height="19" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                <path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"></path>
                <circle cx="12" cy="7" r="4"></circle>
              </svg>
              <span class="user-greeting">{{ authStore.user?.fullName?.split(' ')[0] || 'Account' }}</span>
            </router-link>
            <button @click="authStore.logout" class="logout-pill" title="Sign out" aria-label="Sign out">
              <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                <path d="M9 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h4"></path>
                <polyline points="16 17 21 12 16 7"></polyline>
                <line x1="21" y1="12" x2="9" y2="12"></line>
              </svg>
            </button>
          </div>
          <div v-else class="auth-group">
            <router-link to="/login" class="nav-login">Sign In</router-link>
            <router-link to="/register" class="nav-register">Join</router-link>
          </div>

          <!-- Cart Bag Trigger -->
          <router-link to="/cart" class="cart-pill-btn" aria-label="Shopping Bag">
            <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
              <path d="M6 2L3 6v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2V6l-3-2z"></path>
              <line x1="3" y1="6" x2="21" y2="6"></line>
              <path d="M16 10a4 4 0 0 1-8 0"></path>
            </svg>
            <span class="cart-label">Bag</span>
            <span class="cart-count-badge">{{ cartStore.itemCount }}</span>
          </router-link>

          <!-- Mobile Hamburger Toggle -->
          <button class="mobile-toggle" @click="mobileMenuOpen = !mobileMenuOpen" :aria-expanded="mobileMenuOpen" aria-label="Toggle navigation menu">
            <span class="hamburger-bar" :class="{ 'open-top': mobileMenuOpen }"></span>
            <span class="hamburger-bar" :class="{ 'open-mid': mobileMenuOpen }"></span>
            <span class="hamburger-bar" :class="{ 'open-bot': mobileMenuOpen }"></span>
          </button>
        </div>
      </div>

      <!-- Mobile Navigation Drawer -->
      <Transition name="drawer">
        <div v-if="mobileMenuOpen" class="mobile-drawer" @click.self="mobileMenuOpen = false">
          <div class="mobile-drawer-content">
            <div class="mobile-drawer-header">
              <router-link to="/" class="brand-logo" @click="mobileMenuOpen = false">
                <span class="logo-text">Nova<span class="logo-accent">.</span></span>
              </router-link>
              <button class="close-drawer-btn" @click="mobileMenuOpen = false" aria-label="Close menu">✕</button>
            </div>

            <div class="mobile-search-bar">
              <router-link to="/products" class="mobile-search-link" @click="mobileMenuOpen = false">
                <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                  <circle cx="11" cy="11" r="8"></circle>
                  <line x1="21" y1="21" x2="16.65" y2="16.65"></line>
                </svg>
                <span>Search all products...</span>
              </router-link>
            </div>

            <div class="mobile-nav-links">
              <router-link to="/" @click="mobileMenuOpen = false">Home</router-link>
              <router-link to="/products" @click="mobileMenuOpen = false">Shop All Collection</router-link>
              <router-link to="/contact" @click="mobileMenuOpen = false">Contact Support</router-link>
              
              <div class="mobile-divider"></div>

              <template v-if="authStore.isAuthenticated">
                <router-link to="/account" @click="mobileMenuOpen = false">My Account ({{ authStore.user?.fullName }})</router-link>
                <router-link to="/account/orders" @click="mobileMenuOpen = false">Order History</router-link>
                <router-link to="/wishlist" @click="mobileMenuOpen = false">Wishlist ({{ wishlistStore.items.length }})</router-link>
                <router-link to="/cart" @click="mobileMenuOpen = false">Bag ({{ cartStore.itemCount }})</router-link>
                <router-link v-if="authStore.isAdmin" to="/admin" class="admin-drawer-link" @click="mobileMenuOpen = false">
                  Admin Control Room ⚡
                </router-link>
                <button class="mobile-logout-btn" @click="logoutAndClose">Sign Out</button>
              </template>
              <template v-else>
                <router-link to="/login" class="mobile-btn-primary" @click="mobileMenuOpen = false">Sign In</router-link>
                <router-link to="/register" class="mobile-btn-outline" @click="mobileMenuOpen = false">Create Account</router-link>
              </template>
            </div>
          </div>
        </div>
      </Transition>
    </header>

    <!-- Main Viewport Router View -->
    <main class="main-viewport">
      <router-view />
    </main>

    <!-- Floating Global Toasts -->
    <ToastNotifications />

    <!-- Editorial Footer -->
    <footer class="editorial-footer">
      <div class="footer-top-strip">
        <div class="container footer-badges-grid">
          <div class="footer-badge-item">
            <div class="badge-icon">⚡</div>
            <div>
              <strong>Express Delivery</strong>
              <p>Free on all orders above ₹999</p>
            </div>
          </div>
          <div class="footer-badge-item">
            <div class="badge-icon">🛡️</div>
            <div>
              <strong>256-Bit Encryption</strong>
              <p>Guaranteed secure transactions</p>
            </div>
          </div>
          <div class="footer-badge-item">
            <div class="badge-icon">🔄</div>
            <div>
              <strong>30-Day Returns</strong>
              <p>Instant hassle-free policy</p>
            </div>
          </div>
          <div class="footer-badge-item">
            <div class="badge-icon">✦</div>
            <div>
              <strong>Certified Authentic</strong>
              <p>100% genuine curated products</p>
            </div>
          </div>
        </div>
      </div>

      <div class="container footer-body-grid">
        <div class="footer-brand-col">
          <div class="footer-logo">
            <span class="logo-mark small">
              <svg width="18" height="18" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                <circle cx="12" cy="12" r="10" stroke="currentColor" stroke-width="2"/>
                <path d="M12 6L14.5 9.5L18.5 12L14.5 14.5L12 18L9.5 14.5L5.5 12L9.5 9.5L12 6Z" fill="currentColor"/>
              </svg>
            </span>
            <span>Nova<span class="logo-accent">.</span></span>
          </div>
          <p class="brand-tagline">
            Next-generation essentials, precision sound, and timeless lifestyle objects designed for the way you live now.
          </p>
          <div class="social-links">
            <a href="https://instagram.com" target="_blank" rel="noopener" aria-label="Instagram">IG</a>
            <a href="https://twitter.com" target="_blank" rel="noopener" aria-label="Twitter">X</a>
            <a href="https://github.com" target="_blank" rel="noopener" aria-label="GitHub">GH</a>
          </div>
        </div>

        <div class="footer-links-col">
          <h4>Explore</h4>
          <router-link to="/products">All Products</router-link>
          <router-link to="/products?sort=popular">Popular Drops</router-link>
          <router-link to="/products?sort=new">New Arrivals</router-link>
          <router-link to="/contact">Brand Story</router-link>
        </div>

        <div class="footer-links-col">
          <h4>Client Care</h4>
          <router-link to="/contact">Help & Support</router-link>
          <router-link to="/account/orders">Track Orders</router-link>
          <router-link to="/wishlist">Saved Wishlist</router-link>
          <router-link to="/cart">Shopping Bag</router-link>
        </div>

        <div class="footer-links-col">
          <h4>Account</h4>
          <template v-if="authStore.isAuthenticated">
            <router-link to="/account">Profile Overview</router-link>
            <router-link to="/account/orders">My Orders</router-link>
            <router-link to="/account/addresses">Saved Addresses</router-link>
            <router-link v-if="authStore.isAdmin" to="/admin">Admin Dashboard</router-link>
          </template>
          <template v-else>
            <router-link to="/login">Sign In</router-link>
            <router-link to="/register">Create Account</router-link>
          </template>
        </div>
      </div>

      <div class="container footer-bottom">
        <p>&copy; 2026 NovaKart Inc. Designed with precision & intent.</p>
        <div class="footer-legal">
          <span>Privacy Policy</span>
          <span>•</span>
          <span>Terms of Service</span>
          <span>•</span>
          <span>All Rights Reserved</span>
        </div>
      </div>
    </footer>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted } from 'vue'
import { useAuthStore } from './stores/auth'
import { useCartStore } from './stores/cart'
import { useWishlistStore } from './stores/wishlist'
import ToastNotifications from './components/ToastNotifications.vue'

const authStore = useAuthStore()
const cartStore = useCartStore()
const wishlistStore = useWishlistStore()

const mobileMenuOpen = ref(false)
const isScrolled = ref(false)

function handleScroll() {
  isScrolled.value = window.scrollY > 20
}

function logoutAndClose() {
  authStore.logout()
  mobileMenuOpen.value = false
}

onMounted(() => {
  window.addEventListener('scroll', handleScroll, { passive: true })
  if (authStore.isAuthenticated) {
    cartStore.fetchCart()
    wishlistStore.fetchWishlist()
  }
})

onUnmounted(() => {
  window.removeEventListener('scroll', handleScroll)
})
</script>

<style scoped>
.nova-app {
  min-height: 100vh;
  display: flex;
  flex-direction: column;
  background-color: var(--bg-main);
  color: var(--text-primary);
}

/* Announcement Ticker */
.announcement-bar {
  background: linear-gradient(90deg, #100f1a 0%, #1e1338 50%, #100f1a 100%);
  color: var(--accent-lime-bright);
  border-bottom: 1px solid rgba(147, 51, 234, 0.2);
  overflow: hidden;
  height: 36px;
  display: flex;
  align-items: center;
  font-size: 0.72rem;
  font-weight: 700;
  letter-spacing: 0.08em;
  text-transform: uppercase;
}

.ticker-track {
  display: flex;
  white-space: nowrap;
  width: max-content;
  animation: marquee 35s linear infinite;
}

.ticker-content {
  display: flex;
  align-items: center;
  gap: 1.5rem;
  padding-right: 1.5rem;
}

.ticker-dot {
  color: var(--accent-purple-bright);
  opacity: 0.7;
}

/* Navbar */
.navbar-wrapper {
  position: sticky;
  top: 0;
  z-index: 100;
  background: rgba(10, 10, 15, 0.75);
  backdrop-filter: blur(24px);
  -webkit-backdrop-filter: blur(24px);
  border-bottom: 1px solid var(--border-subtle);
  transition: all var(--transition-base);
}

.navbar-wrapper.scrolled {
  background: rgba(10, 10, 15, 0.92);
  box-shadow: 0 10px 30px rgba(0, 0, 0, 0.5);
  border-bottom-color: rgba(255, 255, 255, 0.12);
}

.nav-container {
  display: flex;
  align-items: center;
  justify-content: space-between;
  height: 72px;
  gap: 1.5rem;
}

/* Brand Logo */
.brand-logo {
  display: inline-flex;
  align-items: center;
  gap: 0.65rem;
  text-decoration: none;
  color: #ffffff;
  font-family: var(--font-display);
  font-size: 1.5rem;
  font-weight: 900;
  letter-spacing: -0.04em;
  transition: transform var(--transition-fast);
}

.brand-logo:hover {
  transform: scale(1.02);
}

.logo-mark {
  display: grid;
  place-items: center;
  width: 34px;
  height: 34px;
  border-radius: 50%;
  background: linear-gradient(135deg, rgba(168, 85, 247, 0.25), rgba(34, 197, 94, 0.25));
  color: #ffffff;
  border: 1px solid rgba(255, 255, 255, 0.2);
  box-shadow: 0 0 16px rgba(168, 85, 247, 0.3);
}

.logo-accent {
  color: var(--accent-lime);
}

/* Desktop Navigation */
.desktop-nav {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.nav-item {
  position: relative;
  padding: 0.5rem 0.95rem;
  color: var(--text-secondary);
  text-decoration: none;
  font-size: 0.88rem;
  font-weight: 600;
  border-radius: var(--radius-pill);
  transition: all var(--transition-fast);
}

.nav-item:hover {
  color: #ffffff;
  background: rgba(255, 255, 255, 0.06);
}

.nav-item.router-link-active {
  color: #ffffff;
  background: rgba(255, 255, 255, 0.08);
}

.admin-pill {
  display: inline-flex;
  align-items: center;
  gap: 0.4rem;
  color: #fca5a5;
  background: rgba(239, 68, 68, 0.12);
  border: 1px solid rgba(239, 68, 68, 0.25);
}

.admin-pill:hover {
  background: rgba(239, 68, 68, 0.2);
  color: #ffffff;
}

.admin-indicator {
  width: 6px;
  height: 6px;
  border-radius: 50%;
  background: #ef4444;
  box-shadow: 0 0 8px #ef4444;
}

/* Right Nav Actions */
.nav-actions {
  display: flex;
  align-items: center;
  gap: 0.85rem;
}

.search-trigger {
  display: flex;
  align-items: center;
  gap: 0.65rem;
  padding: 0.5rem 1rem;
  background: rgba(255, 255, 255, 0.05);
  border: 1px solid var(--border-subtle);
  border-radius: var(--radius-pill);
  color: var(--text-muted);
  text-decoration: none;
  font-size: 0.82rem;
  font-weight: 500;
  transition: all var(--transition-fast);
}

.search-trigger:hover {
  border-color: rgba(255, 255, 255, 0.2);
  color: var(--text-secondary);
  background: rgba(255, 255, 255, 0.08);
}

.action-btn {
  position: relative;
  display: grid;
  place-items: center;
  width: 40px;
  height: 40px;
  border-radius: 50%;
  background: rgba(255, 255, 255, 0.04);
  border: 1px solid var(--border-subtle);
  color: var(--text-secondary);
  text-decoration: none;
  transition: all var(--transition-fast);
}

.action-btn:hover {
  background: rgba(255, 255, 255, 0.1);
  color: #ffffff;
  border-color: rgba(255, 255, 255, 0.2);
  transform: translateY(-1px);
}

.action-badge {
  position: absolute;
  top: -2px;
  right: -2px;
  min-width: 18px;
  height: 18px;
  padding: 0 4px;
  border-radius: 999px;
  font-size: 0.65rem;
  font-weight: 800;
  display: grid;
  place-items: center;
  background: var(--accent-pink);
  color: #ffffff;
  box-shadow: 0 0 8px rgba(236, 72, 153, 0.5);
}

.account-control {
  display: flex;
  align-items: center;
  gap: 0.35rem;
}

.user-btn {
  width: auto;
  padding: 0 0.85rem;
  border-radius: var(--radius-pill);
  gap: 0.5rem;
  display: flex;
}

.user-greeting {
  font-size: 0.82rem;
  font-weight: 600;
}

.logout-pill {
  width: 32px;
  height: 32px;
  display: grid;
  place-items: center;
  border-radius: 50%;
  background: transparent;
  border: 1px solid var(--border-subtle);
  color: var(--text-muted);
  cursor: pointer;
  transition: all var(--transition-fast);
}

.logout-pill:hover {
  background: rgba(239, 68, 68, 0.15);
  color: #f87171;
  border-color: rgba(239, 68, 68, 0.3);
}

.auth-group {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.nav-login {
  color: var(--text-secondary);
  text-decoration: none;
  font-size: 0.84rem;
  font-weight: 600;
  padding: 0.5rem 0.8rem;
  border-radius: var(--radius-pill);
  transition: color var(--transition-fast);
}

.nav-login:hover {
  color: #ffffff;
}

.nav-register {
  background: #ffffff;
  color: #0a0a0f;
  text-decoration: none;
  font-size: 0.84rem;
  font-weight: 700;
  padding: 0.5rem 1rem;
  border-radius: var(--radius-pill);
  transition: all var(--transition-fast);
}

.nav-register:hover {
  background: #f1f5f9;
  transform: translateY(-1px);
  box-shadow: 0 4px 15px rgba(255, 255, 255, 0.25);
}

/* Cart Bag Pill */
.cart-pill-btn {
  display: inline-flex;
  align-items: center;
  gap: 0.55rem;
  padding: 0.48rem 0.95rem;
  background: linear-gradient(135deg, rgba(255, 255, 255, 0.1) 0%, rgba(255, 255, 255, 0.04) 100%);
  border: 1px solid rgba(255, 255, 255, 0.15);
  border-radius: var(--radius-pill);
  color: #ffffff;
  text-decoration: none;
  font-size: 0.84rem;
  font-weight: 700;
  transition: all var(--transition-base);
}

.cart-pill-btn:hover {
  background: linear-gradient(135deg, rgba(168, 85, 247, 0.25) 0%, rgba(34, 197, 94, 0.15) 100%);
  border-color: rgba(168, 85, 247, 0.4);
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(168, 85, 247, 0.25);
}

.cart-count-badge {
  display: grid;
  place-items: center;
  min-width: 20px;
  height: 20px;
  padding: 0 5px;
  border-radius: 999px;
  background: var(--accent-lime);
  color: #052e16;
  font-size: 0.72rem;
  font-weight: 800;
  box-shadow: 0 0 10px var(--accent-lime-glow);
}

/* Mobile Hamburger Toggle */
.mobile-toggle {
  display: none;
  flex-direction: column;
  justify-content: space-around;
  width: 38px;
  height: 38px;
  padding: 8px;
  background: rgba(255, 255, 255, 0.05);
  border: 1px solid var(--border-subtle);
  border-radius: var(--radius-sm);
  cursor: pointer;
}

.hamburger-bar {
  width: 100%;
  height: 2px;
  background: #ffffff;
  border-radius: 2px;
  transition: all var(--transition-fast);
}

.hamburger-bar.open-top {
  transform: translateY(7px) rotate(45deg);
}

.hamburger-bar.open-mid {
  opacity: 0;
}

.hamburger-bar.open-bot {
  transform: translateY(-7px) rotate(-45deg);
}

/* Mobile Drawer */
.mobile-drawer {
  position: fixed;
  inset: 0;
  z-index: 1000;
  background: rgba(0, 0, 0, 0.75);
  backdrop-filter: blur(12px);
  -webkit-backdrop-filter: blur(12px);
  display: flex;
  justify-content: flex-end;
}

.mobile-drawer-content {
  width: min(340px, 85vw);
  height: 100%;
  background: var(--bg-surface);
  border-left: 1px solid var(--border-subtle);
  padding: 2rem 1.5rem;
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
  overflow-y: auto;
}

.mobile-drawer-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
}

.close-drawer-btn {
  background: rgba(255, 255, 255, 0.08);
  border: 0;
  color: #ffffff;
  width: 32px;
  height: 32px;
  border-radius: 50%;
  font-size: 1.1rem;
  cursor: pointer;
}

.mobile-search-bar {
  margin: 0.5rem 0;
}

.mobile-search-link {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  padding: 0.75rem 1rem;
  background: rgba(255, 255, 255, 0.05);
  border: 1px solid var(--border-subtle);
  border-radius: var(--radius-md);
  color: var(--text-secondary);
  text-decoration: none;
  font-size: 0.88rem;
}

.mobile-nav-links {
  display: flex;
  flex-direction: column;
  gap: 0.85rem;
}

.mobile-nav-links a {
  color: var(--text-primary);
  text-decoration: none;
  font-size: 1.05rem;
  font-weight: 600;
  padding: 0.5rem 0;
}

.mobile-divider {
  height: 1px;
  background: var(--border-subtle);
  margin: 0.5rem 0;
}

.admin-drawer-link {
  color: #f87171 !important;
}

.mobile-logout-btn {
  margin-top: 1rem;
  padding: 0.75rem;
  border-radius: var(--radius-md);
  background: rgba(239, 68, 68, 0.15);
  border: 1px solid rgba(239, 68, 68, 0.3);
  color: #fca5a5;
  font-weight: 700;
  cursor: pointer;
}

.mobile-btn-primary {
  display: block;
  text-align: center;
  padding: 0.75rem;
  background: #ffffff;
  color: #0a0a0f !important;
  border-radius: var(--radius-pill);
  font-weight: 700 !important;
}

.mobile-btn-outline {
  display: block;
  text-align: center;
  padding: 0.75rem;
  border: 1px solid var(--border-subtle);
  border-radius: var(--radius-pill);
  color: var(--text-primary) !important;
}

.drawer-enter-active,
.drawer-leave-active {
  transition: opacity 0.3s ease;
}

.drawer-enter-from,
.drawer-leave-to {
  opacity: 0;
}

.drawer-enter-active .mobile-drawer-content,
.drawer-leave-active .mobile-drawer-content {
  transition: transform 0.3s cubic-bezier(0.16, 1, 0.3, 1);
}

.drawer-enter-from .mobile-drawer-content {
  transform: translateX(100%);
}

.drawer-leave-to .mobile-drawer-content {
  transform: translateX(100%);
}

/* Main Viewport */
.main-viewport {
  flex: 1;
}

/* Editorial Footer */
.editorial-footer {
  margin-top: auto;
  background: #07070a;
  border-top: 1px solid var(--border-subtle);
  color: var(--text-secondary);
}

.footer-top-strip {
  border-bottom: 1px solid rgba(255, 255, 255, 0.05);
  padding: 2.5rem 0;
}

.footer-badges-grid {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 2rem;
}

.footer-badge-item {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.badge-icon {
  font-size: 1.5rem;
  display: grid;
  place-items: center;
  width: 44px;
  height: 44px;
  border-radius: 50%;
  background: rgba(255, 255, 255, 0.04);
  border: 1px solid var(--border-subtle);
  flex-shrink: 0;
}

.footer-badge-item strong {
  display: block;
  color: #ffffff;
  font-size: 0.92rem;
  font-weight: 700;
  margin-bottom: 0.15rem;
}

.footer-badge-item p {
  font-size: 0.78rem;
  color: var(--text-muted);
  margin: 0;
}

.footer-body-grid {
  display: grid;
  grid-template-columns: 2fr 1fr 1fr 1fr;
  gap: 3rem;
  padding: 4.5rem 0 3.5rem;
}

.footer-brand-col {
  max-width: 380px;
}

.footer-logo {
  display: flex;
  align-items: center;
  gap: 0.6rem;
  font-family: var(--font-display);
  font-size: 1.4rem;
  font-weight: 900;
  color: #ffffff;
  margin-bottom: 1.2rem;
}

.logo-mark.small {
  width: 28px;
  height: 28px;
}

.brand-tagline {
  font-size: 0.88rem;
  line-height: 1.65;
  color: var(--text-muted);
  margin-bottom: 1.8rem;
}

.social-links {
  display: flex;
  gap: 0.75rem;
}

.social-links a {
  display: grid;
  place-items: center;
  width: 36px;
  height: 36px;
  border-radius: 50%;
  border: 1px solid var(--border-subtle);
  background: rgba(255, 255, 255, 0.03);
  color: var(--text-secondary);
  text-decoration: none;
  font-size: 0.78rem;
  font-weight: 700;
  transition: all var(--transition-fast);
}

.social-links a:hover {
  border-color: rgba(255, 255, 255, 0.25);
  color: #ffffff;
  background: rgba(255, 255, 255, 0.08);
  transform: translateY(-2px);
}

.footer-links-col {
  display: flex;
  flex-direction: column;
  gap: 0.85rem;
}

.footer-links-col h4 {
  color: #ffffff;
  font-size: 0.95rem;
  font-weight: 700;
  margin-bottom: 0.6rem;
  letter-spacing: -0.01em;
}

.footer-links-col a {
  color: var(--text-muted);
  text-decoration: none;
  font-size: 0.85rem;
  transition: color var(--transition-fast);
}

.footer-links-col a:hover {
  color: #ffffff;
}

.footer-bottom {
  border-top: 1px solid rgba(255, 255, 255, 0.05);
  padding: 2rem 0;
  display: flex;
  align-items: center;
  justify-content: space-between;
  font-size: 0.8rem;
  color: var(--text-muted);
}

.footer-legal {
  display: flex;
  gap: 0.85rem;
  align-items: center;
}

/* Responsive Breakpoints */
@media (max-width: 1024px) {
  .footer-badges-grid {
    grid-template-columns: repeat(2, 1fr);
    gap: 1.5rem;
  }
  .footer-body-grid {
    grid-template-columns: 1fr 1fr;
    gap: 2.5rem;
  }
}

@media (max-width: 768px) {
  .desktop-nav,
  .search-trigger .search-hint,
  .user-greeting {
    display: none;
  }

  .mobile-toggle {
    display: flex;
  }

  .footer-badges-grid {
    grid-template-columns: 1fr;
  }

  .footer-body-grid {
    grid-template-columns: 1fr;
    gap: 2rem;
    padding: 3rem 0;
  }

  .footer-bottom {
    flex-direction: column;
    gap: 1rem;
    text-align: center;
  }
}
</style>
