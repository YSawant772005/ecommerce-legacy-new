<template>
  <div class="cart-page">
    <div class="container">
      <!-- Page Header -->
      <header class="cart-header">
        <div class="cart-eyebrow">
          <span class="eyebrow-dot"></span>
          <span>CURATED BAG</span>
        </div>
        <h1 class="cart-title">
          Ready when <em class="serif-accent">you are.</em>
        </h1>
        <p class="cart-subtitle" v-if="cartStore.cart && cartStore.cart.items.length > 0">
          You have {{ cartStore.itemCount }} item{{ cartStore.itemCount === 1 ? '' : 's' }} in your shopping bag.
        </p>
      </header>

      <!-- Active Cart Content -->
      <div v-if="!cartStore.loading && cartStore.cart && cartStore.cart.items.length > 0" class="cart-layout-grid">
        <!-- Left: Cart Items List -->
        <div class="cart-items-column">
          <!-- Free Shipping Progress Bar -->
          <div class="free-shipping-card glass-panel">
            <div class="shipping-progress-header">
              <span class="truck-icon">⚡</span>
              <span v-if="cartStore.cart.subtotal >= 999" class="shipping-status-text unlocked">
                You've unlocked <strong>Free Express Delivery!</strong>
              </span>
              <span v-else class="shipping-status-text">
                Add <strong>₹{{ (999 - cartStore.cart.subtotal).toLocaleString('en-IN') }}</strong> more for Free Delivery
              </span>
            </div>
            <div class="shipping-progress-track">
              <div 
                class="shipping-progress-fill" 
                :style="{ width: `${Math.min(100, (cartStore.cart.subtotal / 999) * 100)}%` }"
              ></div>
            </div>
          </div>

          <!-- Items Stream -->
          <div class="cart-items-stream">
            <article 
              v-for="item in cartStore.cart.items" 
              :key="item.productId" 
              class="cart-item-card glass-card"
            >
              <div class="item-thumb-box">
                <img
                  :src="productImage({ id: item.productId, name: item.productName, imageUrl: item.imageUrl })"
                  :alt="item.productName"
                  @error="useFallbackImage"
                />
              </div>

              <div class="item-main-details">
                <div class="item-info-top">
                  <h2 class="item-title">
                    <router-link :to="`/products/${item.productId}`">{{ item.productName }}</router-link>
                  </h2>
                  <span class="item-unit-price">₹{{ item.unitPrice.toLocaleString('en-IN') }} each</span>
                </div>

                <div class="item-actions-row">
                  <!-- Quantity Stepper -->
                  <div class="item-stepper">
                    <button @click="updateQty(item.productId, item.qty - 1)" aria-label="Decrease quantity">−</button>
                    <span class="stepper-qty">{{ item.qty }}</span>
                    <button @click="updateQty(item.productId, item.qty + 1)" :disabled="item.qty >= 10" aria-label="Increase quantity">+</button>
                  </div>

                  <!-- Remove Action -->
                  <button @click="removeItem(item.productId)" class="item-remove-btn">
                    <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                      <line x1="18" y1="6" x2="6" y2="18"></line>
                      <line x1="6" y1="6" x2="18" y2="18"></line>
                    </svg>
                    <span>Remove</span>
                  </button>
                </div>
              </div>

              <div class="item-total-col">
                <span class="item-line-total">₹{{ item.lineTotal.toLocaleString('en-IN') }}</span>
              </div>
            </article>
          </div>

          <div class="cart-actions-bottom">
            <router-link to="/products" class="continue-shopping-link">
              ← Continue Shopping
            </router-link>
            <button class="clear-cart-link" @click="clearCart">Clear Bag</button>
          </div>
        </div>

        <!-- Right: Order Summary Card -->
        <aside class="cart-summary-column">
          <div class="summary-card glass-panel">
            <h2 class="summary-title">Order Summary</h2>

            <!-- Line Items Breakdown -->
            <div class="summary-breakdown">
              <div class="breakdown-row">
                <span>Subtotal</span>
                <span>₹{{ cartStore.cart.subtotal.toLocaleString('en-IN') }}</span>
              </div>

              <div class="breakdown-row discount-row" v-if="cartStore.cart.discount > 0">
                <span>Promotional Discount</span>
                <span>-₹{{ cartStore.cart.discount.toLocaleString('en-IN') }}</span>
              </div>

              <div class="breakdown-row">
                <span>Estimated Shipping</span>
                <span v-if="cartStore.cart.shipping === 0" class="free-shipping-tag">FREE</span>
                <span v-else>₹{{ cartStore.cart.shipping }}</span>
              </div>

              <div class="breakdown-row">
                <span>Estimated GST Tax (18%)</span>
                <span>₹{{ cartStore.cart.tax.toLocaleString('en-IN') }}</span>
              </div>

              <div class="summary-divider"></div>

              <div class="breakdown-row total-row">
                <span>Total Due</span>
                <span class="total-amount">₹{{ cartStore.cart.total.toLocaleString('en-IN') }}</span>
              </div>
            </div>

            <!-- Coupon Input Block -->
            <div class="coupon-box">
              <label for="cart-coupon" class="coupon-label">Have a promotional coupon?</label>
              <div class="coupon-input-group">
                <input 
                  id="cart-coupon"
                  v-model="couponCode" 
                  placeholder="Enter promo code" 
                />
                <button @click="applyCoupon" class="apply-coupon-btn" :disabled="!couponCode.trim()">
                  Apply
                </button>
              </div>
              <span v-if="cartStore.cart.couponCode" class="active-coupon-pill">
                Active Code: <strong>{{ cartStore.cart.couponCode }}</strong>
              </span>
            </div>

            <!-- Checkout CTA Button -->
            <router-link to="/checkout" class="btn-pill btn-pill-primary checkout-action-btn">
              <span>Proceed to Checkout</span>
              <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
                <line x1="5" y1="12" x2="19" y2="12"></line>
                <polyline points="12 5 19 12 12 19"></polyline>
              </svg>
            </router-link>

            <!-- Trust Badges Under Summary -->
            <div class="summary-trust-bullets">
              <div class="bullet-item">
                <span class="bullet-icon">🔒</span>
                <span>256-bit SSL encrypted checkout</span>
              </div>
              <div class="bullet-item">
                <span class="bullet-icon">✦</span>
                <span>Complimentary 30-day returns</span>
              </div>
            </div>
          </div>
        </aside>
      </div>

      <!-- Loading State -->
      <div v-else-if="cartStore.loading" class="cart-loading-state">
        <div class="skeleton-shimmer" style="height: 350px; border-radius: var(--radius-xl);"></div>
      </div>

      <!-- Empty Cart State -->
      <div v-else class="empty-cart-state glass-panel">
        <div class="empty-bag-icon">
          <svg width="48" height="48" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5">
            <path d="M6 2L3 6v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2V6l-3-2z"></path>
            <line x1="3" y1="6" x2="21" y2="6"></line>
            <path d="M16 10a4 4 0 0 1-8 0"></path>
          </svg>
        </div>
        <h2>Your bag is currently empty</h2>
        <p>Explore our latest catalogue edit and discover something exceptional for your lifestyle.</p>
        <router-link to="/products" class="btn-pill btn-pill-primary start-shopping-btn">
          <span>Explore The Edit</span>
          <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
            <line x1="5" y1="12" x2="19" y2="12"></line>
            <polyline points="12 5 19 12 12 19"></polyline>
          </svg>
        </router-link>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useCartStore } from '../stores/cart'
import { productImage } from '../utils/productImage'

const cartStore = useCartStore()
const couponCode = ref('')
const fallbackImage = 'data:image/svg+xml,%3Csvg xmlns="http://www.w3.org/2000/svg" width="400" height="400" viewBox="0 0 400 400"%3E%3Crect width="400" height="400" fill="%23191827"/%3E%3Ctext x="200" y="210" text-anchor="middle" font-family="Arial" font-size="28" fill="%23ffffff"%3ENova%3C/text%3E%3C/svg%3E'

function useFallbackImage(event: Event) {
  const image = event.target as HTMLImageElement
  if (image.src !== fallbackImage) image.src = fallbackImage
}

function updateQty(productId: number, qty: number) {
  if (qty <= 0) {
    cartStore.removeCartItem(productId)
  } else {
    cartStore.updateCartItem(productId, qty)
  }
}

function removeItem(productId: number) {
  cartStore.removeCartItem(productId)
}

function clearCart() {
  if (confirm('Are you sure you want to empty your shopping bag?')) {
    cartStore.clearCart()
  }
}

function applyCoupon() {
  if (couponCode.value.trim()) {
    cartStore.applyCoupon(couponCode.value.trim())
  }
}

onMounted(() => {
  cartStore.fetchCart()
})
</script>

<style scoped>
.cart-page {
  min-height: 100vh;
  padding: 3.5rem 0 6rem;
  background-color: var(--bg-main);
  color: var(--text-primary);
}

.cart-header {
  margin-bottom: 3rem;
}

.cart-eyebrow {
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

.cart-title {
  font-family: var(--font-display);
  font-size: clamp(2.8rem, 5.5vw, 4.6rem);
  font-weight: 900;
  line-height: 0.95;
  letter-spacing: -0.04em;
  color: #ffffff;
  margin-bottom: 0.8rem;
}

.cart-title em {
  color: var(--accent-purple-bright);
}

.cart-subtitle {
  font-size: 1.05rem;
  color: var(--text-secondary);
}

/* Grid Layout */
.cart-layout-grid {
  display: grid;
  grid-template-columns: 1.4fr 0.9fr;
  gap: 3.5rem;
  align-items: flex-start;
}

/* Left Column */
.cart-items-column {
  display: flex;
  flex-direction: column;
  gap: 1.8rem;
}

.free-shipping-card {
  padding: 1.4rem 1.6rem;
  border-radius: var(--radius-lg);
  border-color: rgba(34, 197, 94, 0.25);
  background: rgba(18, 30, 24, 0.5);
}

.shipping-progress-header {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  font-size: 0.88rem;
  color: #ffffff;
  margin-bottom: 0.75rem;
}

.shipping-status-text.unlocked strong {
  color: #4ade80;
}

.shipping-progress-track {
  width: 100%;
  height: 6px;
  background: rgba(255, 255, 255, 0.08);
  border-radius: var(--radius-pill);
  overflow: hidden;
}

.shipping-progress-fill {
  height: 100%;
  background: linear-gradient(90deg, #10b981, #4ade80);
  border-radius: var(--radius-pill);
  transition: width 0.4s ease;
}

/* Items Stream */
.cart-items-stream {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.cart-item-card {
  display: grid;
  grid-template-columns: 110px 1fr auto;
  gap: 1.5rem;
  padding: 1.4rem;
  border-radius: var(--radius-lg);
  align-items: center;
}

.item-thumb-box {
  width: 110px;
  height: 110px;
  border-radius: var(--radius-md);
  overflow: hidden;
  background: #0d0c15;
}

.item-thumb-box img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.item-main-details {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.item-info-top {
  display: flex;
  flex-direction: column;
  gap: 0.35rem;
}

.item-title {
  font-size: 1.15rem;
  font-weight: 700;
  letter-spacing: -0.01em;
}

.item-title a {
  color: #ffffff;
  text-decoration: none;
  transition: color var(--transition-fast);
}

.item-title a:hover {
  color: var(--accent-purple-bright);
}

.item-unit-price {
  font-size: 0.84rem;
  color: var(--text-muted);
}

.item-actions-row {
  display: flex;
  align-items: center;
  gap: 1.25rem;
}

.item-stepper {
  display: inline-flex;
  align-items: center;
  border: 1px solid var(--border-medium);
  border-radius: var(--radius-pill);
  background: rgba(255, 255, 255, 0.04);
}

.item-stepper button {
  width: 32px;
  height: 32px;
  border: 0;
  background: transparent;
  color: #ffffff;
  font-size: 1.1rem;
  cursor: pointer;
  display: grid;
  place-items: center;
}

.item-stepper button:disabled {
  opacity: 0.3;
  cursor: not-allowed;
}

.stepper-qty {
  width: 30px;
  text-align: center;
  font-weight: 700;
  font-size: 0.88rem;
}

.item-remove-btn {
  display: inline-flex;
  align-items: center;
  gap: 0.35rem;
  background: transparent;
  border: 0;
  color: var(--text-muted);
  font-size: 0.78rem;
  font-weight: 600;
  cursor: pointer;
  transition: color var(--transition-fast);
}

.item-remove-btn:hover {
  color: #f87171;
}

.item-total-col {
  text-align: right;
}

.item-line-total {
  font-size: 1.25rem;
  font-weight: 800;
  color: #ffffff;
}

.cart-actions-bottom {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0.5rem 0;
}

.continue-shopping-link {
  color: var(--text-secondary);
  text-decoration: none;
  font-size: 0.88rem;
  font-weight: 600;
  transition: color var(--transition-fast);
}

.continue-shopping-link:hover {
  color: #ffffff;
}

.clear-cart-link {
  background: transparent;
  border: 0;
  color: var(--text-muted);
  font-size: 0.82rem;
  cursor: pointer;
  text-decoration: underline;
}

.clear-cart-link:hover {
  color: #fca5a5;
}

/* Right Summary Column */
.summary-card {
  position: sticky;
  top: 100px;
  padding: 2.2rem;
  border-radius: var(--radius-xl);
}

.summary-title {
  font-size: 1.45rem;
  font-weight: 800;
  letter-spacing: -0.02em;
  color: #ffffff;
  margin-bottom: 1.8rem;
}

.summary-breakdown {
  display: flex;
  flex-direction: column;
  gap: 1.1rem;
}

.breakdown-row {
  display: flex;
  align-items: center;
  justify-content: space-between;
  font-size: 0.92rem;
  color: var(--text-secondary);
}

.discount-row {
  color: #4ade80;
}

.free-shipping-tag {
  color: #4ade80;
  font-weight: 800;
}

.summary-divider {
  height: 1px;
  background: var(--border-subtle);
  margin: 0.6rem 0;
}

.total-row {
  font-size: 1.25rem;
  font-weight: 800;
  color: #ffffff;
}

.total-amount {
  font-size: 1.6rem;
  color: #ffffff;
  letter-spacing: -0.02em;
}

/* Coupon Box */
.coupon-box {
  margin: 2rem 0;
  padding: 1.2rem;
  border-radius: var(--radius-md);
  background: rgba(255, 255, 255, 0.03);
  border: 1px solid var(--border-subtle);
}

.coupon-label {
  display: block;
  font-size: 0.78rem;
  font-weight: 600;
  color: var(--text-secondary);
  margin-bottom: 0.65rem;
}

.coupon-input-group {
  display: flex;
  gap: 0.5rem;
}

.coupon-input-group input {
  flex: 1;
  padding: 0.65rem 0.95rem;
  border-radius: var(--radius-pill);
  background: rgba(255, 255, 255, 0.06);
  border: 1px solid var(--border-medium);
  color: #ffffff;
  font-size: 0.84rem;
  outline: none;
}

.coupon-input-group input:focus {
  border-color: var(--accent-purple-bright);
}

.apply-coupon-btn {
  padding: 0.65rem 1.2rem;
  border-radius: var(--radius-pill);
  background: #ffffff;
  color: #0a0a0f;
  font-weight: 700;
  font-size: 0.82rem;
  border: 0;
  cursor: pointer;
}

.apply-coupon-btn:disabled {
  opacity: 0.4;
  cursor: not-allowed;
}

.active-coupon-pill {
  display: block;
  font-size: 0.75rem;
  color: var(--accent-lime);
  margin-top: 0.6rem;
}

.checkout-action-btn {
  width: 100%;
  padding: 1.1rem;
  font-size: 1.05rem;
  margin-bottom: 1.8rem;
}

.summary-trust-bullets {
  display: flex;
  flex-direction: column;
  gap: 0.65rem;
  border-top: 1px solid var(--border-subtle);
  padding-top: 1.4rem;
}

.bullet-item {
  display: flex;
  align-items: center;
  gap: 0.65rem;
  font-size: 0.78rem;
  color: var(--text-muted);
}

/* Empty State */
.empty-cart-state {
  max-width: 540px;
  margin: 4rem auto;
  padding: 5rem 2.5rem;
  text-align: center;
  border-radius: var(--radius-xl);
}

.empty-bag-icon {
  width: 80px;
  height: 80px;
  border-radius: 50%;
  background: rgba(255, 255, 255, 0.04);
  color: var(--accent-purple-bright);
  display: grid;
  place-items: center;
  margin: 0 auto 1.5rem;
}

.empty-cart-state h2 {
  font-family: var(--font-display);
  font-size: 2rem;
  font-weight: 800;
  color: #ffffff;
  margin-bottom: 0.8rem;
}

.empty-cart-state p {
  color: var(--text-secondary);
  font-size: 0.95rem;
  margin-bottom: 2rem;
  line-height: 1.6;
}

.start-shopping-btn {
  padding: 1rem 2rem;
  font-size: 0.95rem;
}

@media (max-width: 992px) {
  .cart-layout-grid {
    grid-template-columns: 1fr;
  }
  .summary-card {
    position: static;
  }
}

@media (max-width: 560px) {
  .cart-item-card {
    grid-template-columns: 80px 1fr;
  }
  .item-total-col {
    grid-column: 2;
    text-align: left;
    margin-top: 0.5rem;
  }
}
</style>
