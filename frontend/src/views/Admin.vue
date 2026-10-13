<template>
  <div class="admin">
    <p class="eyebrow">NOVAKART CONTROL ROOM</p><h1>Good morning,<br><span>Admin.</span></h1>
    <p class="intro">A clear view of your store, from today’s revenue to the orders that need attention.</p>

    <div class="stats-grid" v-if="dashboard">
      <div class="stat-card">
        <h3>Total Revenue</h3>
        <p class="stat-value">₹{{ dashboard.totalRevenue.toLocaleString() }}</p>
      </div>
      <div class="stat-card">
        <h3>Monthly Revenue</h3>
        <p class="stat-value">₹{{ dashboard.monthlyRevenue.toLocaleString() }}</p>
      </div>
      <div class="stat-card">
        <h3>Total Orders</h3>
        <p class="stat-value">{{ dashboard.totalOrders }}</p>
      </div>
      <div class="stat-card">
        <h3>Total Customers</h3>
        <p class="stat-value">{{ dashboard.totalCustomers }}</p>
      </div>
      <div class="stat-card">
        <h3>Total Products</h3>
        <p class="stat-value">{{ dashboard.totalProducts }}</p>
      </div>
      <div class="stat-card warning">
        <h3>Low Stock</h3>
        <p class="stat-value">{{ dashboard.lowStock }}</p>
      </div>
    </div>

    <div class="admin-menu">
      <router-link to="/admin/products" class="admin-link">Manage Products</router-link>
      <router-link to="/admin/orders" class="admin-link">Manage Orders</router-link>
      <router-link to="/admin/categories" class="admin-link">Manage Categories</router-link>
      <router-link to="/admin/brands" class="admin-link">Manage Brands</router-link>
      <router-link to="/admin/customers" class="admin-link">Manage Customers</router-link>
      <router-link to="/admin/coupons" class="admin-link">Manage Coupons</router-link>
      <router-link to="/admin/reviews" class="admin-link">Manage Reviews</router-link>
      <router-link to="/admin/messages" class="admin-link">Contact Messages</router-link>
      <router-link to="/admin/newsletter" class="admin-link">Newsletter</router-link>
    </div>

    <div class="recent-orders" v-if="dashboard && dashboard.recentOrders">
      <h2>Recent Orders</h2>
      <table>
        <thead>
          <tr>
            <th>Order #</th>
            <th>Email</th>
            <th>Total</th>
            <th>Status</th>
            <th>Date</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="order in dashboard.recentOrders" :key="order.id">
            <td>{{ order.orderNumber }}</td>
            <td>{{ order.email }}</td>
            <td>₹{{ order.total }}</td>
            <td>{{ order.status }}</td>
            <td>{{ new Date(order.createdOn).toLocaleDateString() }}</td>
          </tr>
        </tbody>
      </table>
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

onMounted(() => {
  loadDashboard()
})
</script>

<style scoped>
.admin {
  padding: 4rem max(1.5rem, calc((100% - 1120px) / 2));
  background:#fbfaff; color:#24145f; min-height:calc(100vh - 100px);
  margin: 0 auto;
}
.eyebrow { color:#9654db; font-size:.68rem; font-weight:800; letter-spacing:.18em; }
.admin h1 { font-size:clamp(2.8rem,6vw,5rem); line-height:.9; letter-spacing:-.08em; margin:.5rem 0 1rem; }
.admin h1 span { color:#9654db; }
.intro { color:#796e96; max-width:520px; margin-bottom:2.5rem; }

.stats-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));
  gap: 1rem;
  margin: 2rem 0;
}

.stat-card {
  background: #fff; border:1px solid #e8e3f1;
  padding: 1.5rem;
  border-radius: 8px;
  text-align: center;
}

.stat-card.warning {
  background: #fff3cd;
  border: 1px solid #ffc107;
}

.stat-card h3 {
  margin: 0 0 0.5rem 0;
  font-size: 0.9rem;
  color: #666;
}

.stat-value {
  font-size: 1.5rem;
  font-weight: bold;
  color: #24145f;
}

.admin-menu {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));
  gap: 1rem;
  margin-bottom: 2rem;
}

.admin-link {
  background: #24145f;
  color: white;
  padding: 1.5rem;
  text-align: center;
  text-decoration: none;
  border-radius: 8px;
  display: block;
}

.recent-orders {
  background: #fff; border:1px solid #e8e3f1;
  padding: 2rem;
  border-radius: 8px;
}

.recent-orders table {
  width: 100%;
  border-collapse: collapse;
}

.recent-orders th,
.recent-orders td {
  padding: 0.75rem;
  text-align: left;
  border-bottom: 1px solid #ddd;
}

.recent-orders th {
  background: #0fe1d1; color:#24145f;
  color: white;
}
</style>
