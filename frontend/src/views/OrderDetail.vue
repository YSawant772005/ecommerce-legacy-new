<template>
  <div class="order-detail-page">
    <div class="container">
      <router-link to="/account/orders" class="back-link-pill">
        <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
          <line x1="19" y1="12" x2="5" y2="12"></line>
          <polyline points="12 19 5 12 12 5"></polyline>
        </svg>
        <span>Back to all orders</span>
      </router-link>

      <div v-if="ordersStore.loading" class="detail-loading-box">
        <div class="skeleton-shimmer" style="height: 400px; border-radius: var(--radius-xl);"></div>
      </div>

      <div v-else-if="ordersStore.currentOrder" class="order-detail-content">
        <!-- Header Banner -->
        <header class="detail-header-card glass-panel">
          <div class="header-left">
            <span class="detail-eyebrow">PURCHASE RECEIPT</span>
            <h1 class="order-id-title">#{{ ordersStore.currentOrder.orderNumber }}</h1>
            <p class="order-date-meta">Placed on {{ new Date(ordersStore.currentOrder.createdOn).toLocaleDateString('en-IN', { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' }) }}</p>
          </div>

          <div class="header-right">
            <div class="status-pill-big" :class="getStatusClass(ordersStore.currentOrder.status)">
              <span class="dot-indicator"></span>
              <span>{{ ordersStore.currentOrder.status }}</span>
            </div>
          </div>
        </header>

        <!-- Status Progress Timeline Tracker -->
        <div class="timeline-card glass-panel">
          <h2 class="timeline-title">Fulfillment Progress</h2>
          <div class="timeline-track">
            <div class="timeline-step" :class="{ 'done': isStepDone(1), 'active': isStepActive(1) }">
              <div class="step-circle">1</div>
              <span class="step-label">Order Placed</span>
            </div>
            <div class="timeline-line" :class="{ 'done': isStepDone(2) }"></div>
            <div class="timeline-step" :class="{ 'done': isStepDone(2), 'active': isStepActive(2) }">
              <div class="step-circle">2</div>
              <span class="step-label">Packed & Inspected</span>
            </div>
            <div class="timeline-line" :class="{ 'done': isStepDone(3) }"></div>
            <div class="timeline-step" :class="{ 'done': isStepDone(3), 'active': isStepActive(3) }">
              <div class="step-circle">3</div>
              <span class="step-label">Dispatched</span>
            </div>
            <div class="timeline-line" :class="{ 'done': isStepDone(4) }"></div>
            <div class="timeline-step" :class="{ 'done': isStepDone(4), 'active': isStepActive(4) }">
              <div class="step-circle">4</div>
              <span class="step-label">Delivered</span>
            </div>
          </div>
        </div>

        <!-- Order Items & Billing Layout -->
        <div class="order-detail-grid">
          <!-- Items List Column -->
          <div class="detail-items-col">
            <div class="detail-card glass-panel">
              <h2 class="card-section-title">Purchased Items ({{ ordersStore.currentOrder.items.length }})</h2>
              <OrderItems :items="ordersStore.currentOrder.items" />
            </div>
          </div>

          <!-- Billing & Payment Column -->
          <div class="detail-summary-col">
            <div class="detail-card glass-panel">
              <h2 class="card-section-title">Payment Summary</h2>

              <div class="summary-breakdown-list">
                <div class="summary-row">
                  <span>Subtotal</span>
                  <span>₹{{ ordersStore.currentOrder.subtotal.toLocaleString('en-IN') }}</span>
                </div>

                <div class="summary-row discount" v-if="ordersStore.currentOrder.discount > 0">
                  <span>Promotional Discount</span>
                  <span>-₹{{ ordersStore.currentOrder.discount.toLocaleString('en-IN') }}</span>
                </div>

                <div class="summary-row">
                  <span>Shipping Fee</span>
                  <span v-if="ordersStore.currentOrder.shipping === 0" class="free-text">FREE</span>
                  <span v-else>₹{{ ordersStore.currentOrder.shipping }}</span>
                </div>

                <div class="summary-row">
                  <span>GST Tax (18%)</span>
                  <span>₹{{ ordersStore.currentOrder.tax.toLocaleString('en-IN') }}</span>
                </div>

                <div class="summary-divider"></div>

                <div class="summary-row grand-total">
                  <span>Total Paid</span>
                  <span class="total-highlight">₹{{ ordersStore.currentOrder.total.toLocaleString('en-IN') }}</span>
                </div>
              </div>

              <!-- Customer Details -->
              <div class="recipient-box">
                <h4>Customer Details</h4>
                <p><strong>Name:</strong> {{ ordersStore.currentOrder.fullName }}</p>
                <p><strong>Email:</strong> {{ ordersStore.currentOrder.email }}</p>
                <p v-if="ordersStore.currentOrder.paymentMethod">
                  <strong>Payment Method:</strong> {{ ordersStore.currentOrder.paymentMethod }}
                </p>
                <p v-if="ordersStore.currentOrder.couponCode">
                  <strong>Coupon Applied:</strong> {{ ordersStore.currentOrder.couponCode }}
                </p>
              </div>
            </div>
          </div>
        </div>
      </div>

      <div v-else class="order-not-found glass-panel">
        <h2>Order Not Found</h2>
        <p>The requested order identifier could not be located in your account history.</p>
        <router-link to="/account/orders" class="btn-pill btn-pill-primary">Back to Orders</router-link>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { useOrdersStore } from '../stores/orders'
import OrderItems from '../components/OrderItems.vue'

const route = useRoute()
const ordersStore = useOrdersStore()

function getStatusClass(status: string) {
  const s = (status || '').toLowerCase()
  if (s.includes('delivered')) return 'status-delivered'
  if (s.includes('shipped')) return 'status-shipped'
  if (s.includes('packed')) return 'status-packed'
  if (s.includes('cancel')) return 'status-cancelled'
  return 'status-placed'
}

function getStepNumber(status: string): number {
  const s = (status || '').toLowerCase()
  if (s.includes('delivered')) return 4
  if (s.includes('shipped')) return 3
  if (s.includes('packed')) return 2
  return 1
}

function isStepDone(step: number): boolean {
  if (!ordersStore.currentOrder) return false
  return getStepNumber(ordersStore.currentOrder.status) >= step
}

function isStepActive(step: number): boolean {
  if (!ordersStore.currentOrder) return false
  return getStepNumber(ordersStore.currentOrder.status) === step
}

onMounted(() => {
  ordersStore.fetchOrder(Number(route.params.id))
})
</script>

<style scoped>
.order-detail-page {
  min-height: 100vh;
  padding: 3.5rem 0 6rem;
  background-color: var(--bg-main);
  color: var(--text-primary);
}

.back-link-pill {
  display: inline-flex;
  align-items: center;
  gap: 0.6rem;
  padding: 0.5rem 1rem;
  border-radius: var(--radius-pill);
  background: rgba(255, 255, 255, 0.05);
  border: 1px solid var(--border-subtle);
  color: var(--text-secondary);
  text-decoration: none;
  font-size: 0.84rem;
  font-weight: 600;
  margin-bottom: 2rem;
  transition: all var(--transition-fast);
}

.back-link-pill:hover {
  background: rgba(255, 255, 255, 0.1);
  color: #ffffff;
}

.detail-header-card {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 2.5rem;
  border-radius: var(--radius-xl);
  margin-bottom: 2rem;
}

.detail-eyebrow {
  display: block;
  font-size: 0.72rem;
  font-weight: 800;
  letter-spacing: 0.15em;
  color: var(--accent-lime);
  margin-bottom: 0.4rem;
}

.order-id-title {
  font-family: var(--font-display);
  font-size: clamp(2.4rem, 4.5vw, 3.8rem);
  font-weight: 900;
  line-height: 1;
  color: #ffffff;
  margin-bottom: 0.5rem;
}

.order-date-meta {
  color: var(--text-muted);
  font-size: 0.88rem;
  margin: 0;
}

.status-pill-big {
  display: inline-flex;
  align-items: center;
  gap: 0.65rem;
  padding: 0.6rem 1.4rem;
  border-radius: var(--radius-pill);
  font-size: 0.95rem;
  font-weight: 800;
  text-transform: capitalize;
}

.dot-indicator {
  width: 8px;
  height: 8px;
  border-radius: 50%;
  background: currentColor;
}

.status-delivered {
  background: rgba(34, 197, 94, 0.15);
  color: #4ade80;
  border: 1px solid rgba(34, 197, 94, 0.35);
}

.status-shipped {
  background: rgba(6, 182, 212, 0.15);
  color: #22d3ee;
  border: 1px solid rgba(6, 182, 212, 0.35);
}

.status-packed {
  background: rgba(245, 158, 11, 0.15);
  color: #fbbf24;
  border: 1px solid rgba(245, 158, 11, 0.35);
}

.status-placed {
  background: rgba(168, 85, 247, 0.15);
  color: #c084fc;
  border: 1px solid rgba(168, 85, 247, 0.35);
}

.status-cancelled {
  background: rgba(239, 68, 68, 0.15);
  color: #f87171;
  border: 1px solid rgba(239, 68, 68, 0.35);
}

/* Timeline */
.timeline-card {
  padding: 2rem 2.5rem;
  border-radius: var(--radius-xl);
  margin-bottom: 2.5rem;
}

.timeline-title {
  font-size: 1.15rem;
  font-weight: 800;
  color: #ffffff;
  margin-bottom: 2rem;
}

.timeline-track {
  display: flex;
  align-items: center;
  justify-content: space-between;
}

.timeline-step {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 0.5rem;
  position: relative;
  z-index: 1;
}

.step-circle {
  width: 36px;
  height: 36px;
  border-radius: 50%;
  background: rgba(255, 255, 255, 0.05);
  border: 2px solid var(--border-medium);
  color: var(--text-muted);
  display: grid;
  place-items: center;
  font-weight: 800;
  font-size: 0.88rem;
}

.timeline-step.done .step-circle {
  background: var(--accent-lime);
  border-color: var(--accent-lime);
  color: #052e16;
}

.timeline-step.active .step-circle {
  background: #ffffff;
  border-color: #ffffff;
  color: #0a0a0f;
  box-shadow: 0 0 15px rgba(255, 255, 255, 0.35);
}

.step-label {
  font-size: 0.78rem;
  font-weight: 600;
  color: var(--text-muted);
  white-space: nowrap;
}

.timeline-step.done .step-label,
.timeline-step.active .step-label {
  color: #ffffff;
}

.timeline-line {
  flex: 1;
  height: 2px;
  background: var(--border-subtle);
  margin: 0 0.5rem 1.4rem;
}

.timeline-line.done {
  background: var(--accent-lime);
}

/* Detail Grid */
.order-detail-grid {
  display: grid;
  grid-template-columns: 1.3fr 0.8fr;
  gap: 2.5rem;
}

.detail-card {
  padding: 2.2rem;
  border-radius: var(--radius-xl);
}

.card-section-title {
  font-size: 1.25rem;
  font-weight: 800;
  color: #ffffff;
  margin-bottom: 1.5rem;
}

.summary-breakdown-list {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.summary-row {
  display: flex;
  justify-content: space-between;
  font-size: 0.9rem;
  color: var(--text-secondary);
}

.summary-row.discount {
  color: #4ade80;
}

.free-text {
  color: #4ade80;
  font-weight: 800;
}

.summary-divider {
  height: 1px;
  background: var(--border-subtle);
  margin: 0.5rem 0;
}

.summary-row.grand-total {
  font-size: 1.15rem;
  font-weight: 800;
  color: #ffffff;
}

.total-highlight {
  font-size: 1.45rem;
  color: #ffffff;
}

.recipient-box {
  margin-top: 2rem;
  padding-top: 1.5rem;
  border-top: 1px solid var(--border-subtle);
  font-size: 0.84rem;
  color: var(--text-secondary);
  display: flex;
  flex-direction: column;
  gap: 0.4rem;
}

.recipient-box h4 {
  font-size: 0.95rem;
  color: #ffffff;
  margin-bottom: 0.5rem;
}

.order-not-found {
  max-width: 500px;
  margin: 4rem auto;
  padding: 4rem 2rem;
  text-align: center;
  border-radius: var(--radius-xl);
}

@media (max-width: 900px) {
  .order-detail-grid {
    grid-template-columns: 1fr;
  }
  .detail-header-card {
    flex-direction: column;
    align-items: flex-start;
    gap: 1.5rem;
  }
}

@media (max-width: 600px) {
  .timeline-track {
    flex-direction: column;
    align-items: flex-start;
    gap: 1.5rem;
  }
  .timeline-line {
    width: 2px;
    height: 30px;
    margin: 0 0 0 17px;
  }
  .timeline-step {
    flex-direction: row;
    gap: 1rem;
  }
}
</style>
