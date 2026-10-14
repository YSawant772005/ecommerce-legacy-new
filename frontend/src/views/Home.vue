<template>
  <div class="home-page">
    <!-- Hero Section: Inspired by Reference Mockups 1 & 2 -->
    <section class="hero-section" aria-label="Hero Spotlight">
      <div class="hero-ambient-glow" aria-hidden="true"></div>
      <div class="hero-ambient-glow-secondary" aria-hidden="true"></div>

      <div class="container hero-container">
        <!-- Left Hero Typography & CTA -->
        <div class="hero-content">
          <div class="hero-badge-pill">
            <span class="pulse-dot"></span>
            <span>NEXT-GEN COLLECTION • FALL / WINTER 2026</span>
          </div>

          <h1 class="hero-title">
            <span class="sr-only">NovaKart — </span>
            Bigger.<br />
            Better.<br />
            <span class="highlight-word">Bolder<span class="dot-accent">.</span></span>
          </h1>

          <p class="hero-subtitle">
            Next-generation essentials, precision sound, and timeless objects. Designed for what's next in modern living.
          </p>

          <div class="hero-actions">
            <router-link to="/products" class="btn-pill btn-pill-primary hero-btn">
              <span>Shop the Drop</span>
              <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
                <line x1="5" y1="12" x2="19" y2="12"></line>
                <polyline points="12 5 19 12 12 19"></polyline>
              </svg>
            </router-link>

            <router-link to="/products?sort=popular" class="btn-pill btn-pill-outline hero-btn-secondary">
              <span class="play-icon">▶</span>
              <span>Explore Edit</span>
            </router-link>
          </div>

          <!-- Social Proof + Metrics Strip -->
          <div class="hero-proof-strip">
            <div class="customer-avatars">
              <img src="https://images.unsplash.com/photo-1534528741775-53994a69daeb?auto=format&fit=crop&w=120&q=80" alt="Customer avatar" />
              <img src="https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?auto=format&fit=crop&w=120&q=80" alt="Customer avatar" />
              <img src="https://images.unsplash.com/photo-1494790108377-be9c29b29330?auto=format&fit=crop&w=120&q=80" alt="Customer avatar" />
              <div class="avatar-count">+12k</div>
            </div>
            <div class="proof-text">
              <strong>12,400+ Verified Buyers</strong>
              <span>Rated 4.9/5 from global community</span>
            </div>
          </div>
        </div>

        <!-- Right Hero Visual: 3D Product Presentation with Floating Widgets -->
        <div class="hero-visual">
          <div class="sculptural-ring" aria-hidden="true"></div>
          
          <div class="hero-image-wrapper animate-float">
            <img 
              src="https://images.unsplash.com/photo-1505740420928-5e560c06d30e?auto=format&fit=crop&w=1000&q=90" 
              alt="Nova Premium Sound Headphones" 
              class="hero-product-img"
            />
          </div>

          <!-- Floating Widget 1: Trending Indicator -->
          <div class="floating-widget trending-widget">
            <div class="trending-icon-box">
              <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
                <polyline points="23 6 13.5 15.5 8.5 10.5 1 18"></polyline>
                <polyline points="17 6 23 6 23 12"></polyline>
              </svg>
            </div>
            <div>
              <span class="widget-label">Trending Now</span>
              <strong class="widget-stat">+148% this week</strong>
            </div>
          </div>

          <!-- Floating Widget 2: Limited Edition Pill -->
          <div class="floating-widget edition-widget">
            <div class="edition-badge">LIMITED EDITION</div>
            <div class="edition-title">Air Soundcraft Gen 4</div>
            <div class="edition-price">₹12,999 <span class="edition-discount">23% OFF</span></div>
          </div>
        </div>
      </div>

      <!-- Trust Ticker Bar -->
      <div class="hero-trust-bar">
        <div class="container trust-bar-grid">
          <div class="trust-item">
            <svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
              <rect x="1" y="3" width="15" height="13"></rect>
              <polygon points="16 8 20 8 23 11 23 16 16 16 16 8"></polygon>
              <circle cx="5.5" cy="18.5" r="2.5"></circle>
              <circle cx="18.5" cy="18.5" r="2.5"></circle>
            </svg>
            <div>
              <strong>Free Express Shipping</strong>
              <span>On all orders over ₹999</span>
            </div>
          </div>

          <div class="trust-item">
            <svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
              <rect x="3" y="11" width="18" height="11" rx="2" ry="2"></rect>
              <path d="M7 11V7a5 5 0 0 1 10 0v4"></path>
            </svg>
            <div>
              <strong>Secure Payments</strong>
              <span>256-bit encrypted checkout</span>
            </div>
          </div>

          <div class="trust-item">
            <svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
              <polyline points="1 4 1 10 7 10"></polyline>
              <path d="M3.51 15a9 9 0 1 0 2.13-9.36L1 10"></path>
            </svg>
            <div>
              <strong>Easy Returns</strong>
              <span>Within 30 days hassle-free</span>
            </div>
          </div>

          <div class="trust-item">
            <svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
              <path d="M12 22s8-4 8-10V5l-8-3-8 3v7c0 6 8 10 8 10z"></path>
            </svg>
            <div>
              <strong>1-Year Warranty</strong>
              <span>Official coverage included</span>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Curated Editorial Spotlight: Inspired by Reference Mockup 3 -->
    <section class="section editorial-spotlight-section">
      <div class="container">
        <div class="editorial-banner-card">
          <div class="editorial-content">
            <span class="editorial-eyebrow">CURATED PICKS</span>
            <h2 class="editorial-title">
              Live <em class="serif-accent">Beautifully.</em>
            </h2>
            <p class="editorial-desc">
              Thoughtful designs, sculptural ergonomics, and timeless style. Hand-selected for intentional spaces.
            </p>
            <router-link to="/products" class="btn-pill btn-pill-primary">
              <span>Explore All Picks</span>
              <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                <line x1="5" y1="12" x2="19" y2="12"></line>
                <polyline points="12 5 19 12 12 19"></polyline>
              </svg>
            </router-link>
          </div>

          <!-- Live Curated Product Cards -->
          <div class="editorial-showcase-grid" v-if="curatedProducts.length > 0">
            <div 
              v-for="(product, idx) in curatedProducts" 
              :key="product.id" 
              class="curated-mini-card"
              :class="`curated-card-${idx}`"
            >
              <div class="mini-card-header">
                <span class="curated-tag" :class="idx === 0 ? 'tag-new' : idx === 1 ? 'tag-best' : 'tag-trend'">
                  {{ idx === 0 ? 'New' : idx === 1 ? 'Bestseller' : 'Trending' }}
                </span>
                <button 
                  class="mini-wishlist-btn" 
                  :class="{ 'in-wishlist': isWishlisted(product.id) }" 
                  @click="toggleWishlist(product)"
                  :aria-label="`Save ${product.name}`"
                >
                  <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                    <path d="M20.84 4.61a5.5 5.5 0 0 0-7.78 0L12 5.67l-1.06-1.06a5.5 5.5 0 0 0-7.78 7.78l1.06 1.06L12 21.23l7.78-7.78 1.06-1.06a5.5 5.5 0 0 0 0-7.78z"></path>
                  </svg>
                </button>
              </div>

              <router-link :to="`/products/${product.id}`" class="mini-card-image-wrap">
                <img :src="productImage(product)" :alt="product.name" loading="lazy" />
              </router-link>

              <div class="mini-card-body">
                <h3>{{ product.name }}</h3>
                <div class="mini-price-row">
                  <span class="current-price">₹{{ product.price.toLocaleString('en-IN') }}</span>
                  <span v-if="product.comparePrice" class="original-price">₹{{ product.comparePrice.toLocaleString('en-IN') }}</span>
                </div>
                <div class="mini-rating">
                  ★ {{ product.rating.toFixed(1) }} <span class="rating-count">({{ product.reviewCount }})</span>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Category Discovery Showcase -->
    <section class="section categories-section">
      <div class="container">
        <div class="section-heading-row">
          <div>
            <span class="category-eyebrow">BROWSE BY MOOD</span>
            <h2 class="section-heading-title">
              Find your next <em class="serif-accent">favourite.</em>
            </h2>
          </div>
          <router-link to="/products" class="view-all-link">
            <span>View all categories</span>
            <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
              <line x1="7" y1="17" x2="17" y2="7"></line>
              <polyline points="7 7 17 7 17 17"></polyline>
            </svg>
          </router-link>
        </div>

        <div class="categories-grid" v-if="!catalogStore.loading && catalogStore.categories.length > 0">
          <router-link 
            v-for="(cat, index) in catalogStore.categories.slice(0, 6)" 
            :key="cat.id" 
            :to="`/products?categoryId=${cat.id}`"
            class="category-tile"
            :class="`tile-style-${index % 3}`"
          >
            <div class="category-tile-bg"></div>
            <div class="category-tile-content">
              <span class="category-index">0{{ index + 1 }}</span>
              <div class="category-text-block">
                <h3>{{ cat.name }}</h3>
                <p>{{ cat.tagline || 'Curated essentials' }}</p>
              </div>
              <div class="category-arrow-circle">
                <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
                  <line x1="7" y1="17" x2="17" y2="7"></line>
                  <polyline points="7 7 17 7 17 17"></polyline>
                </svg>
              </div>
            </div>
          </router-link>
        </div>

        <!-- Skeleton loader -->
        <div v-else-if="catalogStore.loading" class="categories-grid">
          <div v-for="n in 6" :key="n" class="category-tile skeleton-shimmer" style="height: 160px;"></div>
        </div>
      </div>
    </section>

    <!-- Featured Drops Grid -->
    <section class="section featured-drops-section">
      <div class="container">
        <div class="section-heading-row">
          <div>
            <span class="category-eyebrow neon-green">JUST IN & SPOTLIGHT</span>
            <h2 class="section-heading-title">
              Objects with <em class="serif-accent">character.</em>
            </h2>
          </div>
          <router-link to="/products" class="view-all-link">
            <span>Shop complete collection</span>
            <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
              <line x1="7" y1="17" x2="17" y2="7"></line>
              <polyline points="7 7 17 7 17 17"></polyline>
            </svg>
          </router-link>
        </div>

        <div class="product-cards-grid" v-if="!catalogStore.loading && featuredProducts.length > 0">
          <article 
            v-for="product in featuredProducts" 
            :key="product.id" 
            class="product-card-wrap glass-card"
          >
            <div class="card-image-box">
              <span v-if="product.isNew" class="badge-pill badge-lime card-pill">New</span>
              <span v-else-if="product.discountPercent > 0" class="badge-pill badge-discount card-pill">{{ product.discountPercent }}% OFF</span>
              
              <button 
                class="card-wishlist-action" 
                :class="{ 'active': isWishlisted(product.id) }" 
                @click="toggleWishlist(product)"
                :aria-label="`Wishlist ${product.name}`"
              >
                <svg width="17" height="17" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                  <path d="M20.84 4.61a5.5 5.5 0 0 0-7.78 0L12 5.67l-1.06-1.06a5.5 5.5 0 0 0-7.78 7.78l1.06 1.06L12 21.23l7.78-7.78 1.06-1.06a5.5 5.5 0 0 0 0-7.78z"></path>
                </svg>
              </button>

              <router-link :to="`/products/${product.id}`" class="image-aspect-link">
                <img :src="productImage(product)" :alt="product.name" loading="lazy" />
              </router-link>
            </div>

            <div class="card-details-box">
              <div class="card-topline">
                <span class="product-category-name">{{ product.categoryName || 'NovaKart Edit' }}</span>
                <span class="product-stars">★ {{ product.rating.toFixed(1) }}</span>
              </div>

              <h3 class="product-card-name">
                <router-link :to="`/products/${product.id}`">{{ product.name }}</router-link>
              </h3>

              <div class="card-bottom-row">
                <div class="price-stack">
                  <span class="main-price">₹{{ product.price.toLocaleString('en-IN') }}</span>
                  <span v-if="product.comparePrice" class="strike-price">₹{{ product.comparePrice.toLocaleString('en-IN') }}</span>
                </div>

                <button 
                  class="add-to-bag-button"
                  :class="{ 'is-loading': cartStore.isAdding(product.id) }"
                  :disabled="!product.inStock || cartStore.isAdding(product.id)"
                  @click="addToCart(product)"
                  :aria-label="`Add ${product.name} to bag`"
                >
                  <span v-if="cartStore.isAdding(product.id)">…</span>
                  <svg v-else width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
                    <line x1="12" y1="5" x2="12" y2="19"></line>
                    <line x1="5" y1="12" x2="19" y2="12"></line>
                  </svg>
                </button>
              </div>
            </div>
          </article>
        </div>

        <!-- Skeleton loader -->
        <div v-else-if="catalogStore.loading" class="product-cards-grid">
          <div v-for="n in 8" :key="n" class="skeleton-shimmer" style="height: 380px; border-radius: var(--radius-md);"></div>
        </div>
      </div>
    </section>

    <!-- Newsletter Component Upgrade -->
    <Newsletter />
  </div>
</template>

<script setup lang="ts">
import { computed, onMounted } from 'vue'
import { useCatalogStore } from '../stores/catalog'
import { useCartStore } from '../stores/cart'
import { useWishlistStore } from '../stores/wishlist'
import { useAuthStore } from '../stores/auth'
import Newsletter from './Newsletter.vue'
import { productImage } from '../utils/productImage'
import type { Product } from '../types'

const catalogStore = useCatalogStore()
const cartStore = useCartStore()
const wishlistStore = useWishlistStore()
const authStore = useAuthStore()

const featuredProducts = computed(() => {
  return catalogStore.products.filter((p: Product) => p.isFeatured || p.isDeal).slice(0, 8)
})

const curatedProducts = computed(() => {
  return catalogStore.products.slice(0, 3)
})

function addToCart(product: Product) {
  cartStore.addToCart(product.id, 1, undefined, product.name)
}

function isWishlisted(productId: number) {
  return wishlistStore.items.some((item: any) => item.productId === productId)
}

function toggleWishlist(product: Product) {
  if (!authStore.isAuthenticated) {
    window.location.href = '/login'
    return
  }
  wishlistStore.toggleWishlist(product.id)
}

onMounted(() => {
  catalogStore.fetchCategories()
  catalogStore.fetchProducts({ pageSize: 24 })
})
</script>

<style scoped>
.home-page {
  background-color: var(--bg-main);
  color: var(--text-primary);
  overflow-x: hidden;
}

/* ==========================================================================
   Hero Section Styling
   ========================================================================== */
.hero-section {
  position: relative;
  min-height: calc(100vh - 108px);
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  padding: 4rem 0 0;
  overflow: hidden;
}

.hero-ambient-glow {
  position: absolute;
  top: -10%;
  right: -5%;
  width: 750px;
  height: 750px;
  background: radial-gradient(circle, rgba(147, 51, 234, 0.28) 0%, rgba(34, 197, 94, 0.08) 45%, transparent 70%);
  filter: blur(80px);
  pointer-events: none;
  z-index: 0;
}

.hero-ambient-glow-secondary {
  position: absolute;
  bottom: 10%;
  left: -10%;
  width: 600px;
  height: 600px;
  background: radial-gradient(circle, rgba(56, 189, 248, 0.12) 0%, rgba(147, 51, 234, 0.08) 50%, transparent 70%);
  filter: blur(90px);
  pointer-events: none;
  z-index: 0;
}

.hero-container {
  position: relative;
  z-index: 1;
  display: grid;
  grid-template-columns: 1.15fr 0.85fr;
  align-items: center;
  gap: 3.5rem;
  padding-bottom: 4rem;
}

.hero-content {
  max-width: 640px;
}

.hero-badge-pill {
  display: inline-flex;
  align-items: center;
  gap: 0.6rem;
  padding: 0.35rem 0.95rem;
  background: rgba(255, 255, 255, 0.05);
  border: 1px solid rgba(255, 255, 255, 0.12);
  border-radius: var(--radius-pill);
  font-size: 0.75rem;
  font-weight: 700;
  letter-spacing: 0.08em;
  color: var(--accent-lime-bright);
  margin-bottom: 2rem;
}

.pulse-dot {
  width: 8px;
  height: 8px;
  border-radius: 50%;
  background: var(--accent-lime);
  box-shadow: 0 0 10px var(--accent-lime);
  animation: pulseGlow 2s infinite;
}

.hero-title {
  font-family: var(--font-display);
  font-size: clamp(3.8rem, 7.5vw, 6.8rem);
  font-weight: 900;
  line-height: 0.92;
  letter-spacing: -0.05em;
  color: #ffffff;
  margin-bottom: 1.8rem;
}

.highlight-word {
  background: linear-gradient(135deg, #ffffff 30%, #a855f7 70%, #22c55e 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
}

.dot-accent {
  -webkit-text-fill-color: var(--accent-lime);
}

.hero-subtitle {
  font-size: 1.15rem;
  line-height: 1.6;
  color: var(--text-secondary);
  max-width: 480px;
  margin-bottom: 2.5rem;
}

.hero-actions {
  display: flex;
  align-items: center;
  gap: 1.2rem;
  margin-bottom: 3.5rem;
}

.hero-btn {
  padding: 1rem 2rem;
  font-size: 0.98rem;
}

.hero-btn-secondary {
  padding: 1rem 1.8rem;
  font-size: 0.98rem;
}

.play-icon {
  font-size: 0.75rem;
  color: var(--accent-lime);
}

.hero-proof-strip {
  display: flex;
  align-items: center;
  gap: 1.2rem;
  padding-top: 1.8rem;
  border-top: 1px solid var(--border-subtle);
}

.customer-avatars {
  display: flex;
  align-items: center;
}

.customer-avatars img {
  width: 38px;
  height: 38px;
  border-radius: 50%;
  object-fit: cover;
  border: 2px solid var(--bg-main);
  margin-left: -10px;
}

.customer-avatars img:first-child {
  margin-left: 0;
}

.avatar-count {
  width: 38px;
  height: 38px;
  border-radius: 50%;
  background: linear-gradient(135deg, #9333ea, #a855f7);
  color: #ffffff;
  font-size: 0.72rem;
  font-weight: 800;
  display: grid;
  place-items: center;
  border: 2px solid var(--bg-main);
  margin-left: -10px;
}

.proof-text {
  display: flex;
  flex-direction: column;
  gap: 0.15rem;
}

.proof-text strong {
  font-size: 0.88rem;
  color: #ffffff;
}

.proof-text span {
  font-size: 0.78rem;
  color: var(--text-muted);
}

/* Right Hero Visual */
.hero-visual {
  position: relative;
  min-height: 520px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.sculptural-ring {
  position: absolute;
  width: 440px;
  height: 440px;
  border-radius: 50%;
  border: 1.5px solid rgba(168, 85, 247, 0.4);
  box-shadow: 0 0 60px rgba(168, 85, 247, 0.25), inset 0 0 40px rgba(34, 197, 94, 0.15);
  animation: pulseGlow 6s ease-in-out infinite;
}

.hero-image-wrapper {
  position: relative;
  z-index: 2;
  width: 100%;
  max-width: 460px;
  border-radius: var(--radius-xl);
  overflow: hidden;
  box-shadow: 0 30px 80px rgba(0, 0, 0, 0.6), 0 0 50px rgba(168, 85, 247, 0.2);
}

.hero-product-img {
  width: 100%;
  height: auto;
  display: block;
  object-fit: cover;
  transform: scale(1.02);
  transition: transform var(--transition-smooth);
}

.hero-visual:hover .hero-product-img {
  transform: scale(1.06);
}

/* Floating Badges on Hero */
.floating-widget {
  position: absolute;
  z-index: 3;
  padding: 0.85rem 1.2rem;
  background: rgba(18, 17, 27, 0.85);
  backdrop-filter: blur(20px);
  -webkit-backdrop-filter: blur(20px);
  border: 1px solid var(--border-medium);
  border-radius: var(--radius-md);
  box-shadow: 0 20px 40px rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  gap: 0.85rem;
}

.trending-widget {
  bottom: 8%;
  left: -5%;
}

.trending-icon-box {
  width: 36px;
  height: 36px;
  border-radius: 50%;
  background: rgba(34, 197, 94, 0.2);
  color: var(--accent-lime);
  display: grid;
  place-items: center;
}

.widget-label {
  display: block;
  font-size: 0.72rem;
  color: var(--text-muted);
  text-transform: uppercase;
  letter-spacing: 0.05em;
  font-weight: 700;
}

.widget-stat {
  font-size: 0.92rem;
  color: #ffffff;
}

.edition-widget {
  top: 6%;
  right: -5%;
  flex-direction: column;
  align-items: flex-start;
  gap: 0.35rem;
  min-width: 180px;
}

.edition-badge {
  font-size: 0.65rem;
  font-weight: 800;
  letter-spacing: 0.08em;
  color: var(--accent-purple-bright);
}

.edition-title {
  font-size: 0.88rem;
  font-weight: 700;
  color: #ffffff;
}

.edition-price {
  font-size: 0.85rem;
  font-weight: 800;
  color: var(--accent-lime-bright);
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.edition-discount {
  font-size: 0.65rem;
  background: var(--accent-lime);
  color: #052e16;
  padding: 0.15rem 0.4rem;
  border-radius: var(--radius-pill);
}

/* Trust Ticker Bar */
.hero-trust-bar {
  border-top: 1px solid var(--border-subtle);
  border-bottom: 1px solid var(--border-subtle);
  background: rgba(14, 13, 22, 0.6);
  padding: 1.8rem 0;
}

.trust-bar-grid {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 2rem;
}

.trust-item {
  display: flex;
  align-items: center;
  gap: 1rem;
  color: #ffffff;
}

.trust-item svg {
  color: var(--accent-purple-bright);
  flex-shrink: 0;
}

.trust-item strong {
  display: block;
  font-size: 0.88rem;
  font-weight: 700;
}

.trust-item span {
  font-size: 0.78rem;
  color: var(--text-muted);
}

/* ==========================================================================
   Editorial Spotlight Section (Inspired by Mockup 3)
   ========================================================================== */
.editorial-spotlight-section {
  padding: 6rem 0;
}

.editorial-banner-card {
  background: linear-gradient(135deg, #1b1629 0%, #291a38 50%, #15222e 100%);
  border: 1px solid rgba(255, 255, 255, 0.12);
  border-radius: var(--radius-xl);
  padding: 4rem;
  display: grid;
  grid-template-columns: 1fr 1.6fr;
  gap: 3.5rem;
  align-items: center;
  box-shadow: 0 30px 60px rgba(0, 0, 0, 0.4);
}

.editorial-eyebrow {
  font-size: 0.75rem;
  font-weight: 800;
  letter-spacing: 0.15em;
  color: #c084fc;
}

.editorial-title {
  font-family: var(--font-display);
  font-size: clamp(2.6rem, 4vw, 4rem);
  font-weight: 800;
  line-height: 1;
  letter-spacing: -0.04em;
  margin: 1rem 0 1.2rem;
  color: #ffffff;
}

.editorial-title em {
  color: #e879f9;
}

.editorial-desc {
  font-size: 0.98rem;
  line-height: 1.6;
  color: #cbd5e1;
  margin-bottom: 2rem;
  max-width: 380px;
}

.editorial-showcase-grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 1.2rem;
}

.curated-mini-card {
  background: rgba(255, 255, 255, 0.07);
  backdrop-filter: blur(12px);
  -webkit-backdrop-filter: blur(12px);
  border: 1px solid rgba(255, 255, 255, 0.12);
  border-radius: var(--radius-lg);
  padding: 1.2rem;
  display: flex;
  flex-direction: column;
  transition: all var(--transition-base);
}

.curated-mini-card:hover {
  transform: translateY(-6px);
  border-color: rgba(255, 255, 255, 0.3);
  box-shadow: 0 15px 35px rgba(0, 0, 0, 0.3);
}

.mini-card-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 0.8rem;
}

.curated-tag {
  font-size: 0.68rem;
  font-weight: 800;
  padding: 0.2rem 0.6rem;
  border-radius: var(--radius-pill);
}

.tag-new {
  background: rgba(34, 197, 94, 0.2);
  color: #4ade80;
}

.tag-best {
  background: rgba(244, 63, 94, 0.2);
  color: #fb7185;
}

.tag-trend {
  background: rgba(234, 179, 8, 0.2);
  color: #facc15;
}

.mini-wishlist-btn {
  background: transparent;
  border: 0;
  color: rgba(255, 255, 255, 0.6);
  cursor: pointer;
  padding: 0.2rem;
  transition: all var(--transition-fast);
}

.mini-wishlist-btn:hover,
.mini-wishlist-btn.in-wishlist {
  color: var(--accent-pink);
  transform: scale(1.15);
}

.mini-card-image-wrap {
  width: 100%;
  aspect-ratio: 1;
  border-radius: var(--radius-md);
  overflow: hidden;
  margin-bottom: 1rem;
  background: rgba(0, 0, 0, 0.2);
}

.mini-card-image-wrap img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform var(--transition-base);
}

.curated-mini-card:hover .mini-card-image-wrap img {
  transform: scale(1.08);
}

.mini-card-body h3 {
  font-size: 0.92rem;
  font-weight: 700;
  color: #ffffff;
  margin-bottom: 0.4rem;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

.mini-price-row {
  display: flex;
  align-items: baseline;
  gap: 0.6rem;
  margin-bottom: 0.4rem;
}

.current-price {
  font-size: 1.05rem;
  font-weight: 800;
  color: #ffffff;
}

.original-price {
  font-size: 0.8rem;
  text-decoration: line-through;
  color: var(--text-muted);
}

.mini-rating {
  font-size: 0.75rem;
  color: #fbbf24;
}

.rating-count {
  color: var(--text-muted);
}

/* ==========================================================================
   Category Discovery Section
   ========================================================================== */
.categories-section {
  padding: 5rem 0;
}

.section-heading-row {
  display: flex;
  align-items: flex-end;
  justify-content: space-between;
  margin-bottom: 3rem;
}

.category-eyebrow {
  display: block;
  font-size: 0.75rem;
  font-weight: 800;
  letter-spacing: 0.15em;
  color: var(--accent-purple-bright);
  margin-bottom: 0.5rem;
}

.category-eyebrow.neon-green {
  color: var(--accent-lime);
}

.section-heading-title {
  font-family: var(--font-display);
  font-size: clamp(2.2rem, 4vw, 3.5rem);
  font-weight: 850;
  line-height: 1;
  letter-spacing: -0.04em;
  color: #ffffff;
}

.view-all-link {
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  color: var(--text-secondary);
  text-decoration: none;
  font-size: 0.92rem;
  font-weight: 600;
  transition: all var(--transition-fast);
}

.view-all-link:hover {
  color: #ffffff;
  transform: translateX(3px);
}

.categories-grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 1.25rem;
}

.category-tile {
  position: relative;
  min-height: 180px;
  padding: 1.8rem;
  border-radius: var(--radius-lg);
  text-decoration: none;
  overflow: hidden;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  border: 1px solid var(--border-subtle);
  transition: all var(--transition-base);
}

.tile-style-0 {
  background: linear-gradient(135deg, #18112e 0%, #0d0a18 100%);
}

.tile-style-1 {
  background: linear-gradient(135deg, #24133b 0%, #110d21 100%);
}

.tile-style-2 {
  background: linear-gradient(135deg, #0f2420 0%, #081311 100%);
}

.category-tile:hover {
  transform: translateY(-5px);
  border-color: rgba(255, 255, 255, 0.2);
  box-shadow: 0 15px 35px rgba(0, 0, 0, 0.4);
}

.category-tile-content {
  position: relative;
  z-index: 1;
  height: 100%;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
}

.category-index {
  font-size: 0.8rem;
  font-weight: 800;
  color: var(--text-muted);
}

.category-text-block h3 {
  font-size: 1.35rem;
  font-weight: 800;
  color: #ffffff;
  letter-spacing: -0.02em;
  margin-bottom: 0.35rem;
}

.category-text-block p {
  font-size: 0.82rem;
  color: var(--text-muted);
  margin: 0;
}

.category-arrow-circle {
  position: absolute;
  top: 0;
  right: 0;
  width: 36px;
  height: 36px;
  border-radius: 50%;
  background: rgba(255, 255, 255, 0.06);
  border: 1px solid var(--border-subtle);
  display: grid;
  place-items: center;
  color: #ffffff;
  transition: all var(--transition-fast);
}

.category-tile:hover .category-arrow-circle {
  background: #ffffff;
  color: #0a0a0f;
  transform: scale(1.1);
}

/* ==========================================================================
   Featured Drops Section
   ========================================================================== */
.featured-drops-section {
  padding: 5rem 0 7rem;
}

.product-cards-grid {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 1.5rem;
}

.product-card-wrap {
  border-radius: var(--radius-lg);
  overflow: hidden;
  display: flex;
  flex-direction: column;
}

.card-image-box {
  position: relative;
  width: 100%;
  aspect-ratio: 1;
  background: #0d0c14;
  overflow: hidden;
}

.card-pill {
  position: absolute;
  top: 1rem;
  left: 1rem;
  z-index: 2;
}

.card-wishlist-action {
  position: absolute;
  top: 1rem;
  right: 1rem;
  z-index: 2;
  width: 36px;
  height: 36px;
  border-radius: 50%;
  background: rgba(18, 17, 27, 0.7);
  backdrop-filter: blur(10px);
  border: 1px solid var(--border-subtle);
  color: rgba(255, 255, 255, 0.7);
  display: grid;
  place-items: center;
  cursor: pointer;
  transition: all var(--transition-fast);
}

.card-wishlist-action:hover,
.card-wishlist-action.active {
  background: rgba(236, 72, 153, 0.2);
  color: var(--accent-pink);
  border-color: rgba(236, 72, 153, 0.4);
  transform: scale(1.08);
}

.image-aspect-link {
  display: block;
  width: 100%;
  height: 100%;
}

.image-aspect-link img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform var(--transition-smooth);
}

.product-card-wrap:hover .image-aspect-link img {
  transform: scale(1.06);
}

.card-details-box {
  padding: 1.4rem;
  display: flex;
  flex-direction: column;
  flex: 1;
  justify-content: space-between;
}

.card-topline {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 0.5rem;
  font-size: 0.75rem;
}

.product-category-name {
  color: var(--text-muted);
  text-transform: uppercase;
  font-weight: 700;
  letter-spacing: 0.05em;
}

.product-stars {
  color: #fbbf24;
  font-weight: 700;
}

.product-card-name {
  font-size: 1.05rem;
  font-weight: 700;
  letter-spacing: -0.01em;
  margin-bottom: 1.2rem;
  line-height: 1.35;
}

.product-card-name a {
  color: #ffffff;
  text-decoration: none;
  transition: color var(--transition-fast);
}

.product-card-name a:hover {
  color: var(--accent-purple-bright);
}

.card-bottom-row {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-top: auto;
}

.price-stack {
  display: flex;
  align-items: baseline;
  gap: 0.5rem;
}

.main-price {
  font-size: 1.2rem;
  font-weight: 800;
  color: #ffffff;
}

.strike-price {
  font-size: 0.85rem;
  color: var(--text-muted);
  text-decoration: line-through;
}

.add-to-bag-button {
  width: 40px;
  height: 40px;
  border-radius: 50%;
  background: rgba(255, 255, 255, 0.08);
  border: 1px solid var(--border-medium);
  color: #ffffff;
  display: grid;
  place-items: center;
  cursor: pointer;
  transition: all var(--transition-fast);
}

.add-to-bag-button:hover:not(:disabled) {
  background: #ffffff;
  color: #0a0a0f;
  border-color: #ffffff;
  transform: scale(1.08);
  box-shadow: 0 0 15px rgba(255, 255, 255, 0.3);
}

.add-to-bag-button:disabled {
  opacity: 0.4;
  cursor: not-allowed;
}

/* ==========================================================================
   Responsive Adaptations
   ========================================================================== */
@media (max-width: 1200px) {
  .product-cards-grid {
    grid-template-columns: repeat(3, 1fr);
  }
}

@media (max-width: 992px) {
  .hero-container {
    grid-template-columns: 1fr;
    gap: 3rem;
  }
  .hero-visual {
    min-height: 400px;
  }
  .trust-bar-grid {
    grid-template-columns: repeat(2, 1fr);
    gap: 1.5rem;
  }
  .editorial-banner-card {
    grid-template-columns: 1fr;
    padding: 2.5rem;
  }
  .editorial-showcase-grid {
    grid-template-columns: repeat(3, 1fr);
  }
  .categories-grid {
    grid-template-columns: repeat(2, 1fr);
  }
  .product-cards-grid {
    grid-template-columns: repeat(2, 1fr);
  }
}

@media (max-width: 640px) {
  .hero-title {
    font-size: 3.2rem;
  }
  .hero-actions {
    flex-direction: column;
    align-items: stretch;
  }
  .hero-actions a {
    justify-content: center;
  }
  .trending-widget, .edition-widget {
    position: static;
    margin: 0.5rem auto;
  }
  .sculptural-ring {
    width: 280px;
    height: 280px;
  }
  .trust-bar-grid {
    grid-template-columns: 1fr;
  }
  .editorial-showcase-grid {
    grid-template-columns: 1fr;
  }
  .categories-grid {
    grid-template-columns: 1fr;
  }
  .product-cards-grid {
    grid-template-columns: 1fr;
  }
}
</style>
