<template>
  <div class="admin-view-page">
    <div class="container">
      <router-link to="/admin" class="admin-back-btn">
        <span>← Back to Mission Control</span>
      </router-link>

      <header class="admin-view-header">
        <div>
          <span class="badge-pill badge-lime">FULFILLMENT DISPATCH</span>
          <h1 class="admin-view-title">Manage Orders</h1>
        </div>

        <div style="display: flex; gap: 1rem; align-items: center;">
          <select v-model="statusFilter" @change="loadOrders" class="btn-pill btn-pill-outline" style="appearance: none; cursor: pointer;">
            <option value="" style="background: #14131f;">All Statuses</option>
            <option value="Placed" style="background: #14131f;">Placed</option>
            <option value="Packed" style="background: #14131f;">Packed</option>
            <option value="Shipped" style="background: #14131f;">Shipped</option>
            <option value="Delivered" style="background: #14131f;">Delivered</option>
            <option value="Cancelled" style="background: #14131f;">Cancelled</option>
          </select>
        </div>
      </header>

      <div class="admin-table-panel">
        <table class="data-table">
          <thead>
            <tr>
              <th>Order Number</th>
              <th>Customer</th>
              <th>Total</th>
              <th>Current Status</th>
              <th>Date</th>
              <th>Actions / Update</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="order in orders" :key="order.id">
              <td><strong>#{{ order.orderNumber }}</strong></td>
              <td>{{ order.email }}</td>
              <td><strong>₹{{ order.total.toLocaleString('en-IN') }}</strong></td>
              <td>
                <span class="table-status-pill" :class="getStatusClass(order.status)">
                  {{ order.status }}
                </span>
              </td>
              <td>{{ new Date(order.createdOn).toLocaleDateString() }}</td>
              <td>
                <button @click="viewOrder(order)" class="admin-action-btn">View</button>
                <select v-model="order.status" @change="updateStatus(order)" class="admin-action-btn" style="cursor: pointer;">
                  <option value="Placed" style="background: #14131f;">Placed</option>
                  <option value="Packed" style="background: #14131f;">Packed</option>
                  <option value="Shipped" style="background: #14131f;">Shipped</option>
                  <option value="Delivered" style="background: #14131f;">Delivered</option>
                  <option value="Cancelled" style="background: #14131f;">Cancelled</option>
                </select>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <!-- Order Details Modal -->
      <div class="admin-modal-backdrop" v-if="selectedOrder" @click.self="selectedOrder = null">
        <div class="admin-modal-window">
          <div class="admin-modal-header">
            <h2>Order Details #{{ selectedOrder.orderNumber }}</h2>
            <button class="admin-action-btn" @click="selectedOrder = null">✕</button>
          </div>

          <div style="display: flex; flex-direction: column; gap: 0.6rem; margin-bottom: 1.5rem; color: var(--text-secondary); font-size: 0.9rem;">
            <p><strong style="color: #fff;">Customer:</strong> {{ selectedOrder.email }}</p>
            <p><strong style="color: #fff;">Grand Total:</strong> ₹{{ selectedOrder.total.toLocaleString('en-IN') }}</p>
            <p><strong style="color: #fff;">Status:</strong> {{ selectedOrder.status }}</p>
            <p><strong style="color: #fff;">Placed on:</strong> {{ new Date(selectedOrder.createdOn).toLocaleString() }}</p>
          </div>

          <h3 style="font-size: 1.1rem; color: #fff; margin-bottom: 1rem;">Order Items</h3>
          <div class="admin-table-panel">
            <table>
              <thead>
                <tr>
                  <th>Product</th>
                  <th>Qty</th>
                  <th>Unit Price</th>
                  <th>Total</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="item in selectedOrder.items" :key="item.id">
                  <td>{{ item.productName }}</td>
                  <td>{{ item.quantity }}</td>
                  <td>₹{{ item.unitPrice }}</td>
                  <td>₹{{ item.lineTotal }}</td>
                </tr>
              </tbody>
            </table>
          </div>

          <div class="admin-form-actions">
            <button type="button" @click="selectedOrder = null" class="btn-pill btn-pill-primary">Close</button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import api from '../../api/client'

const orders = ref<any[]>([])
const statusFilter = ref('')
const selectedOrder = ref<any>(null)

async function loadOrders() {
  const url = statusFilter.value 
    ? `/admin/orders?status=${statusFilter.value}`
    : '/admin/orders'
  const response = await api.get(url)
  orders.value = response.data.orders
}

async function updateStatus(order: any) {
  await api.put(`/admin/orders/${order.id}/status`, { status: order.status })
  loadOrders()
}

function viewOrder(order: any) {
  selectedOrder.value = order
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
  loadOrders()
})
</script>

<style scoped>
.table-status-pill {
  display: inline-block;
  padding: 0.25rem 0.65rem;
  border-radius: var(--radius-pill);
  font-size: 0.75rem;
  font-weight: 700;
}

.status-delivered { background: rgba(34, 197, 94, 0.15); color: #4ade80; }
.status-shipped { background: rgba(6, 182, 212, 0.15); color: #22d3ee; }
.status-packed { background: rgba(245, 158, 11, 0.15); color: #fbbf24; }
.status-placed { background: rgba(168, 85, 247, 0.15); color: #c084fc; }
.status-cancelled { background: rgba(239, 68, 68, 0.15); color: #f87171; }
</style>
