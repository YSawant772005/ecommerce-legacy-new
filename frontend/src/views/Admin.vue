<template>
  <div class="admin-dashboard-page">
    <div class="container">
      <header class="admin-header">
        <div class="admin-eyebrow">
          <span class="eyebrow-dot"></span>
          <span>MISSION CONTROL • ADMIN PORTAL</span>
        </div>
        <h1 class="admin-title">
          Good morning,<br />
          <span class="name-gradient">Administrator.</span>
        </h1>
        <p class="admin-subtitle">Live real-time telemetry, store metrics, and recent order transactions.</p>
      </header>

      <!-- Stats Grid -->
      <div class="stats-grid" v-if="dashboard">
        <div class="stat-card glass-panel revenue-card">
          <div class="stat-card-top">
            <span class="stat-label">Total Revenue</span>
            <span class="stat-icon-box">💰</span>
          </div>
          <div class="stat-main-value">₹{{ dashboard.totalRevenue.toLocaleString('en-IN') }}</div>
          <span class="stat-trend positive">✦ All-time revenue</span>
        </div>

        <div class="stat-card glass-panel monthly-card">
          <div class="stat-card-top">
            <span class="stat-label">Monthly Run-rate</span>
            <span class="stat-icon-box">📈</span>
          </div>
          <div class="stat-main-value">₹{{ dashboard.monthlyRevenue.toLocaleString('en-IN') }}</div>
          <span class="stat-trend">Current calendar month</span>
        </div>

        <div class="stat-card glass-panel">
          <div class="stat-card-top">
            <span class="stat-label">Total Orders</span>
            <span class="stat-icon-box">📦</span>
          </div>
          <div class="stat-main-value">{{ dashboard.totalOrders }}</div>
          <span class="stat-trend">Across all channels</span>
        </div>

        <div class="stat-card glass-panel">
          <div class="stat-card-top">
            <span class="stat-label">Total Customers</span>
            <span class="stat-icon-box">👥</span>
          </div>
          <div class="stat-main-value">{{ dashboard.totalCustomers }}</div>
          <span class="stat-trend">Registered accounts</span>
        </div>

        <div class="stat-card glass-panel">
          <div class="stat-card-top">
            <span class="stat-label">Active Catalogue</span>
            <span class="stat-icon-box">🏷️</span>
          </div>
          <div class="stat-main-value">{{ dashboard.totalProducts }}</div>
          <span class="stat-trend">Live SKUs listed</span>
        </div>

        <div class="stat-card glass-panel warning-stat" :class="{ 'alert-glow': dashboard.lowStock > 0 }">
          <div class="stat-card-top">
            <span class="stat-label">Low Stock Alerts</span>
            <span class="stat-icon-box">⚠️</span>
          </div>
          <div class="stat-main-value warning-text">{{ dashboard.lowStock }}</div>
          <span class="stat-trend warning">SKUs requiring restock</span>
        </div>
      </div>

      <!-- Loading skeleton -->
      <div v-else class="stats-grid">
        <div v-for="n in 6" :key="n" class="skeleton-shimmer" style="height: 150px; border-radius: var(--radius-xl);"></div>
      </div>

      <!-- Quick Admin Navigation Grid -->
      <section class="admin-section-block">
        <h2 class="section-title">Control Modules</h2>
        <div class="admin-modules-grid">
          <router-link to="/admin/products" class="module-card glass-card">
            <span class="module-icon">🛍️</span>
            <div>
              <h3>Manage Products</h3>
              <p>Add, edit prices, update inventory & images</p>
            </div>
            <span class="module-arrow">→</span>
          </router-link>

          <router-link to="/admin/orders" class="module-card glass-card">
            <span class="module-icon">📋</span>
            <div>
              <h3>Manage Orders</h3>
              <p>Fulfill, dispatch, track & change order statuses</p>
            </div>
            <span class="module-arrow">→</span>
          </router-link>

          <router-link to="/admin/categories" class="module-card glass-card">
            <span class="module-icon">📁</span>
            <div>
              <h3>Categories</h3>
              <p>Curate moods, classifications & tags</p>
            </div>
            <span class="module-arrow">→</span>
          </router-link>

          <router-link to="/admin/brands" class="module-card glass-card">
            <span class="module-icon">✨</span>
            <div>
              <h3>Brands</h3>
              <p>Partner manufacturers & brand identities</p>
            </div>
            <span class="module-arrow">→</span>
          </router-link>

          <router-link to="/admin/customers" class="module-card glass-card">
            <span class="module-icon">👤</span>
            <div>
              <h3>Customers</h3>
              <p>View verified buyer directories & accounts</p>
            </div>
            <span class="module-arrow">→</span>
          </router-link>

          <router-link to="/admin/coupons" class="module-card glass-card">
            <span class="module-icon">🎟️</span>
            <div>
              <h3>Coupons & Deals</h3>
              <p>Promotional codes, discounts & caps</p>
            </div>
            <span class="module-arrow">→</span>
          </router-link>

          <router-link to="/admin/reviews" class="module-card glass-card">
            <span class="module-icon">⭐</span>
            <div>
              <h3>Product Reviews</h3>
              <p>Moderate ratings, feedback & testimonials</p>
            </div>
            <span class="module-arrow">→</span>
          </router-link>

          <router-link to="/admin/messages" class="module-card glass-card">
            <span class="module-icon">💬</span>
            <div>
              <h3>Customer Messages</h3>
              <p>Inquiries, support queries & concierge notes</p>
            </div>
            <span class="module-arrow">→</span>
          </router-link>

          <router-link to="/admin/newsletter" class="module-card glass-card">
            <span class="module-icon">📬</span>
            <div>
              <h3>Subscribers</h3>
              <p>Newsletter member list & marketing export</p>
            </div>
            <span class="module-arrow">→</span>
          </router-link>
        </div>
      </section>

      <!-- Recent Orders Table -->
      <section class="admin-section-block" v-if="dashboard && dashboard.recentOrders">
        <div class="table-header-row">
          <div>
            <h2 class="section-title">Recent Transactions</h2>
            <p class="section-desc">Latest customer orders placed through storefront</p>
          </div>
          <router-link to="/admin/orders" class="btn-pill btn-pill-outline btn-sm">
            View All Orders →
          </router-link>
        </div>

        <div class="table-container glass-panel">
          <table class="nova-admin-table">
            <thead>
              <tr>
                <th>Order Number</th>
                <th>Customer</th>
                <th>Total Value</th>
                <th>Status</th>
                <th>Date Placed</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="order in dashboard.recentOrders" :key="order.id">
                <td>
                  <strong class="order-num-link">#{{ order.orderNumber }}</strong>
                </td>
                <td class="customer-email-cell">{{ order.email }}</td>
                <td>
                  <strong class="order-price-val">₹{{ order.total.toLocaleString('en-IN') }}</strong>
                </td>
                <td>
                  <span class="table-status-pill" :class="getStatusClass(order.status)">
                    {{ order.status }}
                  </span>
                </td>
                <td class="date-cell">{{ new Date(order.createdOn).toLocaleDateString() }}</td>
              </tr>
            </tbody>
          </table>
        </div>
      </section>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import api from '../api/client'

const dashboard = ref<any>(null)

async function loadDashboard() {
  try {
    const response = await api.get('/admin/dashboard')
    dashboard.value = response.data
  } catch (e) {
    console.error('Failed to load dashboard', e)
  }
}

function getStatusClass(status: string) {
  const s = (status || '').toLowerCase()
  if (s.includes('delivered')) return 'status-delivered'
  if (s.includes('shipped')) return 'status-shipped'
  if (s.includes('packed')) return 'status-packed'
  if (s.includes('cancel')) return 'status-cancelled'
  return 'status-placed'
}

onMounted(() => {
  loadDashboard()
})
</script>

<style scoped>
.admin-dashboard-page {
  min-height: 100vh;
  padding: 3.5rem 0 6rem;
  background-color: var(--bg-main);
  color: var(--text-primary);
}

.admin-header {
  margin-bottom: 3rem;
}

.admin-eyebrow {
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

.admin-title {
  font-family: var(--font-display);
  font-size: clamp(2.8rem, 5.5vw, 4.4rem);
  font-weight: 900;
  line-height: 0.95;
  letter-spacing: -0.04em;
  color: #ffffff;
  margin-bottom: 0.8rem;
}

.name-gradient {
  background: linear-gradient(135deg, #ffffff 40%, #ec4899 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
}

.admin-subtitle {
  font-size: 1.05rem;
  color: var(--text-secondary);
}

/* Stats Grid */
.stats-grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 1.5rem;
  margin-bottom: 4rem;
}

.stat-card {
  padding: 1.8rem;
  border-radius: var(--radius-xl);
  display: flex;
  flex-direction: column;
  justify-content: space-between;
}

.stat-card-top {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 0.8rem;
}

.stat-label {
  font-size: 0.84rem;
  font-weight: 600;
  color: var(--text-muted);
}

.stat-icon-box {
  font-size: 1.25rem;
}

.stat-main-value {
  font-family: var(--font-display);
  font-size: 2.4rem;
  font-weight: 900;
  color: #ffffff;
  letter-spacing: -0.02em;
  line-height: 1.1;
  margin-bottom: 0.5rem;
}

.stat-trend {
  font-size: 0.78rem;
  color: var(--text-muted);
}

.stat-trend.positive {
  color: var(--accent-lime);
}

.warning-stat {
  border-color: rgba(245, 158, 11, 0.3);
}

.warning-stat.alert-glow {
  box-shadow: 0 0 25px rgba(245, 158, 11, 0.15);
}

.warning-text {
  color: #fbbf24;
}

.stat-trend.warning {
  color: #f59e0b;
}

/* Modules Grid */
.admin-section-block {
  margin-bottom: 4rem;
}

.section-title {
  font-family: var(--font-display);
  font-size: 1.6rem;
  font-weight: 850;
  color: #ffffff;
  letter-spacing: -0.02em;
  margin-bottom: 0.4rem;
}

.section-desc {
  font-size: 0.88rem;
  color: var(--text-muted);
  margin-bottom: 1.5rem;
}

.admin-modules-grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 1.25rem;
  margin-top: 1.5rem;
}

.module-card {
  padding: 1.6rem;
  border-radius: var(--radius-lg);
  display: flex;
  align-items: center;
  gap: 1.2rem;
  text-decoration: none;
  color: inherit;
  transition: all var(--transition-base);
}

.module-card:hover {
  transform: translateY(-4px);
  border-color: rgba(255, 255, 255, 0.25);
}

.module-icon {
  font-size: 1.8rem;
  flex-shrink: 0;
}

.module-card h3 {
  font-size: 1.05rem;
  font-weight: 800;
  color: #ffffff;
  margin-bottom: 0.2rem;
}

.module-card p {
  font-size: 0.78rem;
  color: var(--text-muted);
  margin: 0;
  line-height: 1.4;
}

.module-arrow {
  margin-left: auto;
  color: var(--text-muted);
  transition: transform var(--transition-fast);
}

.module-card:hover .module-arrow {
  color: #ffffff;
  transform: translateX(4px);
}

/* Table */
.table-header-row {
  display: flex;
  align-items: flex-end;
  justify-content: space-between;
  margin-bottom: 1.5rem;
}

.btn-sm {
  padding: 0.5rem 1.1rem;
  font-size: 0.82rem;
}

.table-container {
  overflow-x: auto;
  border-radius: var(--radius-xl);
}

.nova-admin-table {
  width: 100%;
  border-collapse: collapse;
  text-align: left;
}

.nova-admin-table th {
  padding: 1.2rem 1.5rem;
  font-size: 0.75rem;
  font-weight: 800;
  text-transform: uppercase;
  letter-spacing: 0.08em;
  color: var(--text-muted);
  border-bottom: 1px solid var(--border-subtle);
}

.nova-admin-table td {
  padding: 1.2rem 1.5rem;
  font-size: 0.9rem;
  border-bottom: 1px solid rgba(255, 255, 255, 0.04);
}

.nova-admin-table tr:hover td {
  background: rgba(255, 255, 255, 0.02);
}

.order-num-link {
  color: #ffffff;
}

.customer-email-cell {
  color: var(--text-secondary);
}

.order-price-val {
  color: #ffffff;
}

.table-status-pill {
  display: inline-block;
  padding: 0.25rem 0.65rem;
  border-radius: var(--radius-pill);
  font-size: 0.75rem;
  font-weight: 700;
}

.date-cell {
  color: var(--text-muted);
  font-size: 0.82rem;
}

.status-delivered { background: rgba(34, 197, 94, 0.15); color: #4ade80; }
.status-shipped { background: rgba(6, 182, 212, 0.15); color: #22d3ee; }
.status-packed { background: rgba(245, 158, 11, 0.15); color: #fbbf24; }
.status-placed { background: rgba(168, 85, 247, 0.15); color: #c084fc; }
.status-cancelled { background: rgba(239, 68, 68, 0.15); color: #f87171; }

@media (max-width: 1024px) {
  .stats-grid {
    grid-template-columns: repeat(2, 1fr);
  }
  .admin-modules-grid {
    grid-template-columns: repeat(2, 1fr);
  }
}

@media (max-width: 640px) {
  .stats-grid {
    grid-template-columns: 1fr;
  }
  .admin-modules-grid {
    grid-template-columns: 1fr;
  }
}
</style>
