<template>
  <div class="wishlist-page">
    <div class="container">
      <header class="wishlist-header">
        <div class="wishlist-eyebrow">
          <span class="eyebrow-dot"></span>
          <span>CURATED VAULT</span>
        </div>
        <h1 class="wishlist-title">
          Saved Wishlist<span class="dot-accent">.</span>
        </h1>
        <p class="wishlist-subtitle" v-if="wishlistStore.items.length > 0">
          You have {{ wishlistStore.items.length }} saved object{{ wishlistStore.items.length === 1 ? '' : 's' }} in your personal vault.
        </p>
      </header>

      <!-- Active Items Grid -->
      <div v-if="!wishlistStore.loading && wishlistStore.items.length > 0" class="wishlist-grid">
        <article 
          v-for="item in wishlistStore.items" 
          :key="item.productId" 
          class="wishlist-card glass-card"
        >
          <div class="wishlist-image-box">
            <button 
              class="remove-icon-btn" 
              @click="removeItem(item.productId)" 
              aria-label="Remove from wishlist"
              title="Remove"
            >
              <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                <line x1="18" y1="6" x2="6" y2="18"></line>
                <line x1="6" y1="6" x2="18" y2="18"></line>
              </svg>
            </button>

            <router-link :to="`/products/${item.productId}`" class="wishlist-image-link">
              <img 
                :src="getItemImage(item)" 
                :alt="item.productName" 
                loading="lazy" 
              />
            </router-link>
          </div>

          <div class="wishlist-card-body">
            <h2 class="wishlist-item-title">
              <router-link :to="`/products/${item.productId}`">{{ item.productName }}</router-link>
            </h2>

            <div class="wishlist-price-row">
              <span class="item-price">₹{{ item.price.toLocaleString('en-IN') }}</span>
            </div>

            <button 
              class="btn-pill btn-pill-primary wishlist-add-btn"
              :class="{ 'is-adding': cartStore.isAdding(item.productId) }"
              :disabled="cartStore.isAdding(item.productId)"
              @click="addToCart(item)"
            >
              <span v-if="cartStore.isAdding(item.productId)" class="loading-spin"></span>
              <span v-else class="btn-label-row">
                <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
                  <line x1="12" y1="5" x2="12" y2="19"></line>
                  <line x1="5" y1="12" x2="19" y2="12"></line>
                </svg>
                <span>Add to Bag</span>
              </span>
            </button>
          </div>
        </article>
      </div>

      <!-- Loading State -->
      <div v-else-if="wishlistStore.loading" class="wishlist-grid">
        <div v-for="n in 4" :key="n" class="wishlist-card skeleton-shimmer" style="height: 380px;"></div>
      </div>

      <!-- Empty State -->
      <div v-else class="empty-wishlist-card glass-panel">
        <div class="empty-heart-icon">♡</div>
        <h2>Your wishlist is empty</h2>
        <p>Save items you love by tapping the heart icon on any product card while browsing.</p>
        <router-link to="/products" class="btn-pill btn-pill-primary">Browse Catalogue</router-link>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted } from 'vue'
import { useWishlistStore } from '../stores/wishlist'
import { useCartStore } from '../stores/cart'
import { productImage } from '../utils/productImage'

const wishlistStore = useWishlistStore()
const cartStore = useCartStore()

function getItemImage(item: any): string {
  if (item.imageUrl && !item.imageUrl.includes('placehold.co')) {
    return item.imageUrl
  }
  return productImage({
    id: item.productId,
    name: item.productName,
    imageUrl: item.imageUrl
  })
}

function addToCart(item: { productId: number; productName: string }) {
  cartStore.addToCart(item.productId, 1, undefined, item.productName)
}

function removeItem(productId: number) {
  wishlistStore.removeFromWishlist(productId)
}

onMounted(() => {
  wishlistStore.fetchWishlist()
})
</script>

<style scoped>
.wishlist-page {
  min-height: 100vh;
  padding: 3.5rem 0 6rem;
  background-color: var(--bg-main);
  color: var(--text-primary);
}

.wishlist-header {
  margin-bottom: 3.5rem;
}

.wishlist-eyebrow {
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

.wishlist-title {
  font-family: var(--font-display);
  font-size: clamp(2.8rem, 5.5vw, 4.4rem);
  font-weight: 900;
  line-height: 0.95;
  letter-spacing: -0.04em;
  color: #ffffff;
  margin-bottom: 0.8rem;
}

.wishlist-subtitle {
  font-size: 1.05rem;
  color: var(--text-secondary);
}

.wishlist-grid {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 1.5rem;
}

.wishlist-card {
  border-radius: var(--radius-lg);
  overflow: hidden;
  display: flex;
  flex-direction: column;
}

.wishlist-image-box {
  position: relative;
  width: 100%;
  aspect-ratio: 1;
  background: #0d0c15;
  overflow: hidden;
}

.remove-icon-btn {
  position: absolute;
  top: 0.85rem;
  right: 0.85rem;
  z-index: 2;
  width: 34px;
  height: 34px;
  border-radius: 50%;
  background: rgba(18, 17, 27, 0.75);
  backdrop-filter: blur(8px);
  border: 1px solid var(--border-subtle);
  color: var(--text-muted);
  display: grid;
  place-items: center;
  cursor: pointer;
  transition: all var(--transition-fast);
}

.remove-icon-btn:hover {
  background: rgba(239, 68, 68, 0.2);
  color: #f87171;
  border-color: rgba(239, 68, 68, 0.4);
}

.wishlist-image-link {
  display: block;
  width: 100%;
  height: 100%;
}

.wishlist-image-link img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform var(--transition-smooth);
}

.wishlist-card:hover .wishlist-image-link img {
  transform: scale(1.08);
}

.wishlist-card-body {
  padding: 1.4rem;
  display: flex;
  flex-direction: column;
  flex: 1;
  justify-content: space-between;
}

.wishlist-item-title {
  font-size: 1.05rem;
  font-weight: 700;
  letter-spacing: -0.01em;
  margin-bottom: 0.6rem;
  line-height: 1.35;
}

.wishlist-item-title a {
  color: #ffffff;
  text-decoration: none;
  transition: color var(--transition-fast);
}

.wishlist-item-title a:hover {
  color: var(--accent-purple-bright);
}

.wishlist-price-row {
  margin-bottom: 1.25rem;
}

.item-price {
  font-size: 1.25rem;
  font-weight: 800;
  color: #ffffff;
}

.wishlist-add-btn {
  width: 100%;
  padding: 0.75rem;
}

.btn-label-row {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.empty-wishlist-card {
  max-width: 500px;
  margin: 3rem auto;
  padding: 4.5rem 2rem;
  text-align: center;
  border-radius: var(--radius-xl);
}

.empty-heart-icon {
  font-size: 2.8rem;
  color: var(--accent-pink);
  margin-bottom: 1.2rem;
}

.empty-wishlist-card h2 {
  font-size: 1.8rem;
  color: #ffffff;
  margin-bottom: 0.6rem;
}

.empty-wishlist-card p {
  color: var(--text-secondary);
  font-size: 0.92rem;
  margin-bottom: 2rem;
}

@media (max-width: 1100px) {
  .wishlist-grid {
    grid-template-columns: repeat(3, 1fr);
  }
}

@media (max-width: 768px) {
  .wishlist-grid {
    grid-template-columns: repeat(2, 1fr);
  }
}

@media (max-width: 520px) {
  .wishlist-grid {
    grid-template-columns: 1fr;
  }
}
</style>
