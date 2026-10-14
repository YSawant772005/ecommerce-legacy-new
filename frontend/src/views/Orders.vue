<template>
  <div class="orders-page">
    <div class="container">
      <header class="orders-header">
        <div class="orders-eyebrow">
          <span class="eyebrow-dot"></span>
          <span>ACCOUNT OVERVIEW</span>
        </div>
        <h1 class="orders-title">
          Order history<span class="dot-accent">.</span>
        </h1>
        <p class="orders-subtitle">Track, review, and manage your recent purchases.</p>
      </header>

      <div v-if="!ordersStore.loading && ordersStore.orders.length > 0" class="orders-list">
        <router-link 
          v-for="order in ordersStore.orders" 
          :key="order.id" 
          :to="`/account/orders/${order.id}`" 
          class="order-summary-card glass-card"
        >
          <div class="order-card-header">
            <div class="order-num-meta">
              <span class="order-date-tag">PLACED {{ new Date(order.createdOn).toLocaleDateString() }}</span>
              <h2 class="order-number-title">#{{ order.orderNumber }}</h2>
            </div>
            
            <div class="order-status-badge" :class="getStatusClass(order.status)">
              <span class="status-dot"></span>
              <span>{{ order.status }}</span>
            </div>
          </div>

          <div class="order-items-preview-wrap">
            <OrderItems :items="order.items" />
          </div>

          <div class="order-card-footer">
            <span class="order-item-count">
              {{ order.items.length }} item{{ order.items.length === 1 ? '' : 's' }}
            </span>
            <div class="order-total-callout">
              <span class="total-label">Total Amount:</span>
              <strong class="total-val">₹{{ order.total.toLocaleString('en-IN') }}</strong>
            </div>
            <span class="view-order-details-link">View Details →</span>
          </div>
        </router-link>
      </div>

      <!-- Loading State -->
      <div v-else-if="ordersStore.loading" class="orders-list">
        <div v-for="n in 3" :key="n" class="skeleton-shimmer" style="height: 220px; border-radius: var(--radius-lg); margin-bottom: 1.5rem;"></div>
      </div>

      <!-- Empty State -->
      <div v-else class="empty-orders-state glass-panel">
        <div class="empty-icon-circle">📦</div>
        <h2>No orders placed yet</h2>
        <p>Your orders will appear here once you make your first purchase with NovaKart.</p>
        <router-link to="/products" class="btn-pill btn-pill-primary">Explore Catalogue</router-link>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted } from 'vue'
import { useOrdersStore } from '../stores/orders'
import OrderItems from '../components/OrderItems.vue'

const ordersStore = useOrdersStore()

function getStatusClass(status: string) {
  const s = status.toLowerCase()
  if (s.includes('delivered')) return 'status-delivered'
  if (s.includes('shipped')) return 'status-shipped'
  if (s.includes('packed')) return 'status-packed'
  if (s.includes('cancel')) return 'status-cancelled'
  return 'status-placed'
}

onMounted(() => {
  ordersStore.fetchOrders()
})
</script>

<style scoped>
.orders-page {
  min-height: 100vh;
  padding: 3.5rem 0 6rem;
  background-color: var(--bg-main);
  color: var(--text-primary);
}

.orders-header {
  margin-bottom: 3.5rem;
}

.orders-eyebrow {
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

.orders-title {
  font-family: var(--font-display);
  font-size: clamp(2.8rem, 5.5vw, 4.4rem);
  font-weight: 900;
  line-height: 0.95;
  letter-spacing: -0.04em;
  color: #ffffff;
  margin-bottom: 0.8rem;
}

.orders-subtitle {
  font-size: 1.05rem;
  color: var(--text-secondary);
}

.orders-list {
  display: flex;
  flex-direction: column;
  gap: 1.75rem;
}

.order-summary-card {
  display: block;
  padding: 2rem;
  border-radius: var(--radius-xl);
  text-decoration: none;
  color: inherit;
  transition: all var(--transition-base);
}

.order-summary-card:hover {
  transform: translateY(-4px);
  border-color: rgba(255, 255, 255, 0.25);
  box-shadow: 0 20px 40px rgba(0, 0, 0, 0.4);
}

.order-card-header {
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  margin-bottom: 1.5rem;
  padding-bottom: 1.25rem;
  border-bottom: 1px solid var(--border-subtle);
}

.order-date-tag {
  display: block;
  font-size: 0.72rem;
  font-weight: 800;
  letter-spacing: 0.08em;
  color: var(--accent-purple-bright);
  margin-bottom: 0.35rem;
}

.order-number-title {
  font-family: var(--font-display);
  font-size: 1.5rem;
  font-weight: 800;
  letter-spacing: -0.02em;
  color: #ffffff;
}

.order-status-badge {
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.35rem 0.85rem;
  border-radius: var(--radius-pill);
  font-size: 0.78rem;
  font-weight: 700;
  text-transform: capitalize;
}

.status-dot {
  width: 6px;
  height: 6px;
  border-radius: 50%;
}

.status-delivered {
  background: rgba(34, 197, 94, 0.15);
  color: #4ade80;
  border: 1px solid rgba(34, 197, 94, 0.3);
}
.status-delivered .status-dot { background: #4ade80; }

.status-shipped {
  background: rgba(6, 182, 212, 0.15);
  color: #22d3ee;
  border: 1px solid rgba(6, 182, 212, 0.3);
}
.status-shipped .status-dot { background: #22d3ee; }

.status-packed {
  background: rgba(245, 158, 11, 0.15);
  color: #fbbf24;
  border: 1px solid rgba(245, 158, 11, 0.3);
}
.status-packed .status-dot { background: #fbbf24; }

.status-placed {
  background: rgba(168, 85, 247, 0.15);
  color: #c084fc;
  border: 1px solid rgba(168, 85, 247, 0.3);
}
.status-placed .status-dot { background: #c084fc; }

.status-cancelled {
  background: rgba(239, 68, 68, 0.15);
  color: #f87171;
  border: 1px solid rgba(239, 68, 68, 0.3);
}
.status-cancelled .status-dot { background: #f87171; }

.order-items-preview-wrap {
  margin-bottom: 1.5rem;
}

.order-card-footer {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding-top: 1.25rem;
  border-top: 1px solid var(--border-subtle);
}

.order-item-count {
  font-size: 0.85rem;
  color: var(--text-muted);
}

.order-total-callout {
  display: flex;
  align-items: baseline;
  gap: 0.5rem;
}

.total-label {
  font-size: 0.85rem;
  color: var(--text-secondary);
}

.total-val {
  font-size: 1.35rem;
  font-weight: 900;
  color: #ffffff;
}

.view-order-details-link {
  font-size: 0.84rem;
  font-weight: 700;
  color: var(--accent-lime);
}

/* Empty State */
.empty-orders-state {
  max-width: 500px;
  margin: 3rem auto;
  padding: 4.5rem 2rem;
  text-align: center;
  border-radius: var(--radius-xl);
}

.empty-icon-circle {
  font-size: 2.2rem;
  width: 70px;
  height: 70px;
  border-radius: 50%;
  background: rgba(255, 255, 255, 0.05);
  display: grid;
  place-items: center;
  margin: 0 auto 1.5rem;
}

.empty-orders-state h2 {
  font-size: 1.6rem;
  color: #ffffff;
  margin-bottom: 0.6rem;
}

.empty-orders-state p {
  color: var(--text-secondary);
  margin-bottom: 2rem;
  font-size: 0.92rem;
}

@media (max-width: 640px) {
  .order-card-footer {
    flex-direction: column;
    align-items: flex-start;
    gap: 0.75rem;
  }
}
</style>
