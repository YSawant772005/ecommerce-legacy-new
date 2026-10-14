<template>
  <div class="checkout-page">
    <div class="container">
      <!-- Breadcrumb / Step Indicator -->
      <header class="checkout-header">
        <div class="checkout-eyebrow">
          <span class="eyebrow-dot"></span>
          <span>SECURE ENCRYPTED CHECKOUT</span>
        </div>
        <h1 class="checkout-title">
          Final step. <em class="serif-accent">Make it yours.</em>
        </h1>

        <!-- Step Indicator -->
        <div class="step-indicator-bar">
          <div class="step-item completed">
            <span class="step-num">✓</span>
            <span class="step-label">Bag</span>
          </div>
          <div class="step-connector completed"></div>
          <div class="step-item active">
            <span class="step-num">2</span>
            <span class="step-label">Shipping & Payment</span>
          </div>
          <div class="step-connector"></div>
          <div class="step-item">
            <span class="step-num">3</span>
            <span class="step-label">Confirmation</span>
          </div>
        </div>
      </header>

      <!-- Active Checkout Form -->
      <form 
        @submit.prevent="handleCheckout" 
        v-if="cartStore.cart && cartStore.cart.items.length > 0"
        class="checkout-form-grid"
      >
        <!-- Left: Form Input Sections -->
        <div class="checkout-inputs-col">
          <!-- 1. Contact & Shipping Details -->
          <section class="checkout-section-card glass-panel">
            <div class="section-card-title">
              <span class="card-step-badge">1</span>
              <div>
                <h2>Shipping & Contact Details</h2>
                <p>Where should we deliver your order?</p>
              </div>
            </div>

            <div class="fields-grid">
              <div class="field-item span-2">
                <label for="c-fullname">Full Name</label>
                <input id="c-fullname" v-model="form.fullName" placeholder="e.g. John Doe" required />
              </div>

              <div class="field-item">
                <label for="c-email">Email Address</label>
                <input id="c-email" v-model="form.email" type="email" placeholder="you@domain.com" required />
              </div>

              <div class="field-item">
                <label for="c-phone">Phone Number</label>
                <input id="c-phone" v-model="form.phone" type="tel" placeholder="+91 98765 43210" />
              </div>

              <div class="field-item span-2">
                <label for="c-address">Street Address / Apartment</label>
                <input id="c-address" v-model="form.addressLine" placeholder="House/Flat number, Street name" required />
              </div>

              <div class="field-item">
                <label for="c-city">City</label>
                <input id="c-city" v-model="form.city" placeholder="e.g. Mumbai" required />
              </div>

              <div class="field-item">
                <label for="c-state">State</label>
                <input id="c-state" v-model="form.state" placeholder="e.g. Maharashtra" required />
              </div>

              <div class="field-item span-2">
                <label for="c-postal">Postal / PIN Code</label>
                <input id="c-postal" v-model="form.postalCode" placeholder="e.g. 400001" required />
              </div>
            </div>
          </section>

          <!-- 2. Payment Method Selector -->
          <section class="checkout-section-card glass-panel">
            <div class="section-card-title">
              <span class="card-step-badge">2</span>
              <div>
                <h2>Select Payment Method</h2>
                <p>All payments are processed securely over 256-bit encryption</p>
              </div>
            </div>

            <div class="payment-methods-stack" role="radiogroup" aria-label="Payment method">
              <!-- UPI -->
              <label class="payment-card" :class="{ 'active': form.paymentMethod === 'UPI' }">
                <input type="radio" v-model="form.paymentMethod" value="UPI" required class="sr-only" />
                <div class="payment-card-left">
                  <div class="payment-radio-circle"></div>
                  <div>
                    <strong>UPI (Google Pay, PhonePe, Paytm, QR)</strong>
                    <span>Instant payment confirmation via any UPI app</span>
                  </div>
                </div>
                <div class="payment-badge">Instant</div>
              </label>

              <!-- Card -->
              <label class="payment-card" :class="{ 'active': form.paymentMethod === 'Card' }">
                <input type="radio" v-model="form.paymentMethod" value="Card" class="sr-only" />
                <div class="payment-card-left">
                  <div class="payment-radio-circle"></div>
                  <div>
                    <strong>Credit / Debit Card</strong>
                    <span>Visa, Mastercard, RuPay, and American Express</span>
                  </div>
                </div>
                <div class="payment-badge">Secure</div>
              </label>

              <!-- COD -->
              <label class="payment-card" :class="{ 'active': form.paymentMethod === 'COD' }">
                <input type="radio" v-model="form.paymentMethod" value="COD" class="sr-only" />
                <div class="payment-card-left">
                  <div class="payment-radio-circle"></div>
                  <div>
                    <strong>Cash on Delivery (COD)</strong>
                    <span>Pay with cash or digital payment upon delivery</span>
                  </div>
                </div>
                <div class="payment-badge">Flexible</div>
              </label>
            </div>
          </section>
        </div>

        <!-- Right: Sticky Order Summary & Pay Button -->
        <aside class="checkout-summary-col">
          <div class="checkout-summary-card glass-panel">
            <h2 class="summary-card-heading">Order Review</h2>

            <!-- Quick Item Preview -->
            <div class="checkout-items-preview">
              <div v-for="item in cartStore.cart.items" :key="item.productId" class="preview-item-row">
                <div class="preview-item-thumb">
                  <img :src="productImage({ id: item.productId, name: item.productName, imageUrl: item.imageUrl })" :alt="item.productName" />
                  <span class="preview-qty-badge">{{ item.qty }}</span>
                </div>
                <div class="preview-item-info">
                  <strong>{{ item.productName }}</strong>
                  <span>₹{{ item.unitPrice.toLocaleString('en-IN') }} each</span>
                </div>
                <span class="preview-item-total">₹{{ item.lineTotal.toLocaleString('en-IN') }}</span>
              </div>
            </div>

            <!-- Cost Breakdown -->
            <div class="summary-cost-list">
              <div class="cost-row">
                <span>Subtotal</span>
                <span>₹{{ cartStore.cart.subtotal.toLocaleString('en-IN') }}</span>
              </div>

              <div class="cost-row discount" v-if="cartStore.cart.discount > 0">
                <span>Promotional Discount</span>
                <span>-₹{{ cartStore.cart.discount.toLocaleString('en-IN') }}</span>
              </div>

              <div class="cost-row">
                <span>Shipping</span>
                <span v-if="cartStore.cart.shipping === 0" class="free-badge">FREE</span>
                <span v-else>₹{{ cartStore.cart.shipping }}</span>
              </div>

              <div class="cost-row">
                <span>GST Tax (18%)</span>
                <span>₹{{ cartStore.cart.tax.toLocaleString('en-IN') }}</span>
              </div>

              <div class="cost-divider"></div>

              <div class="cost-row grand-total">
                <span>Grand Total</span>
                <span class="total-number">₹{{ cartStore.cart.total.toLocaleString('en-IN') }}</span>
              </div>
            </div>

            <!-- Error Banner -->
            <div v-if="errorMessage" class="checkout-error-banner" role="alert">
              <span>⚠️ {{ errorMessage }}</span>
            </div>

            <!-- Place Order Button -->
            <button 
              type="submit" 
              class="btn-pill btn-pill-primary submit-order-btn"
              :disabled="loading"
            >
              <span v-if="loading" class="loading-spin"></span>
              <span v-else class="btn-label-row">
                <span>Place Order • ₹{{ cartStore.cart.total.toLocaleString('en-IN') }}</span>
                <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
                  <polyline points="20 6 9 17 4 12"></polyline>
                </svg>
              </span>
            </button>

            <!-- Security Assurance -->
            <div class="checkout-guarantee-note">
              <span class="lock-icon">🔒</span>
              <p>Your details are protected by industry-standard 256-bit encryption. By placing this order, you agree to NovaKart Terms.</p>
            </div>
          </div>
        </aside>
      </form>

      <!-- Empty Cart Fallback -->
      <div v-else class="empty-checkout-card glass-panel">
        <h2>Your bag is currently empty</h2>
        <p>You need items in your cart before you can proceed with checkout.</p>
        <router-link to="/products" class="btn-pill btn-pill-primary">Browse Catalogue</router-link>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useCartStore } from '../stores/cart'
import { useAuthStore } from '../stores/auth'
import { useOrdersStore } from '../stores/orders'
import { productImage } from '../utils/productImage'

const router = useRouter()
const cartStore = useCartStore()
const authStore = useAuthStore()
const ordersStore = useOrdersStore()

const form = ref({
  fullName: authStore.user?.fullName || '',
  email: authStore.user?.email || '',
  phone: authStore.user?.phone || '',
  addressLine: '',
  city: '',
  state: '',
  postalCode: '',
  paymentMethod: 'UPI'
})

const loading = ref(false)
const errorMessage = ref('')

async function handleCheckout() {
  loading.value = true
  errorMessage.value = ''
  try {
    await ordersStore.createOrder({
      ...form.value,
      couponCode: cartStore.cart?.couponCode
    })
    router.push('/account/orders')
  } catch (e: any) {
    errorMessage.value = e.response?.data?.error || 'Unable to place order. Please review your details and try again.'
  } finally {
    loading.value = false
  }
}

onMounted(() => {
  cartStore.fetchCart()
})
</script>

<style scoped>
.checkout-page {
  min-height: 100vh;
  padding: 3.5rem 0 6rem;
  background-color: var(--bg-main);
  color: var(--text-primary);
}

.checkout-header {
  margin-bottom: 3.5rem;
}

.checkout-eyebrow {
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

.checkout-title {
  font-family: var(--font-display);
  font-size: clamp(2.6rem, 5vw, 4.2rem);
  font-weight: 900;
  line-height: 0.95;
  letter-spacing: -0.04em;
  color: #ffffff;
  margin-bottom: 2.2rem;
}

.checkout-title em {
  color: var(--accent-purple-bright);
}

/* Step Bar */
.step-indicator-bar {
  display: flex;
  align-items: center;
  max-width: 520px;
}

.step-item {
  display: flex;
  align-items: center;
  gap: 0.65rem;
}

.step-num {
  width: 28px;
  height: 28px;
  border-radius: 50%;
  border: 1.5px solid var(--border-medium);
  background: rgba(255, 255, 255, 0.04);
  color: var(--text-muted);
  display: grid;
  place-items: center;
  font-size: 0.78rem;
  font-weight: 700;
}

.step-item.active .step-num {
  border-color: #ffffff;
  background: #ffffff;
  color: #0a0a0f;
}

.step-item.completed .step-num {
  border-color: var(--accent-lime);
  background: var(--accent-lime);
  color: #052e16;
}

.step-label {
  font-size: 0.82rem;
  font-weight: 600;
  color: var(--text-muted);
}

.step-item.active .step-label {
  color: #ffffff;
}

.step-connector {
  flex: 1;
  height: 2px;
  background: var(--border-subtle);
  margin: 0 1rem;
}

.step-connector.completed {
  background: var(--accent-lime);
}

/* Form Layout */
.checkout-form-grid {
  display: grid;
  grid-template-columns: 1.35fr 0.85fr;
  gap: 3.5rem;
  align-items: flex-start;
}

.checkout-inputs-col {
  display: flex;
  flex-direction: column;
  gap: 2rem;
}

.checkout-section-card {
  padding: 2.5rem;
  border-radius: var(--radius-xl);
}

.section-card-title {
  display: flex;
  align-items: flex-start;
  gap: 1.2rem;
  margin-bottom: 2rem;
}

.card-step-badge {
  width: 32px;
  height: 32px;
  border-radius: 50%;
  background: rgba(168, 85, 247, 0.2);
  border: 1px solid rgba(168, 85, 247, 0.4);
  color: #c084fc;
  display: grid;
  place-items: center;
  font-weight: 800;
  font-size: 0.88rem;
  flex-shrink: 0;
}

.section-card-title h2 {
  font-size: 1.35rem;
  font-weight: 800;
  letter-spacing: -0.02em;
  color: #ffffff;
  margin-bottom: 0.25rem;
}

.section-card-title p {
  font-size: 0.84rem;
  color: var(--text-muted);
  margin: 0;
}

/* Fields Grid */
.fields-grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 1.25rem;
}

.field-item {
  display: flex;
  flex-direction: column;
  gap: 0.45rem;
}

.field-item.span-2 {
  grid-column: span 2;
}

.field-item label {
  font-size: 0.8rem;
  font-weight: 600;
  color: var(--text-secondary);
}

.field-item input {
  padding: 0.85rem 1.1rem;
  border-radius: var(--radius-md);
  background: rgba(255, 255, 255, 0.04);
  border: 1px solid var(--border-medium);
  color: #ffffff;
  font-family: inherit;
  font-size: 0.9rem;
  outline: none;
  transition: all var(--transition-fast);
}

.field-item input:focus {
  border-color: var(--accent-purple-bright);
  background: rgba(255, 255, 255, 0.08);
  box-shadow: 0 0 15px rgba(168, 85, 247, 0.25);
}

/* Payment Method Cards */
.payment-methods-stack {
  display: flex;
  flex-direction: column;
  gap: 0.85rem;
}

.payment-card {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 1.2rem 1.5rem;
  border-radius: var(--radius-md);
  background: rgba(255, 255, 255, 0.03);
  border: 1px solid var(--border-subtle);
  cursor: pointer;
  transition: all var(--transition-fast);
}

.payment-card:hover {
  background: rgba(255, 255, 255, 0.06);
  border-color: var(--border-medium);
}

.payment-card.active {
  background: rgba(168, 85, 247, 0.12);
  border-color: var(--accent-purple-bright);
  box-shadow: 0 0 20px rgba(168, 85, 247, 0.2);
}

.payment-card-left {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.payment-radio-circle {
  width: 20px;
  height: 20px;
  border-radius: 50%;
  border: 2px solid var(--border-medium);
  display: grid;
  place-items: center;
  flex-shrink: 0;
  transition: all var(--transition-fast);
}

.payment-card.active .payment-radio-circle {
  border-color: var(--accent-purple-bright);
  background: var(--accent-purple-bright);
  box-shadow: inset 0 0 0 3px #0a0a0f;
}

.payment-card-left strong {
  display: block;
  font-size: 0.92rem;
  color: #ffffff;
  margin-bottom: 0.15rem;
}

.payment-card-left span {
  font-size: 0.78rem;
  color: var(--text-muted);
}

.payment-badge {
  font-size: 0.7rem;
  font-weight: 800;
  padding: 0.25rem 0.65rem;
  border-radius: var(--radius-pill);
  background: rgba(255, 255, 255, 0.08);
  color: var(--text-secondary);
}

.payment-card.active .payment-badge {
  background: var(--accent-purple-bright);
  color: #ffffff;
}

/* Right Summary Column */
.checkout-summary-card {
  position: sticky;
  top: 100px;
  padding: 2.2rem;
  border-radius: var(--radius-xl);
}

.summary-card-heading {
  font-size: 1.35rem;
  font-weight: 800;
  letter-spacing: -0.02em;
  color: #ffffff;
  margin-bottom: 1.5rem;
}

.checkout-items-preview {
  display: flex;
  flex-direction: column;
  gap: 0.85rem;
  max-height: 220px;
  overflow-y: auto;
  padding-right: 0.5rem;
  margin-bottom: 1.5rem;
}

.preview-item-row {
  display: flex;
  align-items: center;
  gap: 0.85rem;
  padding-bottom: 0.85rem;
  border-bottom: 1px solid rgba(255, 255, 255, 0.05);
}

.preview-item-thumb {
  position: relative;
  width: 50px;
  height: 50px;
  border-radius: var(--radius-sm);
  overflow: hidden;
  background: #0d0c15;
  flex-shrink: 0;
}

.preview-item-thumb img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.preview-qty-badge {
  position: absolute;
  top: 2px;
  right: 2px;
  background: rgba(0, 0, 0, 0.8);
  color: #ffffff;
  font-size: 0.65rem;
  font-weight: 800;
  padding: 1px 5px;
  border-radius: var(--radius-pill);
}

.preview-item-info {
  flex: 1;
  min-width: 0;
}

.preview-item-info strong {
  display: block;
  font-size: 0.84rem;
  color: #ffffff;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

.preview-item-info span {
  font-size: 0.74rem;
  color: var(--text-muted);
}

.preview-item-total {
  font-size: 0.88rem;
  font-weight: 700;
  color: #ffffff;
}

/* Cost Breakdown */
.summary-cost-list {
  display: flex;
  flex-direction: column;
  gap: 0.95rem;
  margin-bottom: 1.8rem;
}

.cost-row {
  display: flex;
  justify-content: space-between;
  font-size: 0.88rem;
  color: var(--text-secondary);
}

.cost-row.discount {
  color: #4ade80;
}

.free-badge {
  color: #4ade80;
  font-weight: 800;
}

.cost-divider {
  height: 1px;
  background: var(--border-subtle);
  margin: 0.4rem 0;
}

.cost-row.grand-total {
  font-size: 1.15rem;
  font-weight: 800;
  color: #ffffff;
}

.total-number {
  font-size: 1.5rem;
  color: #ffffff;
  letter-spacing: -0.02em;
}

.checkout-error-banner {
  padding: 0.85rem 1rem;
  border-radius: var(--radius-md);
  background: rgba(239, 68, 68, 0.15);
  border: 1px solid rgba(239, 68, 68, 0.3);
  color: #fca5a5;
  font-size: 0.82rem;
  margin-bottom: 1.25rem;
}

.submit-order-btn {
  width: 100%;
  padding: 1.1rem;
  font-size: 1rem;
}

.btn-label-row {
  display: flex;
  align-items: center;
  gap: 0.6rem;
}

.checkout-guarantee-note {
  display: flex;
  align-items: flex-start;
  gap: 0.65rem;
  margin-top: 1.5rem;
  padding-top: 1.2rem;
  border-top: 1px solid var(--border-subtle);
}

.lock-icon {
  font-size: 0.9rem;
}

.checkout-guarantee-note p {
  font-size: 0.72rem;
  color: var(--text-muted);
  line-height: 1.5;
  margin: 0;
}

/* Empty State */
.empty-checkout-card {
  max-width: 500px;
  margin: 4rem auto;
  padding: 4rem 2rem;
  text-align: center;
  border-radius: var(--radius-xl);
}

.empty-checkout-card h2 {
  font-size: 1.8rem;
  color: #ffffff;
  margin-bottom: 0.75rem;
}

.empty-checkout-card p {
  color: var(--text-secondary);
  margin-bottom: 2rem;
}

@media (max-width: 992px) {
  .checkout-form-grid {
    grid-template-columns: 1fr;
  }
  .checkout-summary-card {
    position: static;
  }
}

@media (max-width: 600px) {
  .fields-grid {
    grid-template-columns: 1fr;
  }
  .field-item.span-2 {
    grid-column: span 1;
  }
  .checkout-section-card {
    padding: 1.8rem 1.4rem;
  }
}
</style>
