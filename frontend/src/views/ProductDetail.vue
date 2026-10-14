<template>
  <div class="product-detail-page">
    <div class="container">
      <!-- Breadcrumb navigation -->
      <nav class="breadcrumb-nav" aria-label="Breadcrumb">
        <router-link to="/">Home</router-link>
        <span class="sep">/</span>
        <router-link to="/products">Shop</router-link>
        <span class="sep">/</span>
        <span class="current-crumb">{{ product ? product.name : 'Loading...' }}</span>
      </nav>

      <!-- Main Product Display Grid -->
      <div v-if="product" class="product-viewport-grid">
        <!-- Left: Image Gallery & Trust Guarantees -->
        <div class="product-gallery-column">
          <div class="gallery-layout">
            <!-- Vertical Thumbnails -->
            <div class="thumbnail-strip" role="tablist" aria-label="Product angles">
              <button 
                v-for="(thumb, i) in galleryImages" 
                :key="i"
                class="thumb-btn"
                :class="{ 'active': activeImageIndex === i }"
                @click="activeImageIndex = i"
                :aria-label="`View image angle ${i + 1}`"
              >
                <img :src="thumb" :alt="`${product.name} angle ${i + 1}`" />
              </button>
            </div>

            <!-- Main High-Res Showcase -->
            <div class="main-image-container glass-card">
              <span class="image-counter-pill">{{ activeImageIndex + 1 }} / {{ galleryImages.length }}</span>
              <img 
                :src="galleryImages[activeImageIndex]" 
                :alt="product.name" 
                class="main-product-img"
              />
            </div>
          </div>

          <!-- Trust Badges Bar under Gallery (Matches Reference Mockup 4) -->
          <div class="product-trust-strip glass-panel">
            <div class="trust-pill">
              <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                <rect x="1" y="3" width="15" height="13"></rect>
                <polygon points="16 8 20 8 23 11 23 16 16 16 16 8"></polygon>
                <circle cx="5.5" cy="18.5" r="2.5"></circle>
                <circle cx="18.5" cy="18.5" r="2.5"></circle>
              </svg>
              <div>
                <strong>Free Delivery</strong>
                <p>On orders above ₹999</p>
              </div>
            </div>

            <div class="trust-pill">
              <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                <polyline points="1 4 1 10 7 10"></polyline>
                <path d="M3.51 15a9 9 0 1 0 2.13-9.36L1 10"></path>
              </svg>
              <div>
                <strong>Easy Returns</strong>
                <p>Within 30 days</p>
              </div>
            </div>

            <div class="trust-pill">
              <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                <path d="M12 22s8-4 8-10V5l-8-3-8 3v7c0 6 8 10 8 10z"></path>
              </svg>
              <div>
                <strong>1-Year Warranty</strong>
                <p>For peace of mind</p>
              </div>
            </div>
          </div>
        </div>

        <!-- Right: Purchasing Controls & Specs -->
        <div class="product-purchase-column">
          <!-- Stock Badge -->
          <div class="stock-badge-container">
            <span v-if="product.inStock" class="badge-pill in-stock-badge">
              <span class="stock-dot"></span> In Stock
            </span>
            <span v-else class="badge-pill out-stock-badge">
              Out of Stock
            </span>
            <span v-if="product.brandName" class="brand-tag">{{ product.brandName }}</span>
          </div>

          <!-- Product Title & Category -->
          <h1 class="pdp-title">{{ product.name }}</h1>
          <p class="pdp-category-line">{{ product.categoryName || 'NovaKart Exclusive Drop' }}</p>

          <!-- Rating Stars & Reviews link -->
          <div class="rating-bar">
            <div class="stars-display">
              <span v-for="star in 5" :key="star" class="star" :class="{ 'filled': star <= Math.round(product.rating) }">★</span>
            </div>
            <span class="rating-score">{{ product.rating.toFixed(1) }}</span>
            <a href="#reviews-anchor" class="reviews-count-link">({{ product.reviewCount || reviews.length }} verified reviews)</a>
          </div>

          <!-- Pricing Section with Savings -->
          <div class="pricing-card">
            <div class="price-figures">
              <span class="pdp-main-price">₹{{ product.price.toLocaleString('en-IN') }}</span>
              <span v-if="product.comparePrice" class="pdp-compare-price">₹{{ product.comparePrice.toLocaleString('en-IN') }}</span>
              <span v-if="product.discountPercent > 0" class="pdp-discount-pill">
                {{ product.discountPercent }}% OFF
              </span>
            </div>
            <p v-if="product.comparePrice && product.comparePrice > product.price" class="savings-text">
              You save ₹{{ (product.comparePrice - product.price).toLocaleString('en-IN') }}
            </p>
          </div>

          <!-- Color Swatch Selector (Mockup 4 Feature) -->
          <div class="option-group">
            <label class="option-label">Color Variant: <strong>{{ selectedColor }}</strong></label>
            <div class="color-swatch-list" role="radiogroup">
              <button 
                v-for="color in colors" 
                :key="color.name"
                class="color-swatch-btn"
                :class="{ 'active': selectedColor === color.name }"
                :style="{ backgroundColor: color.hex }"
                @click="selectedColor = color.name"
                :title="color.name"
              ></button>
            </div>
          </div>

          <!-- Quantity Stepper Selector -->
          <div class="option-group">
            <label class="option-label">Quantity</label>
            <div class="quantity-stepper">
              <button @click="decrementQty" :disabled="qty <= 1" aria-label="Decrease quantity">−</button>
              <span class="qty-display">{{ qty }}</span>
              <button @click="incrementQty" :disabled="qty >= Math.min(10, product.stock || 10)" aria-label="Increase quantity">+</button>
            </div>
          </div>

          <!-- Primary Actions -->
          <div class="action-buttons-group">
            <button 
              class="btn-pill btn-pill-primary pdp-add-cart-btn"
              :class="{ 'is-adding': cartStore.isAdding(product.id) }"
              :disabled="!product.inStock || cartStore.isAdding(product.id)"
              @click="addToCart"
            >
              <span v-if="cartStore.isAdding(product.id)" class="loading-spin"></span>
              <span v-else-if="!product.inStock">Sold Out</span>
              <span v-else class="btn-text-row">
                <span>Add to Cart</span>
                <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
                  <line x1="5" y1="12" x2="19" y2="12"></line>
                  <polyline points="12 5 19 12 12 19"></polyline>
                </svg>
              </span>
            </button>

            <button 
              class="btn-pill btn-pill-outline pdp-wishlist-btn"
              :class="{ 'in-wishlist': isInWishlist }"
              @click="toggleWishlist"
            >
              <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                <path d="M20.84 4.61a5.5 5.5 0 0 0-7.78 0L12 5.67l-1.06-1.06a5.5 5.5 0 0 0-7.78 7.78l1.06 1.06L12 21.23l7.78-7.78 1.06-1.06a5.5 5.5 0 0 0 0-7.78z"></path>
              </svg>
              <span>{{ isInWishlist ? 'In Wishlist' : 'Add to Wishlist' }}</span>
            </button>
          </div>

          <!-- Editorial Description -->
          <div class="product-description-card glass-panel">
            <h3>Description & Specifications</h3>
            <p>{{ product.description || 'Precision crafted with aerospace-grade aesthetics and intentional materials. Engineered for seamless everyday performance.' }}</p>
          </div>
        </div>
      </div>

      <!-- Loading State -->
      <div v-else class="pdp-skeleton-container container">
        <div class="skeleton-shimmer" style="height: 500px; border-radius: var(--radius-xl);"></div>
      </div>

      <!-- Customer Reviews Section -->
      <section id="reviews-anchor" class="reviews-section" v-if="product">
        <div class="reviews-header-row">
          <div>
            <span class="reviews-eyebrow">VERIFIED EXPERIENCES</span>
            <h2 class="reviews-title">Customer Reviews</h2>
          </div>
          <div class="reviews-summary-chip">
            <span class="stars-icon">★</span>
            <strong>{{ product.rating.toFixed(1) }}</strong>
            <span>based on {{ reviews.length }} review{{ reviews.length === 1 ? '' : 's' }}</span>
          </div>
        </div>

        <div class="reviews-content-grid">
          <!-- Reviews List -->
          <div class="reviews-list-col">
            <div v-if="reviews.length > 0" class="reviews-stream">
              <article v-for="review in reviews" :key="review.id" class="review-item-card glass-card">
                <div class="review-item-top">
                  <div class="author-block">
                    <div class="author-avatar">{{ review.authorName.charAt(0) }}</div>
                    <div>
                      <strong class="author-name">{{ review.authorName }}</strong>
                      <span v-if="review.verifiedPurchase" class="verified-tag">✓ Verified Purchase</span>
                    </div>
                  </div>
                  <span class="review-date-text">{{ new Date(review.createdOn).toLocaleDateString() }}</span>
                </div>

                <div class="review-stars-row">
                  <span v-for="star in 5" :key="star" class="review-star" :class="{ 'on': star <= review.rating }">★</span>
                </div>

                <h4 class="review-headline">{{ review.title }}</h4>
                <p class="review-body-text">{{ review.body }}</p>
              </article>
            </div>

            <div v-else class="no-reviews-box glass-panel">
              <p>Be the first to review this product and share your thoughts with the community.</p>
            </div>
          </div>

          <!-- Submit Review Form Card -->
          <div class="review-form-col">
            <div class="write-review-card glass-panel" v-if="authStore.isAuthenticated">
              <h3>Write a Review</h3>
              <form @submit.prevent="submitReview" class="review-form-fields">
                <div class="form-group">
                  <label>Overall Rating</label>
                  <select v-model.number="reviewForm.rating" required class="form-select">
                    <option :value="5">⭐⭐⭐⭐⭐ (5 - Exceptional)</option>
                    <option :value="4">⭐⭐⭐⭐ (4 - Very Good)</option>
                    <option :value="3">⭐⭐⭐ (3 - Average)</option>
                    <option :value="2">⭐⭐ (2 - Below Expectation)</option>
                    <option :value="1">⭐ (1 - Disappointing)</option>
                  </select>
                </div>

                <div class="form-group">
                  <label>Review Headline</label>
                  <input v-model="reviewForm.title" placeholder="Summarize your experience..." required class="form-input" />
                </div>

                <div class="form-group">
                  <label>Your Review</label>
                  <textarea v-model="reviewForm.body" rows="4" placeholder="What did you like or dislike? How is the quality?" required class="form-textarea"></textarea>
                </div>

                <button type="submit" class="btn-pill btn-pill-primary submit-review-btn">
                  Submit Review
                </button>
              </form>
            </div>

            <div v-else class="login-prompt-box glass-panel">
              <h3>Share your thoughts</h3>
              <p>Please sign in to your NovaKart account to submit a verified product review.</p>
              <router-link to="/login" class="btn-pill btn-pill-primary">Sign In to Review</router-link>
            </div>
          </div>
        </div>
      </section>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { useRoute } from 'vue-router'
import { useCatalogStore } from '../stores/catalog'
import { useCartStore } from '../stores/cart'
import { useWishlistStore } from '../stores/wishlist'
import { useAuthStore } from '../stores/auth'
import { reviewsApi } from '../api/reviews'
import type { Product } from '../types'
import { productImage } from '../utils/productImage'

const route = useRoute()
const catalogStore = useCatalogStore()
const cartStore = useCartStore()
const wishlistStore = useWishlistStore()
const authStore = useAuthStore()

const product = ref<Product | null>(null)
const reviews = ref<any[]>([])
const reviewForm = ref({ rating: 5, title: '', body: '' })
const qty = ref(1)
const activeImageIndex = ref(0)

const colors = [
  { name: 'Electric Lime', hex: '#22c55e' },
  { name: 'Neon Purple', hex: '#a855f7' },
  { name: 'Titanium Silver', hex: '#cbd5e1' },
  { name: 'Obsidian Black', hex: '#18181b' }
]
const selectedColor = ref('Electric Lime')

const galleryImages = computed(() => {
  if (!product.value) return []
  const base = productImage(product.value)
  // Provide alternative view angle crops
  return [
    base,
    `${base}&crop=entropy&sat=10`,
    `${base}&crop=focalpoint&fp-z=1.5`,
    `${base}&auto=format&q=95`
  ]
})

const isInWishlist = computed(() => 
  wishlistStore.items.some((item: any) => item.productId === product.value?.id)
)

function incrementQty() {
  if (qty.value < 10) qty.value++
}

function decrementQty() {
  if (qty.value > 1) qty.value--
}

async function loadProduct() {
  const id = parseInt(route.params.id as string)
  product.value = await catalogStore.fetchProduct(id)
}

async function loadReviews() {
  if (product.value) {
    const response = await reviewsApi.getProductReviews(product.value.id)
    reviews.value = response.data
  }
}

function addToCart() {
  if (product.value) {
    cartStore.addToCart(product.value.id, qty.value, selectedColor.value, product.value.name)
  }
}

function toggleWishlist() {
  if (!authStore.isAuthenticated) {
    window.location.href = '/login'
    return
  }
  if (product.value) {
    wishlistStore.toggleWishlist(product.value.id)
  }
}

async function submitReview() {
  if (product.value) {
    await reviewsApi.submitReview({
      productId: product.value.id,
      rating: reviewForm.value.rating,
      title: reviewForm.value.title,
      body: reviewForm.value.body
    })
    reviewForm.value = { rating: 5, title: '', body: '' }
    await loadReviews()
    await loadProduct()
  }
}

onMounted(async () => {
  await loadProduct()
  await loadReviews()
  if (authStore.isAuthenticated) {
    await wishlistStore.fetchWishlist()
  }
})
</script>

<style scoped>
.product-detail-page {
  min-height: 100vh;
  padding: 2.5rem 0 6rem;
  background-color: var(--bg-main);
  color: var(--text-primary);
}

/* Breadcrumb */
.breadcrumb-nav {
  display: flex;
  align-items: center;
  gap: 0.6rem;
  font-size: 0.82rem;
  color: var(--text-muted);
  margin-bottom: 2.5rem;
}

.breadcrumb-nav a {
  color: var(--text-secondary);
  text-decoration: none;
  transition: color var(--transition-fast);
}

.breadcrumb-nav a:hover {
  color: #ffffff;
}

.current-crumb {
  color: var(--text-primary);
  font-weight: 600;
  max-width: 300px;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

/* Main PDP Grid */
.product-viewport-grid {
  display: grid;
  grid-template-columns: 1.15fr 0.85fr;
  gap: 4rem;
  align-items: flex-start;
  margin-bottom: 5rem;
}

/* Left Gallery Column */
.product-gallery-column {
  display: flex;
  flex-direction: column;
  gap: 2rem;
}

.gallery-layout {
  display: grid;
  grid-template-columns: 88px 1fr;
  gap: 1.25rem;
}

.thumbnail-strip {
  display: flex;
  flex-direction: column;
  gap: 0.85rem;
}

.thumb-btn {
  width: 88px;
  height: 88px;
  border-radius: var(--radius-md);
  overflow: hidden;
  background: var(--bg-surface);
  border: 2px solid var(--border-subtle);
  cursor: pointer;
  padding: 0;
  transition: all var(--transition-fast);
}

.thumb-btn:hover {
  border-color: rgba(255, 255, 255, 0.4);
}

.thumb-btn.active {
  border-color: var(--accent-lime);
  box-shadow: 0 0 15px var(--accent-lime-glow);
}

.thumb-btn img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.main-image-container {
  position: relative;
  aspect-ratio: 1;
  border-radius: var(--radius-xl);
  overflow: hidden;
  background: #0d0c15;
  display: flex;
  align-items: center;
  justify-content: center;
}

.image-counter-pill {
  position: absolute;
  bottom: 1.2rem;
  right: 1.2rem;
  background: rgba(0, 0, 0, 0.7);
  backdrop-filter: blur(8px);
  color: #ffffff;
  font-size: 0.75rem;
  font-weight: 700;
  padding: 0.35rem 0.8rem;
  border-radius: var(--radius-pill);
  z-index: 2;
}

.main-product-img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform var(--transition-smooth);
}

.main-image-container:hover .main-product-img {
  transform: scale(1.05);
}

/* Trust Strip below Gallery */
.product-trust-strip {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 1.5rem;
  padding: 1.5rem;
  border-radius: var(--radius-lg);
}

.trust-pill {
  display: flex;
  align-items: center;
  gap: 0.85rem;
}

.trust-pill svg {
  color: var(--accent-lime);
  flex-shrink: 0;
}

.trust-pill strong {
  display: block;
  font-size: 0.84rem;
  color: #ffffff;
}

.trust-pill p {
  font-size: 0.74rem;
  color: var(--text-muted);
  margin: 0;
}

/* Right Purchasing Column */
.product-purchase-column {
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
}

.stock-badge-container {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.in-stock-badge {
  background: rgba(34, 197, 94, 0.15);
  color: #4ade80;
  border: 1px solid rgba(34, 197, 94, 0.3);
}

.stock-dot {
  width: 6px;
  height: 6px;
  border-radius: 50%;
  background: #22c55e;
  box-shadow: 0 0 8px #22c55e;
}

.out-stock-badge {
  background: rgba(239, 68, 68, 0.15);
  color: #f87171;
  border: 1px solid rgba(239, 68, 68, 0.3);
}

.brand-tag {
  font-size: 0.78rem;
  font-weight: 700;
  color: var(--text-muted);
  text-transform: uppercase;
  letter-spacing: 0.08em;
}

.pdp-title {
  font-family: var(--font-display);
  font-size: clamp(2.4rem, 4vw, 3.4rem);
  font-weight: 850;
  line-height: 1.05;
  letter-spacing: -0.03em;
  color: #ffffff;
}

.pdp-category-line {
  font-size: 0.95rem;
  color: var(--text-secondary);
  margin-top: -0.75rem;
}

.rating-bar {
  display: flex;
  align-items: center;
  gap: 0.65rem;
}

.stars-display {
  display: flex;
  gap: 2px;
  color: var(--text-muted);
}

.star.filled {
  color: #fbbf24;
}

.rating-score {
  font-weight: 800;
  font-size: 0.88rem;
  color: #ffffff;
}

.reviews-count-link {
  color: var(--accent-purple-bright);
  font-size: 0.82rem;
  text-decoration: underline;
}

/* Pricing Card */
.pricing-card {
  padding: 1.5rem;
  background: rgba(255, 255, 255, 0.03);
  border: 1px solid var(--border-subtle);
  border-radius: var(--radius-lg);
}

.price-figures {
  display: flex;
  align-items: baseline;
  gap: 1rem;
}

.pdp-main-price {
  font-size: 2.4rem;
  font-weight: 900;
  color: #ffffff;
  letter-spacing: -0.02em;
}

.pdp-compare-price {
  font-size: 1.25rem;
  color: var(--text-muted);
  text-decoration: line-through;
}

.pdp-discount-pill {
  background: var(--accent-lime);
  color: #052e16;
  font-size: 0.78rem;
  font-weight: 800;
  padding: 0.25rem 0.65rem;
  border-radius: var(--radius-pill);
}

.savings-text {
  font-size: 0.84rem;
  color: var(--accent-lime-bright);
  margin-top: 0.4rem;
  font-weight: 600;
}

/* Swatches & Stepper */
.option-group {
  display: flex;
  flex-direction: column;
  gap: 0.6rem;
}

.option-label {
  font-size: 0.84rem;
  color: var(--text-secondary);
}

.option-label strong {
  color: #ffffff;
}

.color-swatch-list {
  display: flex;
  gap: 0.85rem;
}

.color-swatch-btn {
  width: 32px;
  height: 32px;
  border-radius: 50%;
  border: 3px solid var(--bg-main);
  outline: 2px solid transparent;
  cursor: pointer;
  transition: all var(--transition-fast);
}

.color-swatch-btn:hover {
  transform: scale(1.15);
}

.color-swatch-btn.active {
  outline-color: var(--accent-lime);
  transform: scale(1.15);
}

.quantity-stepper {
  display: inline-flex;
  align-items: center;
  width: max-content;
  border: 1px solid var(--border-medium);
  border-radius: var(--radius-pill);
  background: rgba(255, 255, 255, 0.04);
}

.quantity-stepper button {
  width: 38px;
  height: 38px;
  border: 0;
  background: transparent;
  color: #ffffff;
  font-size: 1.2rem;
  cursor: pointer;
  display: grid;
  place-items: center;
  transition: background var(--transition-fast);
}

.quantity-stepper button:hover:not(:disabled) {
  background: rgba(255, 255, 255, 0.1);
}

.quantity-stepper button:disabled {
  opacity: 0.3;
  cursor: not-allowed;
}

.qty-display {
  width: 38px;
  text-align: center;
  font-weight: 700;
  font-size: 0.95rem;
  color: #ffffff;
}

/* Action Buttons */
.action-buttons-group {
  display: flex;
  gap: 1rem;
  margin-top: 0.5rem;
}

.pdp-add-cart-btn {
  flex: 1.6;
  padding: 1.05rem 1.8rem;
  font-size: 0.98rem;
}

.pdp-wishlist-btn {
  flex: 1;
  padding: 1.05rem 1.4rem;
  font-size: 0.92rem;
}

.pdp-wishlist-btn.in-wishlist {
  border-color: var(--accent-pink);
  color: var(--accent-pink);
  background: rgba(236, 72, 153, 0.1);
}

.btn-text-row {
  display: flex;
  align-items: center;
  gap: 0.6rem;
}

.product-description-card {
  padding: 1.8rem;
  border-radius: var(--radius-lg);
  margin-top: 1rem;
}

.product-description-card h3 {
  font-size: 1.05rem;
  font-weight: 700;
  color: #ffffff;
  margin-bottom: 0.75rem;
}

.product-description-card p {
  color: var(--text-secondary);
  font-size: 0.92rem;
  line-height: 1.65;
}

/* Reviews Section */
.reviews-section {
  padding: 5rem 0 2rem;
  border-top: 1px solid var(--border-subtle);
}

.reviews-header-row {
  display: flex;
  align-items: flex-end;
  justify-content: space-between;
  margin-bottom: 3rem;
}

.reviews-eyebrow {
  display: block;
  font-size: 0.75rem;
  font-weight: 800;
  letter-spacing: 0.15em;
  color: var(--accent-purple-bright);
  margin-bottom: 0.5rem;
}

.reviews-title {
  font-family: var(--font-display);
  font-size: 2.6rem;
  font-weight: 850;
  color: #ffffff;
}

.reviews-summary-chip {
  display: flex;
  align-items: center;
  gap: 0.6rem;
  padding: 0.6rem 1.2rem;
  border-radius: var(--radius-pill);
  background: rgba(255, 255, 255, 0.05);
  border: 1px solid var(--border-subtle);
  font-size: 0.88rem;
}

.stars-icon {
  color: #fbbf24;
}

.reviews-summary-chip strong {
  color: #ffffff;
}

.reviews-summary-chip span {
  color: var(--text-muted);
}

.reviews-content-grid {
  display: grid;
  grid-template-columns: 1.3fr 0.7fr;
  gap: 3.5rem;
}

.reviews-stream {
  display: flex;
  flex-direction: column;
  gap: 1.2rem;
}

.review-item-card {
  padding: 1.8rem;
  border-radius: var(--radius-lg);
}

.review-item-top {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 0.8rem;
}

.author-block {
  display: flex;
  align-items: center;
  gap: 0.85rem;
}

.author-avatar {
  width: 36px;
  height: 36px;
  border-radius: 50%;
  background: linear-gradient(135deg, #a855f7, #22c55e);
  color: #ffffff;
  font-weight: 800;
  display: grid;
  place-items: center;
  font-size: 0.95rem;
}

.author-name {
  display: block;
  font-size: 0.92rem;
  color: #ffffff;
}

.verified-tag {
  font-size: 0.72rem;
  color: var(--accent-lime);
  font-weight: 600;
}

.review-date-text {
  font-size: 0.78rem;
  color: var(--text-muted);
}

.review-stars-row {
  display: flex;
  gap: 2px;
  margin-bottom: 0.6rem;
}

.review-star {
  color: rgba(255, 255, 255, 0.2);
  font-size: 0.85rem;
}

.review-star.on {
  color: #fbbf24;
}

.review-headline {
  font-size: 1.05rem;
  font-weight: 700;
  color: #ffffff;
  margin-bottom: 0.5rem;
}

.review-body-text {
  color: var(--text-secondary);
  font-size: 0.92rem;
  line-height: 1.6;
}

/* Review Form */
.write-review-card,
.login-prompt-box {
  padding: 2.2rem;
  border-radius: var(--radius-xl);
}

.write-review-card h3,
.login-prompt-box h3 {
  font-size: 1.35rem;
  font-weight: 800;
  color: #ffffff;
  margin-bottom: 1.5rem;
}

.review-form-fields {
  display: flex;
  flex-direction: column;
  gap: 1.25rem;
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.form-group label {
  font-size: 0.82rem;
  font-weight: 600;
  color: var(--text-secondary);
}

.form-input,
.form-select,
.form-textarea {
  width: 100%;
  padding: 0.85rem 1rem;
  background: rgba(255, 255, 255, 0.04);
  border: 1px solid var(--border-medium);
  border-radius: var(--radius-md);
  color: #ffffff;
  font-family: inherit;
  font-size: 0.88rem;
  outline: none;
}

.form-input:focus,
.form-select:focus,
.form-textarea:focus {
  border-color: var(--accent-purple-bright);
}

.form-select option {
  background: var(--bg-surface);
  color: #ffffff;
}

.submit-review-btn {
  width: 100%;
  margin-top: 0.5rem;
}

.login-prompt-box p {
  color: var(--text-secondary);
  margin-bottom: 1.5rem;
  line-height: 1.5;
}

/* Responsive */
@media (max-width: 992px) {
  .product-viewport-grid {
    grid-template-columns: 1fr;
    gap: 3rem;
  }
  .reviews-content-grid {
    grid-template-columns: 1fr;
  }
}

@media (max-width: 640px) {
  .gallery-layout {
    grid-template-columns: 1fr;
  }
  .thumbnail-strip {
    flex-direction: row;
    order: 2;
    overflow-x: auto;
  }
  .thumb-btn {
    width: 65px;
    height: 65px;
    flex-shrink: 0;
  }
  .product-trust-strip {
    grid-template-columns: 1fr;
  }
  .action-buttons-group {
    flex-direction: column;
  }
}
</style>
